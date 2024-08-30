using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ImportDtos
{
    [XmlType("Customer")]
    public class ImportCustomerDto
    {

        [Required]
        [StringLength(60, MinimumLength = 4)]
        [XmlElement(nameof(FullName))]
        public string FullName { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 6)]
        [XmlElement(nameof(Email))]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(13)]
        [RegularExpression(@"^\+\d{12}$")]
        [XmlAttribute("phoneNumber")]
        public string PhoneNumber { get; set; } = null!;

    }
}
