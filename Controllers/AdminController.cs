using ASP_P42.Data;
using ASP_P42.Models.Admin;
using ASP_P42.Services.Storage;
using Microsoft.AspNetCore.Mvc;

namespace ASP_P42.Controllers
{
    public class AdminController(IStorageService storageService, DataContext dataContext) : Controller
    {
        private readonly IStorageService _storageService = storageService;
        private readonly DataContext _dataContext = dataContext;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Group()
        {
            AdminGroupViewModel viewModel = new()
            {
                Groups = _dataContext.ProductGroups.ToList(),
            };
            return View();
        }

        [HttpPost]
        public IActionResult AddGroup(AdminAddGroupFormModel formModel)
        {
            try
            {
                return Ok( _storageService.Save(formModel.Image) );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
