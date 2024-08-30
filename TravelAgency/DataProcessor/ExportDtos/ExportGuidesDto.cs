using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ExportDtos
{
    [XmlType("Guide")]
    public class ExportGuidesDto
    {
        [XmlElement(nameof(FullName))]
        public string FullName { get; set; } = null!;

        [XmlArray(nameof(TourPackages))]
        public ExportTourPackageGuideDto[] TourPackages { get; set; } 

    }
}
