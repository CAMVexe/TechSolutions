using System;
using System.Collections.Generic;

namespace TechSolutions.Models;

public partial class Producto
{
    public string IdProd { get; set; } = null!;

    public string? Nombre { get; set; }

    public int? Precio { get; set; }

    public int? Stock { get; set; }
}
