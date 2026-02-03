using System.ComponentModel.DataAnnotations;

namespace Webster.Models.Entities
{
    public class Manager
    {
        [Key]
        public int ManagerId { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;
    }
}
