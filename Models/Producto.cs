using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechSolutions.Models;

public partial class Producto
{
    [Required(ErrorMessage = "Es necesario ingresar el ID del producto para poder registrarlo en la base de datos")]
    [RegularExpression("^Prod-\\d+$", ErrorMessage = "El ID del producto debe ser 'Prod-' seguido del número correspondiente")]
    public string IdProd { get; set; } = null!;

    [Required(ErrorMessage = "Es necesario ingresar el nombre del producto para poder registrarlo en la base de datos")]
    [MinLength(3, ErrorMessage = "La cantidad mínima de caracteres del nombre del producto es de 3")]
    public string? Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Es necesario ingresar el precio del producto para poder registrarlo en la base de datos")]
    [Range(0, int.MaxValue, ErrorMessage = "El precio no puede ser negativo")]
    public int? Precio { get; set; } = 0;

    [Required(ErrorMessage = "Es necesario ingresar una cantidad en stock del producto para poder registrarlo en la base de datos")]
    [Range(0, int.MaxValue, ErrorMessage = "El precio no puede ser negativo")]
    public int? Stock { get; set; } = 0;
}
