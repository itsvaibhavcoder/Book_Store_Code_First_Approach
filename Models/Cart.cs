using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class CartItems
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; } // Represents the user who owns the cart item

        [Required]
        public int BookId { get; set; } // Book added to the cart

        [Required]
        public int Quantity { get; set; }

        [ForeignKey("BookId")]
        public Book Book { get; set; }
    }
}
