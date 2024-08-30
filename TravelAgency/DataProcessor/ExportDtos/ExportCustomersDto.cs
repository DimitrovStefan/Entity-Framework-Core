namespace TravelAgency.DataProcessor.ExportDtos
{
    public class ExportCustomersDto
    {
        public string FullName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public ExportBookingsToCustomerDto[] Bookings { get; set; }


    }
}
