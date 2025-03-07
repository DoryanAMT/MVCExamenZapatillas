using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using MVCExamenZapatillas.Models;
using MVCExamenZapatillas.Repositories;

namespace MVCExamenZapatillas.Controllers
{
    public class ZapasController : Controller
    {
        private RepositoryZapas repo;
        public ZapasController(RepositoryZapas repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Zapatillas()
        {
            List<Zapa> zapas = await this.repo.GetZapasAsync();
            return View(zapas);
        }
        public async Task<IActionResult> Details
            (int idZapa)
        {
            ViewData["ZAPASELECCIONADA"] = await this.repo.FindZapaAsync(idZapa);
            return View();
        }
        public async Task<IActionResult> _ImagenesZapaOutPartial
            (int? posicion, int idZapa)
        {
            if (posicion == null)
            {
                posicion = 1;
                return View();
            }
            else
            {
                ModelImagenesZapa model = await
                    this.repo.GetImagenesZapaOutAsync(posicion.Value, idZapa);
                Zapa zapa = await this.repo.FindZapaAsync(idZapa);
                ViewData["ZAPATILLASELECIONADO"] = zapa;
                ViewData["REGISTROS"] = model.NumeroRegistros;
                ViewData["IDZAPA"] = idZapa;

                int siguiente = posicion.Value + 1;
                if (siguiente > model.NumeroRegistros)
                {
                    siguiente = model.NumeroRegistros;
                }
                int anterior = posicion.Value - 1;
                if (anterior < 1)
                {
                    anterior = 1;
                }
                ViewData["ULTIMO"] = model.NumeroRegistros;
                ViewData["SIGUIENTE"] = siguiente;
                ViewData["ANTERIOR"] = anterior;
                ViewData["POSICION"] = posicion;
                return PartialView("_ImagenesZapaOutPartial", model.Imagenes);
            }
        }
    }
}
