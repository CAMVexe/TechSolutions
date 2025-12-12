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

        public IActionResult Index()
        {
            var productos = _context.Productos.ToList();
            return View(productos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Producto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _context.Productos.Add(model);
            _context.SaveChanges();

            TempData["Mensaje"] = $"Producto '{model.Nombre}' registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult BuscarProd(string nombre)
        {
            var resultado = _context.Productos.Where(p => p.Nombre.Contains(nombre)).ToList();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                TempData["Mensaje"] = "Mostrando todos los productos, si desea buscar un producto en específico, ingrese el nombre";
                return RedirectToAction(nameof(Index));
            }

            if (resultado.Count == 0)
            {
                TempData["Mensaje"] = $"No se encontraron productos con el nombre '{nombre}'.";
                return RedirectToAction(nameof(Index));
            }

            return View("Index", resultado);
        }

        // GET: mostrar el formulario de edición

        public IActionResult Edit(string IdProd)
        {
            var current = _context.Productos.Find(IdProd);
            if (current == null)
            {
                TempData["Mensaje"] = $"No se encontró el producto con el identificador {IdProd}.";
                return RedirectToAction(nameof(Index));
            }

            return View(current);
        }

        // POST: guardar los cambios enviados desde la vista Edit
        [HttpPost]
        public IActionResult Edit(Producto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existing = _context.Productos.Find(model.IdProd);
            if (existing == null)
            {
                TempData["Mensaje"] = $"No se encontró el producto con el identificador {model.IdProd}.";
                return RedirectToAction(nameof(Index));
            }

            _context.Entry(existing).CurrentValues.SetValues(model); // Entry(r) = obtiene registro r | CurrentValues = valores actuales del registro | SetValues(m) = asigna los valores del modelo m
            _context.SaveChanges();

            TempData["Mensaje"] = $"Producto '{model.Nombre}' actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(string IdProd)
        {
            var producto = _context.Productos.Find(IdProd);

            if (producto == null)
            {
                TempData["Mensaje"] = $"No se encontró la persona con cédula {IdProd}.";
                return RedirectToAction(nameof(Index));
            }

            _context.Productos.Remove(producto);
            _context.SaveChanges();

            TempData["Mensaje"] = $"{producto.Nombre} está ahora fuera del inventario.";
            return RedirectToAction(nameof(Index));
        }

        // Reportes y Estadísticas

        public IActionResult OrderP()
        {
            var ordered = _context.Productos.OrderByDescending(p => p.Precio).ToList();
            return View("Index", ordered);
        }

        public IActionResult AvgPrecio()
        {
            var avg = _context.Productos.Average(p => p.Precio);
            TempData["Mensaje"] = $"El promedio del precio de los productos en inventario es de ₡ {avg:F2}";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ValInventario()
        {
            var totalValue = _context.Productos.Sum(p => p.Precio * p.Stock);
            TempData["Mensaje"] = $"El valor total del inventario es de ₡ {totalValue}";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ReStock()
        {
            var lowStockProducts = _context.Productos.Where(p => p.Stock < 5).ToList();
            if (lowStockProducts.Count == 0)
            {
                TempData["Mensaje"] = "No hay productos con stock crítico (stock menor a 5).";
                return RedirectToAction(nameof(Index));
            }
            TempData["Mensaje"] = "Productos que necesitan reabastecimiento (stock crítico, menor a 5):";
            return View("Index", lowStockProducts);
        }
    }
}

