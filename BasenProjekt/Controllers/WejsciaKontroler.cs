using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basen.Models;
using Basen.Repositories;
using BasenProjekt.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Basen.Pages
{
    public class WejsciaModel : PageModel
    {
        private readonly IWejsciaRepo _wejsciaRepository;
        private readonly ILogger<WejsciaModel> _logger;

        public WejsciaModel(IWejsciaRepo wejsciaRepository, ILogger<WejsciaModel> logger)
        {
            _wejsciaRepository = wejsciaRepository;
            _logger = logger;
        }

        public List<Wejscie> Wejscia { get; set; }
        public List<Klient> Klienci { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Wejscia = await _wejsciaRepository.PobierzWszystkieWejsciaAsync();
            Klienci = await _wejsciaRepository.PobierzWszystkichKlientowAsync();
            return Page();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDodaj([FromBody] Wejscie wejscie)
        {
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                wejscie.Klient = await _wejsciaRepository.PobierzKlienta(wejscie.klientId);
                await _wejsciaRepository.DodajWejscie(wejscie);
                _logger.LogInformation($"Dodano wejscie o ID: {wejscie.Id}");
                return RedirectToPage();
            }

            foreach (var modelStateKey in ModelState.Keys)
            {
                var modelStateVal = ModelState[modelStateKey];
                foreach (var error in modelStateVal.Errors)
                {
                    _logger.LogError($"Błąd walidacji modelu: Key={modelStateKey}, Error={error.ErrorMessage}");
                }
            }

            Wejscia = await _wejsciaRepository.PobierzWszystkieWejsciaAsync();
            return Page();
        }

        public async Task<IActionResult> OnGetPobierz(string id)
        {
            try
            {
                var wejscie = await _wejsciaRepository.PobierzWejscie(id);
                if (wejscie == null)
                {
                    _logger.LogError($"Nie znaleziono klienta o ID: {id}");
                    return NotFound();
                }
                return new JsonResult(wejscie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Wystąpił błąd podczas pobierania klienta o ID: {id}");
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostEdytuj([FromBody] Wejscie wejscie)
        {
            _logger.LogInformation($"Otrzymano żądanie edycji dla wejścia o ID: {wejscie.Id}");
            wejscie.Klient = await _wejsciaRepository.PobierzKlienta(wejscie.klientId);
            if (ModelState.IsValid)
            {
                await _wejsciaRepository.EdytujWejscie(wejscie);
                return RedirectToPage();
            }

            Wejscia = await _wejsciaRepository.PobierzWszystkieWejsciaAsync();
            return Page();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostUsun(string id)
        {
            try
            {
                await _wejsciaRepository.UsunWejscie(id);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Wystąpił błąd podczas usuwania wejścia o ID: {id}");
            }

            Wejscia = await _wejsciaRepository.PobierzWszystkieWejsciaAsync();
            return Page();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostSearch(string searchTerm)
        {
            Wejscia = await _wejsciaRepository.WyszukajWejsciaAsync(searchTerm);
            return Page();
        }

        public async Task<IActionResult> OnPostClearSearch()
        {
            Wejscia = await _wejsciaRepository.PobierzWszystkieWejsciaAsync();
            return Page();
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnGetEnd()
        {
            Wejscia = await _wejsciaRepository.ZakonczoneAsync();
            return Page();
        }
    }
}