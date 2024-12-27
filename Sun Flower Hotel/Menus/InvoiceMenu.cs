using Spectre.Console;
using Sun_Flower_Hotel.Data;
using Sun_Flower_Hotel.Helper;
using Sun_Flower_Hotel.Model;
using System;

namespace Sun_Flower_Hotel.Menus
{
    public class InvoiceMenu
    {
        public static void DisplayMenu(HotelDbContext dbContext)
        {
            var service = new PaymentInvoiceService(dbContext);

            while (true)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new FigletText("Invoice Menu").Centered().Color(Color.Yellow));

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Select an option:[/]")
                        .AddChoices(
                            "Add Payment Invoice",
                            "View All Invoices",
                            "Update Payment Invoice",
                            "Delete Payment Invoice",
                            "Cancel Unpaid Bookings",
                            "Back to Main Menu"));

                if (choice == "Back to Main Menu") return;

                switch (choice)
                {
                    case "Add Payment Invoice":
                        AddInvoice(service);
                        break;
                    case "View All Invoices":
                        ViewAllInvoices(service);
                        break;
                    case "Update Payment Invoice":
                        UpdateInvoice(service);
                        break;
                    case "Delete Payment Invoice":
                        DeleteInvoice(service);
                        break;
                    case "Cancel Unpaid Bookings":
                        service.CancelUnpaidBookings();
                        break;
                }

                AnsiConsole.Markup("\n[green]Press any key to return to the menu...[/]");
                Console.ReadKey();
            }
        }

        private static void AddInvoice(PaymentInvoiceService service)
        {
            var invoice = new PaymentInvoice
            {
                BookingId = PromptInput<int>("Enter [yellow]Booking ID[/]:"),
                Amount = PromptInput<decimal>("Enter [yellow]Amount[/]:"),
                PaymentDate = DateTime.Today,
                PaymentMethod = PromptInput<string>("Enter [yellow]Payment Method[/]:"),
                PaymentStatus = PromptInput<string>("Enter [yellow]Payment Status[/]:"),
                InvoiceDate = DateTime.Today,
                InvoiceNotes = PromptInput<string>("Enter [yellow]Invoice Notes[/] (optional):", "")
            };

            service.AddInvoice(invoice);
        }

        private static void ViewAllInvoices(PaymentInvoiceService service)
        {
            AnsiConsole.Clear();
            AnsiConsole.Markup("[bold yellow]View All Invoices[/]\n");

            var invoices = service.GetAllInvoices();

            if (!invoices.Any())
            {
                AnsiConsole.Markup("[red]No invoices found.[/]\n");
                return;
            }

            var table = new Table()
                .Centered()
                .AddColumn("[cyan]Invoice ID[/]")
                .AddColumn("[cyan]Booking ID[/]")
                .AddColumn("[cyan]Amount[/]")
                .AddColumn("[cyan]Payment Date[/]")
                .AddColumn("[cyan]Payment Method[/]")
                .AddColumn("[cyan]Payment Status[/]")
                .AddColumn("[cyan]Invoice Notes[/]");

            foreach (var invoice in invoices)
            {
                table.AddRow(
                    invoice.PaymentInvoiceId.ToString(),
                    invoice.BookingId.ToString(),
                    $"{invoice.Amount:C}", // Formats as currency
                    invoice.PaymentDate?.ToShortDateString() ?? "Not Paid", // Handles null PaymentDate                    invoice.PaymentMethod ?? "N/A",
                    invoice.PaymentStatus ?? "N/A",
                    invoice.InvoiceNotes ?? "N/A"
                );
            }

            AnsiConsole.Write(table);
        }

        private static void UpdateInvoice(PaymentInvoiceService service)
        {
            var id = PromptInput<int>("Enter [yellow]Invoice ID[/] to update:");
            var newAmount = PromptInput<decimal>("Enter [yellow]New Amount[/]:");
            var newStatus = PromptInput<string>("Enter [yellow]New Status[/]:");
            var newMethod = PromptInput<string>("Enter [yellow]New Method[/]:");
            var newNotes = PromptInput<string>("Enter [yellow]New Notes[/] (optional):", "");

            service.UpdateInvoice(id, newAmount, newStatus, newMethod, newNotes);
        }

        private static void DeleteInvoice(PaymentInvoiceService service)
        {
            var id = PromptInput<int>("Enter [yellow]Invoice ID[/] to delete:");
            service.DeleteInvoice(id);
        }

        private static T PromptInput<T>(string message, T defaultValue = default)
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)AnsiConsole.Ask<string>(message, (string)(object)defaultValue);
            }

            return AnsiConsole.Ask<T>(message);
        }
    }
}
