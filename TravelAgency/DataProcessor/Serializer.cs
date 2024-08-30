using Newtonsoft.Json;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ExportDtos;
using TravelAgency.Helper;

namespace TravelAgency.DataProcessor
{
    public class Serializer
    {
        public static string ExportGuidesWithSpanishLanguageWithAllTheirTourPackages(TravelAgencyContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();
            const string xmlRoot = "Guides";

            ExportGuidesDto[] guidesToExport = context.Guides
                .Where(g => g.TourPackagesGuides.Any(tg => tg.Guide.Language == Language.Spanish))
                .OrderByDescending(x => x.TourPackagesGuides.Count())
                .ThenBy(x => x.FullName)
                .Select(t => new ExportGuidesDto()
                {
                    FullName = t.FullName,
                    TourPackages = t.TourPackagesGuides
                    .Select(tg => new ExportTourPackageGuideDto()
                    {
                        Name = tg.TourPackage.PackageName,
                        Description = tg.TourPackage.Description,
                        Price = tg.TourPackage.Price
                    })
                    .OrderByDescending(x => x.Price)
                    .ThenBy(x => x.Name)
                    .ToArray()
                }).ToArray();


            return xmlHelper.Serialize(guidesToExport, xmlRoot);
        }

        public static string ExportCustomersThatHaveBookedHorseRidingTourPackage(TravelAgencyContext context)
        {
            var customersValid = context.Customers
           .Where(c => c.Bookings.Any(b => b.TourPackage.PackageName == "Horse Riding Tour"))
           .Select(c => new
           {
               c.FullName,
               c.PhoneNumber,
               Bookings = c.Bookings
                   .Where(b => b.TourPackage.PackageName == "Horse Riding Tour")
                   .Select(b => new
                   {
                       b.TourPackage.PackageName,
                       Date = b.BookingDate
                   })
                   .ToArray()
           })
           .ToArray();

            
            var customersToExport = customersValid
                .Select(c => new ExportCustomersDto()
                {
                    FullName = c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Bookings = c.Bookings
                        .Select(b => new ExportBookingsToCustomerDto()
                        {
                            TourPackageName = b.PackageName,
                            Date = b.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                        })
                        .OrderBy(b => b.Date)
                        .ToArray()
                })
                .OrderByDescending(c => c.Bookings.Length)
                .ThenBy(c => c.FullName)
                .ToArray();

            return JsonConvert.SerializeObject(customersToExport, Formatting.Indented);
        }
    }
}
