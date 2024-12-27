﻿using PalmParadiseHotelApp.Menus.BookingMenu;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sun_Flower_Hotel.Menu.Booking_Menu
{
    public class BookingMenu
    {
        public void DisplayBookingMenu()
        {
            while (true)
            {
                AnsiConsole.Write(new FigletText("Booking Menu").Centered().Color(Color.Green));

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Select an option:[/]")
                        .AddChoices("Create a Booking", "View All Bookings", "Cancel a Booking", "Update a Booking", "Search Available Rooms",
                                      "Back to Main Menu"));

                switch (choice)
                {
                    case "Create a Booking":
                        new BookingCreation().CreateBooking();
                        break;
                    case "View All Bookings":
                        new BookingView().ViewAllBookings();
                        break;
                    case "Cancel a Booking":
                        new BookingCancellation().CancelBooking();
                        break;
                    case "Update a Booking":
                        new BookingUpdate().UpdateBooking();
                        break;
                    case "Search Available Rooms":
                        new BookingSearch().SearchAvailableRooms();
                        break;

                    case "Back to Main Menu":
                        return;
                }

                AnsiConsole.Markup("\n[green]Press any key to return to the menu...[/]");
                Console.ReadKey();
            }
        }
    }
}
