using Basen.Models;
using BasenProjekt.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Raven.Client.Documents.Session;

namespace Basen.Pages
{
    public class KarnetyModel : PageModel
    {
        private readonly IKarnetRepo _karnetRepo;
        private readonly ILogger<KarnetyModel> _logger;

        public KarnetyModel(IKarnetRepo karnetRepo, ILogger<KarnetyModel> logger)
        {
            _karnetRepo = karnetRepo;
            _logger = logger;
        }

        [BindProperty]
        public List<Karnet> Karnety { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Karnety = await _karnetRepo.PobierzWszystkieKarnetyAsync();
            return Page();
        }

        public async Task<IActionResult> OnGetPobierz(string id)
        {
            try
            {
                var karnet = await _karnetRepo.PobierzKarnet(id);
                if (karnet == null)
                {
                    return NotFound();
                }
                return new JsonResult(karnet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDodaj([FromBody] Karnet karnet)
        {
            
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    await _karnetRepo.DodajKarnet(karnet);
                    return RedirectToPage();
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Wystąpił wyjątek podczas dodawania karnetu: {@karnet}", karnet);
                throw;
            }

            Karnety = await _karnetRepo.PobierzWszystkieKarnetyAsync();
            return Page();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostEdytuj([FromBody] Karnet karnet)
        {
            if (ModelState.IsValid)
            {
                await _karnetRepo.EdytujKarnet(karnet);
                return RedirectToPage();
            }

            Karnety = await _karnetRepo.PobierzWszystkieKarnetyAsync();
            return Page();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostUsun(string id)
        {
            try
            {
                await _karnetRepo.UsunKarnet(id);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Wystąpił błąd podczas usuwania karnetu o ID: {id}");
                return Page();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostSearch(string searchTerm)
        {
            Karnety = await _karnetRepo.WyszukajKarnetyAsync(searchTerm);
            return Page();
        }

        public async Task<IActionResult> OnPostClearSearch()
        {
            Karnety = await _karnetRepo.PobierzWszystkieKarnetyAsync();
            return Page();
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnGetEnd()
        {
            Karnety = await _karnetRepo.ZakonczoneAsync();
            return Page();
        }
    }
}
