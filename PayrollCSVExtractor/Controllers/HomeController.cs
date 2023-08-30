using Microsoft.AspNetCore.Mvc;
using PayrollCSVExtractor.Models;
using System.Diagnostics;
using CsvHelper;
using System.Globalization;

namespace PayrollCSVExtractor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //used httpPost request to handle the request
        [HttpPost]
        public ActionResult Index(IFormFile payrollcsv) {

            //innitated list from model to insert data from csv file to send back to dysplay in view https://www.youtube.com/watch?v=LECH7G4mmfs
            List<Employee> EmployeesToShow = new List<Employee>();

            // if the csv file is not null then proceed to use csvHelper to parse document and add it to the list above
            if (payrollcsv != null)
            {
                try
                {
                    // using csvHelper to append information from csv file into list https://joshclose.github.io/CsvHelper/
                    //found out about the streamreader here https://stackoverflow.com/questions/11086942/using-csvhelper-on-file-upload pulls the uploaded file
                    using (var reader = new StreamReader(payrollcsv.OpenReadStream()))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        var employeeList = csv.GetRecords<Employee>();

                        foreach (var item in employeeList)
                        {
                            Employee Employees = new Employee
                            {
                                EmployeeId = item.EmployeeId,
                                FirstName = item.FirstName,
                                LastName = item.LastName,
                                Payroll = item.Payroll,
                                Error = item.Error
                            };

                            EmployeesToShow.Add(Employees);
                        }
                    }
                }
                // handling the exception error to show error message in the view, found it here: https://www.youtube.com/watch?v=DyFgB878pOI&t=16s
                catch (CsvHelperException) {
                    TempData["error"] = "File Uploaded has wrong fields. Please make sure that the file contains these fields: Employee ID, First Name, Last Name, Payroll, Error.";
                    return View(); 
                }
                {
                    return View(EmployeesToShow);
                }
            }
            TempData["error"] = "File Uploaded has wrong fields. Please make sure that the file contains these fields: Employee ID, First Name, Last Name, Payroll, Error.";
            return View();
        
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}