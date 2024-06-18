using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static OutOfOffice.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace OutOfOffice.Core.Entities
{
    public class Employee : IdentityUser<int>
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        public Subdivision Subdivision { get; set; }

        [Required]
        public Position Position { get; set; }

        [Required]
        public ActivityStatus Status { get; set; }

        [Required]
        public int PeoplePartnerId { get; set; }

        [Required]
        public int OutOfOfficeBalance { get; set; }

        public string? Photo { get; set; }
    }
}
