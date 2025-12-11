using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TechSolutions.Models;

namespace TechSolutions.Controllers
{
    public class ProductosController : Controller
    {
        private readonly TechSolutionsContext _context;

        public ProductosController(TechSolutionsContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var productos = from p in _context.Productos select p;

            if (!string.IsNullOrEmpty(searchString))
                productos = productos.Where(p => p.Nombre != null && p.Nombre.Contains(searchString));

            return View(await productos.ToListAsync());
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdProd,Nombre,Precio,Stock")] Producto producto)
        {
            if (id != producto.IdProd) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.IdProd == id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Estadisticas()
        {
            var productos = await _context.Productos.ToListAsync();

            ViewData["ProductosOrdenados"] = productos
                .OrderByDescending(p => p.Precio ?? 0)
                .ToList();
            ViewData["PromedioPrecio"] = productos
                .Where(p => p.Precio.HasValue)
                .Average(p => p.Precio.Value);
            ViewData["ValorInventario"] = productos
                .Sum(p => (p.Precio ?? 0) * (p.Stock ?? 0));
            ViewData["StockCritico"] = productos
                .Where(p => (p.Stock ?? 0) < 5)
                .ToList();

            return View();
        }

        private bool ProductoExists(string id)
        {
            return _context.Productos.Any(e => e.IdProd == id);
        }
    }
}

