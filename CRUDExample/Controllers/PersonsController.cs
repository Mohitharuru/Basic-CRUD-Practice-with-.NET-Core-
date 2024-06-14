using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;
using Rotativa.AspNetCore;

namespace CRUDExample.Controllers
{
    [Route("[controller]")]
    public class PersonsController : Controller
    {
        //private fields
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        public PersonsController(IPersonsService personsService, ICountriesService countriesService)
        {
            _personsService = personsService;
            _countriesService = countriesService;
        }

        [Route("index")]
        [Route("/")]
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            //Searching
            ViewBag.searchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.Email), "Email" },
                { nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                { nameof(PersonResponse.Gender), "Gender"},
                { nameof(PersonResponse.Address), "Address" },
                { nameof(PersonResponse.CountryID), "Country" },
            };
            List<PersonResponse> persons = await _personsService.GetFilteredPersons(searchBy, searchString);

            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //Sorting
            List<PersonResponse> sortedPersons = await _personsService.GetSortedPerson(persons, sortBy, sortOrder);
            ViewBag.sortBy = sortBy;
            ViewBag.sortOrder = sortOrder.ToString();

            return View(sortedPersons);
        }

        //for opening the view 
        [Route("create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse> countries = await _countriesService.GetAllCountries();
            ViewBag.countries = countries.Select(temp => new SelectListItem()
            {
                Text = temp.CountryName,
                Value = temp.CountryID.ToString()
            });
            return View();
        }

        //when user clicks on submit button 

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = await _countriesService.GetAllCountries();
                ViewBag.countries = countries;
                ViewBag.errors = ModelState.Values.SelectMany(v => v.Errors).Select(temp => temp.ErrorMessage).ToList();
                return View(personAddRequest);
            }
            PersonResponse personResponse = await _personsService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "Persons");
        }

        [Route("[action]/{personID}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? personID)
        {
            PersonResponse? personResponse = await _personsService.GetPersonByPersonID(personID);
            if (personResponse == null)
            {
                return RedirectToAction("Index", "Persons");
            }

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            List<CountryResponse> countries = await _countriesService.GetAllCountries();
            ViewBag.countries = countries.Select(temp => new SelectListItem()
            {
                Text = temp.CountryName,
                Value = temp.CountryID.ToString()
            });

            return View(personUpdateRequest);
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = await _personsService.GetPersonByPersonID(personUpdateRequest.PersonID);
            if (personResponse == null)
            {
                return RedirectToAction("Index", "Persons");
            }

            if (ModelState.IsValid)
            {
                PersonResponse updatedPerson = await _personsService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index", "Persons");
            }

            else
            {
                List<CountryResponse> countries = await _countriesService.GetAllCountries();
                ViewBag.countries = countries.Select(temp => new SelectListItem()
                {
                    Text = temp.CountryName,
                    Value = temp.CountryID.ToString()
                });
                ViewBag.errors = ModelState.Values.SelectMany(v => v.Errors).Select(temp => temp.ErrorMessage).ToList();
                return View(personResponse.ToPersonUpdateRequest());
            }
        }

        [HttpGet]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(Guid? personID)
        {
            PersonResponse? personResponse = await _personsService.GetPersonByPersonID(personID);
            if(personResponse == null)
            {
                return RedirectToAction("Index", "Persons");
            }
            return View(personResponse);
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = await _personsService.GetPersonByPersonID(personUpdateRequest.PersonID);
            if(personResponse == null)
            {
                return RedirectToAction("Index", "Persons");
            }
            bool DeletedPerson = await _personsService.DeletePerson(personUpdateRequest.PersonID);
            return RedirectToAction("Index", "Persons");
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsPDF()
        {
            //get list of persons
            List <PersonResponse> persons = await _personsService.GetAllPersons();

            //return view as pdf
            return new ViewAsPdf("PersonsPDF", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Right = 20,
                    Left = 20,
                    Bottom = 20
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream memoryStream = await _personsService.GetPersonsCSV();

            return File(memoryStream, "application/octet-stream", "persons.csv");
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream memoryStream = await _personsService.GetPersonsExcel();

            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }
    }
}
