using Spectre.Console;
using Sun_Flower_Hotel.Data;
using Sun_Flower_Hotel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sun_Flower_Hotel.Menus
{
    public class FeedbackMenu
    {
        public void DisplayFeedbackMenu()
        {
            while (true)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new FigletText("Feedback Menu").Centered().Color(Color.Cyan1));

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Select an option:[/]")
                        .AddChoices("Add Feedback", "View Feedbacks", "Delete Feedback", "Back to Main Menu"));

                try
                {
                    switch (choice)
                    {
                        case "Add Feedback":
                            AddFeedback();
                            break;
                        case "View Feedbacks":
                            ViewFeedbacks();
                            break;
                        case "Delete Feedback":
                            DeleteFeedback();
                            break;
                        case "Back to Main Menu":
                            return; // Exit to Main Menu
                    }
                }
                catch (Exception ex)
                {
                    HandleError(ex);
                }

                AnsiConsole.Markup("\n[green]Press any key to return to the menu...[/]");
                Console.ReadKey();
            }
        }

        private static void HandleError(Exception ex)
        {
            AnsiConsole.Markup($"[red]Error: {ex.Message}[/]\n");
        }

        private void AddFeedback()
        {
            AnsiConsole.Clear();
            AnsiConsole.Markup("[bold yellow]Add Feedback[/]\n");

            try
            {
                var guestId = PromptInput<int>("Enter Guest ID:");
                var bookingId = PromptInput<int>("Enter Booking ID:");
                var employeeId = PromptInput<int?>("Enter Employee ID (or press Enter to skip):", null);
                var comments = PromptInput<string>("Enter Comments:");
                var rating = PromptInput<int>("Enter Rating (1-5):");

                if (rating < 1 || rating > 5)
                {
                    AnsiConsole.Markup("[red]Rating must be between 1 and 5.[/]\n");
                    return;
                }

                using (var context = new HotelDbContext())
                {
                    if (!context.Bookings.Any(b => b.BookingId == bookingId))
                    {
                        AnsiConsole.Markup("[red]Booking not found.[/]\n");
                        return;
                    }

                    var feedback = new Feedback
                    {
                        GuestId = guestId,
                        BookingId = bookingId,
                        EmployeeId = employeeId,
                        Comments = comments,
                        Rating = rating
                    };

                    context.Feedbacks.Add(feedback);
                    context.SaveChanges();
                    AnsiConsole.Markup("[green]Feedback added successfully![/]\n");
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void ViewFeedbacks()
        {
            AnsiConsole.Clear();
            AnsiConsole.Markup("[bold yellow]View Feedbacks[/]\n");

            using (var context = new HotelDbContext())
            {
                // Fetch all feedbacks
                var feedbacks = context.Feedbacks
                    .OrderBy(f => f.FeedbackId)
                    .ToList();

                // Handle empty feedback table
                if (!feedbacks.Any())
                {
                    AnsiConsole.Markup("[red]No feedbacks found.[/]\n");
                    return;
                }

                // Display feedbacks in a table
                var table = new Table();
                table.AddColumn("[cyan]Feedback ID[/]");
                table.AddColumn("[cyan]Guest ID[/]");
                table.AddColumn("[cyan]Booking ID[/]");
                table.AddColumn("[cyan]Employee ID[/]");
                table.AddColumn("[cyan]Rating[/]");
                table.AddColumn("[cyan]Comments[/]");

                foreach (var feedback in feedbacks)
                {
                    table.AddRow(
                    feedback.FeedbackId.ToString(),
                    feedback.GuestId.ToString(),
                    feedback.BookingId?.ToString() ?? "N/A",
                    feedback.EmployeeId?.ToString() ?? "N/A",
                    feedback.Rating.ToString(),
                    feedback?.Comments);
                }

                AnsiConsole.Write(table);
            }
        }
        private void DeleteFeedback()
        {
            AnsiConsole.Clear();
            AnsiConsole.Markup("[bold red]Delete Feedback[/]\n");

            try
            {
                var feedbackId = PromptInput<int>("Enter Feedback ID to delete:");

                using (var context = new HotelDbContext())
                {
                    var feedback = context.Feedbacks.Find(feedbackId);

                    if (feedback == null)
                    {
                        AnsiConsole.Markup("[red]Feedback not found.[/]\n");
                        return;
                    }

                    context.Feedbacks.Remove(feedback);
                    context.SaveChanges();
                    AnsiConsole.Markup("[green]Feedback deleted successfully![/]\n");
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private static T PromptInput<T>(string message, T defaultValue = default)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<T>(message)
                    .PromptStyle("yellow")
                    .DefaultValue(defaultValue));
        }
    }
}
