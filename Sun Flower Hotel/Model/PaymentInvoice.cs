using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sun_Flower_Hotel.Model
{
    public class PaymentInvoice
    {
        [Key]
        public int PaymentInvoiceId { get; set; } // Primary Key

        [ForeignKey(nameof(Booking))]
        public int BookingId { get; set; } // Foreign Key to Booking

        [Required]
        public decimal Amount { get; set; } // Total payment amount (includes room and services)

        [Required]
        public DateTime? PaymentDate { get; set; } // Date when payment was made

        [Required]
        [MaxLength(50)]
        public string ?PaymentMethod { get; set; } // e.g., Credit Card, Cash, Bank Transfer

        [MaxLength(20)]
        public string? PaymentStatus { get; set; } // e.g., Completed, Pending, Failed

        [Required]
        public DateTime InvoiceDate { get; set; } // Date when the invoice was generated

        [MaxLength(500)]
        public string? InvoiceNotes { get; set; } // Optional notes for the invoice

        // Navigation Property
        public Booking? Booking { get; set; } // Associated Booking

    }
}
