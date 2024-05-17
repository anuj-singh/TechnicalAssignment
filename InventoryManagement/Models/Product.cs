using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Please enter product name")]
        [MinLength(3,ErrorMessage ="Please enter at least 3 characters")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }
        [DisplayName("Created Date")]
        public DateOnly? CreatedDate { get; set; }
        [DisplayName("Updated Date")]
        public DateOnly UpdatedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    }
}
