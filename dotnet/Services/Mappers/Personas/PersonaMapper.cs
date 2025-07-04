using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Services.Mappers
{
    public static class PersonasMapper
    {
        public static PersonaViewModel? toPersonaViewModel(this Cliente cliente)
        {
            if (cliente == null)
            {
                return null;
            }

            return new PersonaViewModel
            {
                Id = cliente.Codigo,
                RazonSocial = cliente.Razon,
                CodigoInterno = cliente.Interno,
                NombreFantasia = cliente.Descripcion,
                Ruc = cliente.Ruc,
                Direccion = cliente.Dir,
                Telefono = cliente.Tel,
                Email = cliente.Mail,
                Barrio = cliente.Barrio,
                Tipo = "CLIENTE"
            };
        }


        public static PersonaViewModel? toPersonaViewModelProveedor(this Proveedor proveedor)
        {
            if (proveedor == null)
            {
                return null;
            }

            return new PersonaViewModel
            {
                Id = proveedor.Codigo,
                RazonSocial = proveedor.Razon,
                CodigoInterno = "",
                NombreFantasia = proveedor.NombreComun,
                Ruc = proveedor.Ruc,
                Direccion = proveedor.Direccion,
                Telefono = proveedor.Telefono,
                Email = "",
                Barrio = "",
                Tipo = "PROVEEDOR"
            };
        }
    }
}