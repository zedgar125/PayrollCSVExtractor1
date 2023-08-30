using CsvHelper.Configuration.Attributes;
using System.Security;
// model to hold employee payroll information
namespace PayrollCSVExtractor.Models
{
    public class Employee
    {
        [Name("Employee ID")]
        public string EmployeeId { get; set; }
        [Name("First Name")]
        public string FirstName { get; set;}
        [Name("Last Name")]
        public string LastName { get; set;}
        public int Payroll { get; set;}
        public string Error { get; set; }
    }
}
