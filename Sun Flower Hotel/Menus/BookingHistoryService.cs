using System;
using System.Linq;
using Spectre.Console;
using Sun_Flower_Hotel.Data;
using Sun_Flower_Hotel.Model;

namespace Sun_Flower_Hotel.Helper
{
    public class BookingHistoryService
    {
        private readonly HotelDbContext _dbContext;

        public BookingHistoryService(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void ViewBookingHistory()
        {
            var bookings = _dbContext.Bookings
                .OrderByDescending(b => b.CheckInDate)
                .ToList();

            if (!bookings.Any())
            {
                AnsiConsole.Markup("[red]No booking history found.[/]\n");
                return;
            }

            var table = new Table()
                .AddColumn("[cyan]Booking ID[/]")
                .AddColumn("[cyan]Guest Name[/]")
                .AddColumn("[cyan]Check-In Date[/]")
                .AddColumn("[cyan]Check-Out Date[/]")
                .AddColumn("[cyan]Room Number[/]");

            foreach (var booking in bookings)
            {
                var guest = _dbContext.Guests.FirstOrDefault(g => g.GuestId == booking.GuestId);
                table.AddRow(
                    booking.BookingId.ToString(),
                    guest?.Name ?? "N/A",
                    booking.CheckInDate.ToShortDateString(),
                    booking.CheckOutDate.ToShortDateString(),
                    booking.RoomNumber.ToString()
                );
            }

            AnsiConsole.Write(table);
        }
    }
}
