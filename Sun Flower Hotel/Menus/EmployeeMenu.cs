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
    public class EmployeeMenu
    {
        public void DisplayEmployeeMenu()
        {
            while (true)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new FigletText("Employee Menu").Centered().Color(Color.Green));

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Select an option:[/]")
                        .AddChoices("Add Employee", "View Employees", "Edit Employee", "Delete Employee", "Back to Main Menu"));

                if (choice == "Back to Main Menu") return;

                try
                {
                    switch (choice)
                    {
                        case "Add Employee":
                            AddEmployee();
                            break;
                        case "View Employees":
                            ViewEmployees();
                            break;
                        case "Edit Employee":
                            EditEmployee();
                            break;
                        case "Delete Employee":
                            DeleteEmployee();
                            break;
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

        private void AddEmployee()
        {
            AnsiConsole.Clear();
            AnsiConsole.Markup("[bold yellow]Add Employee[/]\n");

            using (var context = new HotelDbContext())
            {
                var name = AnsiConsole.Ask<string>("Enter [yellow]Employee Name[/]:");
                // Validate Contact Number
                string contactNumber;
                while (true)
                {
                    contactNumber = AnsiConsole.Ask<string>("Enter [yellow]Contact Number[/] (must be numeric):");
                    if (long.TryParse(contactNumber, out _)) // Validate that it's numeric
                        break;
                    AnsiConsole.Markup("[red]Contact number must be numeric. Please try again.[/]\n");
                }
                // Optional Email
                var email = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter [yellow]Email Address (optional)[/] (press Enter to skip):")
                        .AllowEmpty());

                // Role Selection
                var role = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select [yellow]Role[/] for the employee:")
                        .AddChoices(new[] { "Transport", "Food", "Receptionist", "Cleaner", "Driver", "Other" }));

                // Validate Required Fields
                if (string.IsNullOrWhiteSpace(name))
                {
                    AnsiConsole.Markup("[red]Employee name is required.[/]\n");
                    return;
                }

                if (string.IsNullOrWhiteSpace(contactNumber))
                {
                    AnsiConsole.Markup("[red]Contact number is required.[/]\n");
                    return;
                }

                var employee = new Employee
                {
                    Name = name,
                    Role = role,
                    ContactNumber = contactNumber,
                    Email = string.IsNullOrWhiteSpace(email) ? null : email
                };

                try
                {
                    context.Employees.Add(employee);
                    context.SaveChanges();
                    AnsiConsole.Markup("[green]Employee added successfully![/]\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                    }
                    AnsiConsole.Markup("[red]Failed to add employee. Please try again.[/]\n");
                }
            }
        }


        private void ViewEmployees()
        {
            AnsiConsole.Clear();
            AnsiConsole.Markup("[bold yellow]View Employees[/]\n");

            using (var context = new HotelDbContext())
            {
                var employees = context.Employees.ToList();
                if (!employees.Any())
                {
                    AnsiConsole.Markup("[red]No employees found.[/]\n");
                    return;
                }

                var table = new Table();
                table.AddColumn("[cyan]ID[/]");
                table.AddColumn("[cyan]Name[/]");
                table.AddColumn("[cyan]Role[/]");
                table.AddColumn("[cyan]Contact Number[/]");
                table.AddColumn("[cyan]Email[/]");

                foreach (var emp in employees)
                {
                    table.AddRow(
                        emp.EmployeeId.ToString(),
                        emp.Name,
                        emp.Role,
                        emp.ContactNumber ?? "",
                        emp.Email ?? "");
                }

                AnsiConsole.Write(table);
            }
        }

        private void EditEmployee()
        {
            AnsiConsole.Clear();
            AnsiConsole.Markup("[bold yellow]Edit Employee[/]\n");

            var employeeId = PromptInput<int>("Enter Employee ID to edit:");

            using (var context = new HotelDbContext())
            {
                var employee = context.Employees.Find(employeeId);
                if (employee == null)
                {
                    AnsiConsole.Markup("[red]Employee not found.[/]\n");
                    return;
                }

                employee.Name = PromptInput<string>($"Enter new Name (current: {employee.Name}):", employee.Name);
                employee.Role = PromptInput<string>($"Enter new Role (current: {employee.Role}):", employee.Role);

                context.SaveChanges();
                AnsiConsole.Markup("[green]Employee updated successfully![/]\n");
            }
        }

        private void DeleteEmployee()
        {
            AnsiConsole.Clear();
            AnsiConsole.Markup("[bold red]Delete Employee[/]\n");

            var employeeId = PromptInput<int>("Enter Employee ID to delete:");

            using (var context = new HotelDbContext())
            {
                var employee = context.Employees.Find(employeeId);
                if (employee == null)
                {
                    AnsiConsole.Markup("[red]Employee not found.[/]\n");
                    return;
                }

                context.Employees.Remove(employee);
                context.SaveChanges();
                AnsiConsole.Markup("[green]Employee deleted successfully![/]\n");
            }
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

