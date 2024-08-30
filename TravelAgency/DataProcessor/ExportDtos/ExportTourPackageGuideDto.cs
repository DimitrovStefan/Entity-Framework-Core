using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ExportDtos
{
    [XmlType("TourPackage")]
    public class ExportTourPackageGuideDto
    {
        [XmlElement(nameof(Name))]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(Description))]
        public string Description { get; set; } = null!;

        [XmlElement(nameof(Price))]
        public decimal Price { get; set; }
    }
}
