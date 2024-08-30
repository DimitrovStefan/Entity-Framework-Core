using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ImportDtos;
using TravelAgency.Helper;

namespace TravelAgency.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedCustomer = "Successfully imported customer - {0}";
        private const string SuccessfullyImportedBooking = "Successfully imported booking. TourPackage: {0}, Date: {1}";

        public static string ImportCustomers(TravelAgencyContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper xmlHelper = new XmlHelper();
            const string xmlRoot = "Customers";

            ICollection<Customer> customersToImport = new List<Customer>();

            ImportCustomerDto[] deserializedCustomers =
                xmlHelper.Deserialize<ImportCustomerDto[]>(xmlString, xmlRoot);

            foreach (ImportCustomerDto customerDto in deserializedCustomers)
            {
                if (!IsValid(customerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (customersToImport.Any(x => x.FullName == customerDto.FullName ||
                                           x.Email == customerDto.Email ||
                                           x.PhoneNumber == customerDto.PhoneNumber))
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                Customer newCustomer = new Customer()
                {
                    FullName = customerDto.FullName,
                    Email = customerDto.Email,
                    PhoneNumber = customerDto.PhoneNumber
                };

               
                customersToImport.Add(newCustomer);
                sb.AppendLine(String.Format(SuccessfullyImportedCustomer, newCustomer.FullName));
            }

            context.Customers.AddRange(customersToImport);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportBookings(TravelAgencyContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ICollection<Booking> bookingsToImport = new List<Booking>();

            ImportBookingDto[] deserializedBookings = JsonConvert.DeserializeObject<ImportBookingDto[]>(jsonString)!;

            foreach (ImportBookingDto bookingDto in deserializedBookings)
            {
                if (!DateTime.TryParseExact(bookingDto.BookingDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validBookDate))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var customer = context.Customers.FirstOrDefault(c => c.FullName == bookingDto.CustomerName);
                var tourPackage = context.TourPackages.FirstOrDefault(tp => tp.PackageName == bookingDto.TourPackageName);

                if (customer == null || tourPackage == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isDuplicate = context.Bookings.Any(b =>
                    b.BookingDate == validBookDate &&
                    b.CustomerId == customer.Id &&
                    b.TourPackageId == tourPackage.Id);

                if (isDuplicate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Booking newBooking = new Booking()
                {
                    BookingDate = validBookDate,
                    Customer = customer,
                    TourPackage = tourPackage
                };


                bookingsToImport.Add(newBooking);
                sb.AppendLine(String.Format(SuccessfullyImportedBooking, tourPackage.PackageName, validBookDate.ToString("yyyy-MM-dd")));

            }

            context.Bookings.AddRange(bookingsToImport);
            context.SaveChanges();

            return sb.ToString();

        }

        public static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validateContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                string currValidationMessage = validationResult.ErrorMessage;
            }

            return isValid;
        }
    }
}
