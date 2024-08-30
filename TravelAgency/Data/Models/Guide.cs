using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Data.Models
{
    public class Guide
    {

        public Guide()
        {
            TourPackagesGuides = new List<TourPackageGuide>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(4)]
        [MaxLength(60)]
        public string FullName { get; set; } = null!;

        [Required]
        public Language Language { get; set; }

        public virtual ICollection<TourPackageGuide> TourPackagesGuides { get; set; }

    }

    public enum Language 
    {
        English = 0,
        German,
        French,
        Spanish,
        Russian
    }
}
