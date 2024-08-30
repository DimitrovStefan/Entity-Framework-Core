using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Data.Models
{
    public class TourPackage
    {

        public TourPackage()
        {
            Bookings = new List<Booking>();
            TourPackagesGuides = new List<TourPackageGuide>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string PackageName { get; set; } = null!;


        [MaxLength(200)]
        public string Description { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<TourPackageGuide> TourPackagesGuides { get; set; }
    }
}
