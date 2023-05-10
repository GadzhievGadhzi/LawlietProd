using ClosedXML.Excel;
using Lawliet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.EntityFrameworkCore.Metadata;


namespace Lawliet.Controllers {
    public class ExcelController : Controller {

        [HttpGet]
        public IActionResult Index(object _) => View();

        [HttpPost]
        public IActionResult Index() 
        {
            List<StudentViewModel> students = new List<StudentViewModel>();
            using (XLWorkbook workBook = new XLWorkbook(HttpContext.Request.Form.Files[0].OpenReadStream())) 
            {
                foreach (IXLWorksheet worksheet in workBook.Worksheets) 
                {
                    foreach (IXLRow row in worksheet.RowsUsed().Skip(1)) 
                    {
                        StudentViewModel student = new StudentViewModel();
                        var fio = row.Cell(1).Value.ToString().Split(' ');
                        student.LastName = fio[0];
                        student.FirstName = fio[1];
                        student.Patronomyc = fio[2];
                        student.Group = worksheet.Name;

                        student.Ratings.Add(Convert.ToInt32(row.Cell(2).Value.ToString()));
                        student.Ratings.Add(Convert.ToInt32(row.Cell(3).Value.ToString()));
                        student.Ratings.Add(Convert.ToInt32(row.Cell(4).Value.ToString()));
                        student.Ratings.Add(Convert.ToInt32(row.Cell(5).Value.ToString()));
                        students.Add(student);
                    }
                }
            }
            return View(students);
        }
    }
}