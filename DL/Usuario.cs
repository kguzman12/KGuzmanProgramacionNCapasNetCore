﻿using System;
using System.Collections.Generic;

namespace DL;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? Nombre { get; set; }

    public string? ApellidoPaterno { get; set; }

    public string? ApellidoMaterno { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? UserName { get; set; }

    public string? Sexo { get; set; }

    public string? Telefono { get; set; }

    public string? Celular { get; set; }

    public string? Curp { get; set; }

    public byte? IdRol { get; set; }
    public string? RolNombre { get; set; }

    public string? Imagen { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Direccion> Direccions { get; } = new List<Direccion>();

    public virtual Rol? IdRolNavigation { get; set; }


//Propiedades usuario
    public int IdDireccion { get; set; }
    public string? Calle { get; set; }
    public string? NumeroInterior { get; set; }
    public string? NumeroExterior { get; set; }

    public int IdColonia { get; set; }
    public string? NombreColonia { get; set; }

    public int IdMunicipio { get; set; }
    public string? NombreMunicipio { get; set; }

    public int IdEstado { get; set; }
    public string? NombreEstado { get; set; }

    public int IdPais { get; set; }
    public string? NombrePais { get; set; }
}
