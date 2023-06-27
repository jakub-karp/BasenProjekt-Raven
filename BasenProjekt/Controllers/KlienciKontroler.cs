using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Basen.Models;
using Basen.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Basen.Pages
{
    public class KlienciModel : PageModel
    {
        private readonly IKlientRepo _klientRepository;
        private readonly ILogger<KlienciModel> _logger;

        public KlienciModel(IKlientRepo klientRepository, ILogger<KlienciModel> logger)
        {
            _klientRepository = klientRepository;
            _logger = logger;
        }

        public List<Klient> Klienci { get; set; }
        public List<Karnet> Karnety { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Klienci = await _klientRepository.PobierzKlientownaStronieAsync(1,5);
            Karnety = await _klientRepository.PobierzWszystkieKarnetyAsync();
            return Page();
        }
        public async Task<IActionResult> OnGetPobierz(string id)
        {
            try
            {
                var klient = await _klientRepository.PobierzKlienta(id);
                if (klient == null)
                {
                    _logger.LogError($"Nie znaleziono klienta o ID: {id}");
                    return NotFound();
                }
                return new JsonResult(klient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Wystąpił błąd podczas pobierania klienta o ID: {id}");
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDodaj([FromBody] Klient klient)
        {
            if (ModelState.IsValid)
            {
                klient.Karnet = await _klientRepository.PobierzKarnet(klient.KarnetId);
                if (klient.Karnet == null)
                {
                    _logger.LogError($"Nie znaleziono karnetu o ID: {klient.KarnetId}");
                    return NotFound();
                }

                await _klientRepository.DodajKlienta(klient);
                return RedirectToPage();
            }

            Klienci = await _klientRepository.PobierzWszystkichKlientowAsync();
            return Page();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostEdytuj([FromBody] Klient klient)
        {
            _logger.LogInformation($"Otrzymano żądanie edycji dla klienta o ID: {klient.Id}");
            if (ModelState.IsValid)
            {
                klient.Karnet = await _klientRepository.PobierzKarnet(klient.KarnetId);
                if (klient.Karnet == null)
                {
                    _logger.LogError($"Nie znaleziono karnetu o ID: {klient.KarnetId}");
                    return NotFound();
                }

                await _klientRepository.EdytujKlienta(klient);
                return RedirectToPage();
            }

            Klienci = await _klientRepository.PobierzWszystkichKlientowAsync();
            return Page();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostUsun(string id)
        {
            try
            {
                await _klientRepository.UsunKlienta(id);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Wystąpił błąd podczas usuwania klienta o ID: {id}");
            }

            Klienci = await _klientRepository.PobierzWszystkichKlientowAsync();
            return Page();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostSearch(string searchTerm)
        {
            Klienci = await _klientRepository.WyszukajKlientowAsync(searchTerm);
            return Page();
        }

        public async Task<IActionResult> OnPostClearSearch()
        {
            Klienci = await _klientRepository.PobierzWszystkichKlientowAsync();
            return Page();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostPobierzStrone(int aktualnastrona, int iloscstrona)
        {
            try
            {
                await Console.Out.WriteLineAsync(aktualnastrona.ToString() + ' ' + iloscstrona.ToString());
                Klienci = await _klientRepository.PobierzKlientownaStronieAsync(aktualnastrona, iloscstrona);
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Wystąpił błąd podczas pobierania klientów dla strony.");
                return BadRequest();
            }
        }
    }
}
