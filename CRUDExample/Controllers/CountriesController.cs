using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;

namespace CRUDExample.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        private readonly ICountriesService _countriesService;
        public CountriesController(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        [Route("[action]")]
        public async Task<IActionResult> UploadFromExcel()
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UploadFromExcel(IFormFile excelFile)
        {
            if(excelFile == null || excelFile.Length == 0)
            {
                ViewBag.ErrorMessage = "Please select an xslx file";
                return View();
            }

            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMessage = "unsupported file";
                return View();
            }
            int countriesCount = await _countriesService.UploadCountriesFromExcelFile(excelFile);
            ViewBag.Message = $"{countriesCount} Countries Uploaded";
            return View();
        }
    }
}
