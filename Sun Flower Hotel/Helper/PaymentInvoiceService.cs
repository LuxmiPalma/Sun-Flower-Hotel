using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sun_Flower_Hotel.Data;
using Sun_Flower_Hotel.Model;

namespace Sun_Flower_Hotel.Helper
{
    public class PaymentInvoiceService
    {
        private readonly HotelDbContext _context;

        public PaymentInvoiceService(HotelDbContext context)
        {
            _context = context;
        }

        public void AddInvoice(PaymentInvoice invoice)
        {
            try
            {
                _context.PaymentInvoices.Add(invoice);
                _context.SaveChanges();
                Console.WriteLine("Invoice added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding invoice: {ex.Message}");
            }
        }

        public List<PaymentInvoice> GetAllInvoices()
        {// Retrieve all invoices from the database
            var invoices = _context.PaymentInvoices.ToList();

            // Log the count of invoices retrieved
            //Console.WriteLine($"Debug: Found {invoices.Count} invoices in the database.");

            // Log details of each invoice
            foreach (var invoice in invoices)
            {
                //Console.WriteLine($"Debug: Invoice ID = {invoice.PaymentInvoiceId}, Booking ID = {invoice.BookingId}, Amount = {invoice.Amount}, Status = {invoice.PaymentStatus}");
            }

            return invoices;
        }

        public PaymentInvoice GetInvoiceById(int id)
        {
            return _context.PaymentInvoices.FirstOrDefault(i => i.PaymentInvoiceId == id);
        }

        public void UpdateInvoice(int id, decimal newAmount, string newStatus, string newMethod, string newNotes)
        {
            try
            {
                var invoice = GetInvoiceById(id);
                if (invoice != null)
                {
                    invoice.Amount = newAmount;
                    invoice.PaymentStatus = newStatus;
                    invoice.PaymentMethod = newMethod;
                    invoice.InvoiceNotes = newNotes;

                    _context.SaveChanges();
                    Console.WriteLine("Invoice updated successfully!");
                }
                else
                {
                    Console.WriteLine("Invoice not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating invoice: {ex.Message}");
            }
        }

        public void DeleteInvoice(int id)
        {
            try
            {
                var invoice = GetInvoiceById(id);
                if (invoice != null)
                {
                    _context.PaymentInvoices.Remove(invoice);
                    _context.SaveChanges();
                    Console.WriteLine("Invoice deleted successfully!");
                }
                else
                {
                    Console.WriteLine("Invoice not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting invoice: {ex.Message}");
            }
        }
        public void CancelUnpaidBookings()
        {
            try
            {
                Console.WriteLine("Checking for unpaid bookings...");

                var unpaidBookings = _context.Bookings
                    .Where(b => !b.PaymentInvoices.Any(pi => pi.PaymentStatus == "Completed") &&
                                EF.Functions.DateDiffDay(b.CheckInDate, DateTime.Today) > 10)
                    .ToList();

                if (!unpaidBookings.Any())
                {
                    Console.WriteLine("No unpaid bookings found.");
                    return;
                }

                foreach (var booking in unpaidBookings)
                {
                    Console.WriteLine($"Canceling Booking ID: {booking.BookingId} due to non-payment.");
                    _context.Bookings.Remove(booking);
                }

                _context.SaveChanges();
                Console.WriteLine("Unpaid bookings canceled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during unpaid bookings cancellation: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
        }

    }

}

