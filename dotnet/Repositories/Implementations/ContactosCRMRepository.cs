using Api.Data;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class ContactosCRMRepository(ApplicationDbContext context) : IContactosCRMRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<ContactoCRM> CrearContacto(ContactoCRM contacto)
    {
        var contactoCreado = await _context.ContactosCRM.AddAsync(contacto);
        await _context.SaveChangesAsync();
        return contactoCreado.Entity;
    }

    public async Task<ContactoCRM> ActualizarContacto(ContactoCRM contacto)
    {
        var contactoActualizado = _context.ContactosCRM.Update(contacto);
        await _context.SaveChangesAsync();
        return contactoActualizado.Entity;
    }

    public async Task<ContactoCRM?> GetContactoById(uint id)
    {
        return await _context.ContactosCRM.FindAsync(id);
    }

    public async Task<IEnumerable<ContactoCRM>> GetContactos()
    {
        return await _context.ContactosCRM.ToListAsync();
    }

    // Método optimizado para obtener contacto con datos relacionados
    public async Task<ContactoViewModel?> GetContactoCompletoById(uint id)
    {
        var query = from contacto in _context.ContactosCRM
                    join departamento in _context.Departamentos on contacto.Departamento equals departamento.Id
                    join ciudad in _context.Ciudades on contacto.Ciudad equals ciudad.Id
                    join zona in _context.Zonas on contacto.Zona equals zona.Codigo
                    join operador in _context.Operadores on (int)contacto.Operador equals operador.OpCodigo
                    where contacto.Codigo == id
                    select new ContactoViewModel
                    {
                        Codigo = contacto.Codigo,
                        Nombre = contacto.Nombre,
                        EMail = contacto.EMail,
                        Telefono = contacto.Telefono,
                        Ruc = contacto.Ruc,
                        Notas = contacto.Notas,
                        Empresa = contacto.Empresa,
                        Cargo = contacto.Cargo,
                        FechaContacto = contacto.FechaContacto,
                        EsCliente = contacto.EsCliente,
                        Departamento = contacto.Departamento,
                        Ciudad = contacto.Ciudad,
                        Zona = contacto.Zona,
                        Direccion = contacto.Direccion,
                        Estado = contacto.Estado,
                        DepartamentoDescripcion = departamento.Descripcion,
                        CiudadDescripcion = ciudad.Descripcion,
                        ZonaDescripcion = zona.Descripcion,
                        Operador = contacto.Operador,
                        General = contacto.General,
                        OperadorNombre = operador.OpNombre
                    };

        return await query.FirstOrDefaultAsync();
    }

    // Método optimizado para obtener todos los contactos con datos relacionados
    public async Task<IEnumerable<ContactoViewModel>> GetContactosCompletos()
    {
        var query = from contacto in _context.ContactosCRM
                    join departamento in _context.Departamentos on contacto.Departamento equals departamento.Id
                    join ciudad in _context.Ciudades on contacto.Ciudad equals ciudad.Id
                    join zona in _context.Zonas on contacto.Zona equals zona.Codigo
                    join operador in _context.Operadores on (int)contacto.Operador equals operador.OpCodigo
                    select new ContactoViewModel
                    {
                        Codigo = contacto.Codigo,
                        Nombre = contacto.Nombre,
                        EMail = contacto.EMail,
                        Telefono = contacto.Telefono,
                        Ruc = contacto.Ruc,
                        Notas = contacto.Notas,
                        Empresa = contacto.Empresa,
                        Cargo = contacto.Cargo,
                        FechaContacto = contacto.FechaContacto,
                        EsCliente = contacto.EsCliente,
                        Departamento = contacto.Departamento,
                        Ciudad = contacto.Ciudad,
                        Zona = contacto.Zona,
                        Direccion = contacto.Direccion,
                        Estado = contacto.Estado,
                        DepartamentoDescripcion = departamento.Descripcion,
                        CiudadDescripcion = ciudad.Descripcion,
                        ZonaDescripcion = zona.Descripcion,
                        Operador = contacto.Operador,
                        General = contacto.General,
                        OperadorNombre = operador.OpNombre
                    };

        return await query.ToListAsync();
    }
}