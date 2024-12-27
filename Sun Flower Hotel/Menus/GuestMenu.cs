using Spectre.Console;
using Sun_Flower_Hotel.Data;
using Sun_Flower_Hotel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Sun_Flower_Hotel.Menus
{
    public class GuestMenu
    {
        public void DisplayGuestMenu()
        {
            while (true)
            {
                AnsiConsole.Write(new FigletText("Guest Menu").Centered().Color(Color.Yellow));

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Select an option:[/]")
                        .AddChoices("Add a New Guest", "View All Guests", "Edit Guest Information", "Delete a Guest", "Back to Main Menu"));

                switch (choice)
                {
                    case "Add a New Guest":
                        AddGuest();
                        break;
                    case "View All Guests":
                        ViewAllGuests();
                        break;
                    case "Edit Guest Information":
                        EditGuest();
                        break;
                    case "Delete a Guest":
                        DeleteGuest();
                        break;
                    case "Back to Main Menu":
                        return;
                }

                AnsiConsole.Markup("\n[green]Press any key to return to the menu...[/]");
                Console.ReadKey();
            }
        }

        private void AddGuest()
        {
            try
            {
                AnsiConsole.Clear();
                AnsiConsole.Markup("[bold yellow]Add New Guest[/]\n");

                var name = AnsiConsole.Ask<string>("Enter [yellow]Guest Name[/]:");
                var contactNumber = AnsiConsole.Ask<string>("Enter [yellow]Contact Number[/]:");
                var email = AnsiConsole.Ask<string>("Enter [yellow]Email[/]:");

                using (var context = new HotelDbContext())
                {
                    var guest = new Guest
                    {
                        Name = name,
                        ContactNumber = contactNumber,
                        Email = email
                    };

                    context.Guests.Add(guest);
                    context.SaveChanges();
                    AnsiConsole.Markup("[green]Guest added successfully![/]\n");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]Error: {ex.Message}[/]\n");
            }
        }

        private void ViewAllGuests()
        {
            try
            {
                AnsiConsole.Clear();
                AnsiConsole.Markup("[bold yellow]View All Guests[/]\n");

                using (var context = new HotelDbContext())
                {
                    var guests = context.Guests.ToList();

                    if (!guests.Any())
                    {
                        AnsiConsole.Markup("[red]No guests found![/]\n");
                        return;
                    }

                    var table = new Table();
                    table.AddColumn("[cyan]Guest ID[/]");
                    table.AddColumn("[cyan]Name[/]");
                    table.AddColumn("[cyan]Contact Number[/]");
                    table.AddColumn("[cyan]Email[/]");

                    foreach (var guest in guests)
                    {
                        table.AddRow(
                            guest.GuestId.ToString(),
                            guest.Name,
                            guest.ContactNumber,
                            guest.Email);
                    }

                    AnsiConsole.Write(table);
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]Error: {ex.Message}[/]\n");
            }
        }

        private void EditGuest()
        {
            try
            {
                AnsiConsole.Clear();
                AnsiConsole.Markup("[bold yellow]Edit Guest Info[/]\n");

                var guestId = AnsiConsole.Ask<int>("Enter [yellow]Guest ID[/] to edit:");

                using (var context = new HotelDbContext())
                {
                    var guest = context.Guests.Find(guestId);

                    if (guest == null)
                    {
                        AnsiConsole.Markup("[red]Guest not found![/]\n");
                        return;
                    }

                    guest.Name = AnsiConsole.Ask<string>(
                        $"Enter new [yellow]Name[/] (current: {guest.Name}):", guest.Name);
                    guest.ContactNumber = AnsiConsole.Ask<string>(
                        $"Enter new [yellow]Contact Number[/] (current: {guest.ContactNumber}):", guest.ContactNumber);
                    guest.Email = AnsiConsole.Ask<string>(
                        $"Enter new [yellow]Email[/] (current: {guest.Email}):", guest.Email);

                    context.SaveChanges();
                    AnsiConsole.Markup("[green]Guest updated successfully![/]\n");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]Error: {ex.Message}[/]\n");
            }
        }

        private void DeleteGuest()
        {
            try
            {
                AnsiConsole.Clear();
                AnsiConsole.Markup("[bold red]Delete a Guest[/]\n");

                var guestId = AnsiConsole.Ask<int>("Enter [yellow]Guest ID[/] to delete:");

                using (var context = new HotelDbContext())
                {
                    var guest = context.Guests
                        .Include(g => g.Bookings) // Kontrollera om det finns bokningar
                        .FirstOrDefault(g => g.GuestId == guestId);

                    if (guest == null)
                    {
                        AnsiConsole.Markup("[red]Guest not found![/]\n");
                        return;
                    }

                    if (guest.Bookings != null && guest.Bookings.Any())
                    {
                        AnsiConsole.Markup(
                            $"[red]Cannot delete Guest '{guest.Name}' because there are active bookings![/]\n");
                        return;
                    }

                    if (AnsiConsole.Confirm($"Are you sure you want to delete Guest [red]{guest.Name}[/]?") == false)
                    {
                        AnsiConsole.Markup("[yellow]Operation cancelled.[/]\n");
                        return;
                    }

                    context.Guests.Remove(guest);
                    context.SaveChanges();
                    AnsiConsole.Markup($"[green]Guest '{guest.Name}' deleted successfully![/]\n");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]Error: {ex.Message}[/]\n");
            }
        }
    }
}

