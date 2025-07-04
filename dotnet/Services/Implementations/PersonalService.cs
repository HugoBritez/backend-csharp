using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Api.Services.Mappers;

namespace Api.Services.Implementations
{
    public class PersonalService : IPersonalService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IProveedoresRepository _proveedoresRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly IListaPrecioRepository _listaPrecioRepository;
        private readonly IConfiguracionRepository _configuracionRepository;


        public PersonalService(
            IClienteRepository clienteRepository,
            IProveedoresRepository proveedoresRepository,
            IUsuarioRepository usuarioRepository,
            IPersonaRepository personaRepository,
            IListaPrecioRepository listaPrecioRepository,
            IConfiguracionRepository configuracionRepository
        )
        {
            _clienteRepository = clienteRepository;
            _proveedoresRepository = proveedoresRepository;
            _usuarioRepository = usuarioRepository;
            _personaRepository = personaRepository;
            _listaPrecioRepository = listaPrecioRepository;
            _configuracionRepository = configuracionRepository;
        }
        public async Task<bool?> CrearPersona(CrearPersonaDTO data)
        {
            try
            {
                if (data == null)
                    throw new ArgumentNullException(nameof(data), "No se recibieron datos para crear la persona.");

                if (data.persona == null)
                    throw new ArgumentNullException(nameof(data.persona), "No se proporcionó información de la persona.");

                if (!data.Tipo.Any())
                    throw new ArgumentException("Debe especificar al menos un tipo de persona.");

                // Validar que los tipos seleccionados tengan su correspondiente data
                foreach (int tipo in data.Tipo.Distinct())
                {
                    switch (tipo)
                    {
                        case 0 when data.Cliente == null:
                            throw new ArgumentException("Se seleccionó tipo Cliente pero no se proporcionaron los datos.");
                        case 1 when data.Proveedor == null:
                            throw new ArgumentException("Se seleccionó tipo Proveedor pero no se proporcionaron los datos.");
                        case 2 when data.Operador == null:
                            throw new ArgumentException("Se seleccionó tipo Operador pero no se proporcionaron los datos.");
                    }
                }

                // Verificar si la persona ya existe
                var personaExistente = await _personaRepository.GetByRuc(data.persona.Ruc);
                if (personaExistente != null)
                {
                    // Actualizar datos de la persona existente
                    personaExistente.RazonSocial = data.persona.RazonSocial;
                    personaExistente.NombreFantasia = data.persona.NombreFantasia;
                    personaExistente.CodigoInterno = data.persona.CodigoInterno?.ToString() ?? "";
                    personaExistente.Ci = data.persona.Ci;
                    personaExistente.TipoDocumento = data.persona.TipoDocumento;
                    personaExistente.Departamento = data.persona.Departamento;
                    personaExistente.Ciudad = data.persona.Ciudad;
                    personaExistente.Direccion = data.persona.Direccion;
                    personaExistente.Barrio = data.persona.Barrio;
                    personaExistente.Zona = data.persona.Zona;
                    personaExistente.Moneda = data.persona.Moneda;
                    personaExistente.Email = data.persona.Email;
                    personaExistente.Telefono = data.persona.Telefono;
                    personaExistente.FechaModificacion = DateTime.Now;

                    await _personaRepository.UpdatePersona(personaExistente);
                }
                else
                {
                    data.persona.Codigo = 0;
                    data.persona.FechaCreacion = DateTime.Now;
                    data.persona.FechaModificacion = DateTime.Now;
                    data.persona.CodigoInterno = data.persona.CodigoInterno?.ToString() ?? "";
                    // Crear nueva persona
                    await _personaRepository.CreateAsync(data.persona);
                }

                // Procesar SOLO los tipos seleccionados
                foreach (int tipo in data.Tipo.Distinct())
                {
                    switch (tipo)
                    {
                        case 0: // Cliente
                            var clienteExistente = await _clienteRepository.GetByRuc(data.persona.Ruc);
                            if (clienteExistente != null && data.Cliente != null)
                            {
                                // Actualizar cliente existente
                                clienteExistente.Razon = data.Cliente.Razon;
                                clienteExistente.Descripcion = data.Cliente.Descripcion;
                                clienteExistente.Interno = data.Cliente.Interno?.ToString() ?? "";
                                clienteExistente.Ruc = data.Cliente.Ruc;
                                clienteExistente.Ci = data.Cliente.Ci;
                                clienteExistente.Ciudad = data.Cliente.Ciudad;
                                clienteExistente.Moneda = data.Cliente.Moneda;
                                clienteExistente.Barrio = data.Cliente.Barrio;
                                clienteExistente.Dir = data.Cliente.Dir;
                                clienteExistente.Tel = data.Cliente.Tel;
                                clienteExistente.Credito = data.Cliente.Credito;
                                clienteExistente.LimiteCredito = data.Cliente.LimiteCredito;
                                clienteExistente.Vendedor = data.Cliente.Vendedor;
                                clienteExistente.Cobrador = data.Cliente.Cobrador;
                                clienteExistente.Referencias = data.Cliente.Referencias;
                                clienteExistente.Estado = data.Cliente.Estado;
                                clienteExistente.FechaAd = data.Cliente.FechaAd;
                                clienteExistente.Condicion = data.Cliente.Condicion;
                                clienteExistente.Tipo = data.Cliente.Tipo;
                                clienteExistente.Grupo = data.Cliente.Grupo;
                                clienteExistente.Plazo = data.Cliente.Plazo;
                                clienteExistente.Zona = data.Cliente.Zona;
                                clienteExistente.Llamada = data.Cliente.Llamada;
                                clienteExistente.ProxLlamada = data.Cliente.ProxLlamada;
                                clienteExistente.Respuesta = data.Cliente.Respuesta;
                                clienteExistente.FecNac = data.Cliente.FecNac;
                                clienteExistente.Exentas = data.Cliente.Exentas;
                                clienteExistente.Mail = data.Cliente.Mail;
                                clienteExistente.Agente = data.Cliente.Agente;
                                clienteExistente.Contrato = data.Cliente.Contrato;
                                clienteExistente.NombreCod = data.Cliente.NombreCod;
                                clienteExistente.DocCod = data.Cliente.DocCod;
                                clienteExistente.ObsDeuda = data.Cliente.ObsDeuda;
                                clienteExistente.Moroso = data.Cliente.Moroso;
                                clienteExistente.AgenteRetentor = data.Cliente.AgenteRetentor;
                                clienteExistente.Consultar = data.Cliente.Consultar;
                                clienteExistente.Plan = data.Cliente.Plan;
                                clienteExistente.FechaPago = data.Cliente.FechaPago;
                                clienteExistente.Departamento = data.Cliente.Departamento;
                                clienteExistente.Gerente = data.Cliente.Gerente;
                                clienteExistente.GerTelefono = data.Cliente.GerTelefono;
                                clienteExistente.GerTelefono2 = data.Cliente.GerTelefono2;
                                clienteExistente.GerPagina = data.Cliente.GerPagina;
                                clienteExistente.GerMail = data.Cliente.GerMail;
                                clienteExistente.PermitirDesc = data.Cliente.PermitirDesc;
                                clienteExistente.CalcMora = data.Cliente.CalcMora;
                                clienteExistente.BloquearVendedor = data.Cliente.BloquearVendedor;
                                clienteExistente.Sexo = data.Cliente.Sexo;
                                clienteExistente.TipoDoc = data.Cliente.TipoDoc;
                                clienteExistente.RepetirRuc = data.Cliente.RepetirRuc;
                                clienteExistente.Acuerdo = data.Cliente.Acuerdo;
                                clienteExistente.DirCod = data.Cliente.DirCod;
                                clienteExistente.TelefCod = data.Cliente.TelefCod;

                                await _clienteRepository.UpdateCliente(clienteExistente);
                            }
                            else
                            {
                                data.Cliente!.Interno = data.Cliente.Interno?.ToString() ?? "";
                                // Crear nuevo cliente
                                var clienteCreado = await _clienteRepository.CrearCliente(data.Cliente!);
                                if (clienteCreado?.Codigo == null)
                                    throw new InvalidOperationException("No se pudo obtener el ID del cliente creado.");

                                // Procesar precios solo si es cliente
                                foreach (uint precio in data.Precios.Select(v => (uint)v))
                                {
                                    var listaPrecios = new ClientesListaDePrecio
                                    {
                                        ClienteId = clienteCreado.Codigo,
                                        ListaDePrecioId = precio
                                    };

                                    await _listaPrecioRepository.InsertarPorCliente(listaPrecios);
                                }
                            }
                            break;
                        case 1: // Proveedor - NO procesar si no está seleccionado
                            if (data.Proveedor != null)
                            {
                                var proveedorExistente = await _proveedoresRepository.GetByRuc(data.persona.Ruc);
                                if (proveedorExistente != null)
                                {
                                    // Actualizar proveedor existente
                                    proveedorExistente.Razon = data.Proveedor.Razon;
                                    proveedorExistente.NombreComun = data.Proveedor.NombreComun;
                                    proveedorExistente.Direccion = data.Proveedor.Direccion;
                                    proveedorExistente.Telefono = data.Proveedor.Telefono;
                                    proveedorExistente.Mail = data.Proveedor.Mail;
                                    proveedorExistente.Moneda = data.Proveedor.Moneda;
                                    proveedorExistente.Zona = data.Proveedor.Zona;
                                    proveedorExistente.Observacion = data.Proveedor.Observacion;
                                    proveedorExistente.PaisExtranjero = data.Proveedor.PaisExtranjero;
                                    proveedorExistente.Plazo = data.Proveedor.Plazo;
                                    proveedorExistente.Credito = data.Proveedor.Credito;
                                    proveedorExistente.TipoNac = data.Proveedor.TipoNac;
                                    proveedorExistente.Supervisor = data.Proveedor.Supervisor;
                                    proveedorExistente.TelefonoSupervisor = data.Proveedor.TelefonoSupervisor;
                                    proveedorExistente.Vendedor = data.Proveedor.Vendedor;
                                    proveedorExistente.TelefonoVendedor = data.Proveedor.TelefonoVendedor;
                                    proveedorExistente.AplicarGasto = data.Proveedor.AplicarGasto;
                                    proveedorExistente.Plan = data.Proveedor.Plan;
                                    proveedorExistente.TipoDoc = data.Proveedor.TipoDoc;
                                    proveedorExistente.Estado = data.Proveedor.Estado;

                                    await _proveedoresRepository.UpdateProveedor(proveedorExistente);
                                }
                                else
                                {
                                    // Crear nuevo proveedor
                                    await _proveedoresRepository.CrearProveedor(data.Proveedor);
                                }
                            }
                            break;
                        case 2: // Operador - NO procesar si no está seleccionado
                            if (data.Operador != null)
                            {
                                await _usuarioRepository.CrearOperador(data.Operador);
                            }
                            break;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear/actualizar persona: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<PersonaViewModel>> GetPersonas(string? Busqueda, int Tipo)
        {
            var personas = new List<PersonaViewModel>();
            var personasPorRuc = new Dictionary<string, PersonaViewModel>();

            if (Tipo == 0) // Ambos: clientes y proveedores
            {
                var proveedores = await _proveedoresRepository.GetAll(Busqueda);
                var clientes = await _clienteRepository.GetAll(Busqueda);

                // Procesar proveedores primero
                foreach (Proveedor proveedor in proveedores)
                {
                    var proveedorViewModel = proveedor.toPersonaViewModelProveedor();
                    if (proveedorViewModel != null)
                    {
                        proveedorViewModel.Tipo = "Proveedor";

                        // Solo usar RUC para agrupación si es un RUC válido/real
                        if (!string.IsNullOrEmpty(proveedorViewModel.Ruc) && IsValidRuc(proveedorViewModel.Ruc))
                        {
                            personasPorRuc[proveedorViewModel.Ruc] = proveedorViewModel;
                        }
                        else
                        {
                            // RUC no válido, agregar directamente sin agrupar
                            personas.Add(proveedorViewModel);
                        }
                    }
                }

                // Procesar clientes y verificar duplicados
                foreach (Cliente cliente in clientes)
                {
                    var clienteViewModel = cliente.toPersonaViewModel();
                    if (clienteViewModel != null)
                    {
                        if (!string.IsNullOrEmpty(clienteViewModel.Ruc) && IsValidRuc(clienteViewModel.Ruc))
                        {
                            if (personasPorRuc.ContainsKey(clienteViewModel.Ruc))
                            {
                                // Ya existe como proveedor con RUC válido, actualizar tipo
                                personasPorRuc[clienteViewModel.Ruc].Tipo = "Cliente/Proveedor";
                            }
                            else
                            {
                                // RUC válido pero no existe como proveedor
                                clienteViewModel.Tipo = "Cliente";
                                personasPorRuc[clienteViewModel.Ruc] = clienteViewModel;
                            }
                        }
                        else
                        {
                            // RUC no válido, agregar directamente sin agrupar
                            clienteViewModel.Tipo = "Cliente";
                            personas.Add(clienteViewModel);
                        }
                    }
                }

                // Agregar las personas agrupadas por RUC válido
                personas.AddRange(personasPorRuc.Values);
            }
            else if (Tipo == 1) // Solo proveedores
            {
                var proveedores = await _proveedoresRepository.GetAll(Busqueda);

                foreach (Proveedor proveedor in proveedores)
                {
                    var proveedorViewModel = proveedor.toPersonaViewModelProveedor();
                    if (proveedorViewModel != null)
                    {
                        personas.Add(proveedorViewModel);
                    }
                }
            }
            else // Solo clientes (Tipo == 2 o cualquier otro valor)
            {
                var clientes = await _clienteRepository.GetAll(Busqueda);

                foreach (Cliente cliente in clientes)
                {
                    var clienteViewModel = cliente.toPersonaViewModel();
                    if (clienteViewModel != null)
                    {
                        personas.Add(clienteViewModel);
                    }
                }
            }

            return personas;
        }


        public async Task<CrearPersonaDTO?> GetPersonaByRuc(uint id, int Tipo)
        {
            // Primero buscamos según el tipo especificado
            Cliente? cliente = null;
            Proveedor? proveedor = null;
            Persona? persona = null;
            var tipos = new List<int>();

            switch (Tipo)
            {
                case 0: // Cliente
                    cliente = await _clienteRepository.GetById(id);
                    if (cliente == null) return null;

                    // Si el RUC es inválido, mapeamos directamente a persona
                    if (string.IsNullOrEmpty(cliente.Ruc) || !IsValidRuc(cliente.Ruc))
                    {
                        persona = MapClienteToPersona(cliente);
                        tipos.Add(0);
                    }
                    else
                    {
                        // Solo buscamos persona si el RUC es válido
                        persona = await _personaRepository.GetByRuc(cliente.Ruc);
                        persona ??= MapClienteToPersona(cliente);
                        tipos.Add(0);

                        // Verificamos si también es proveedor
                        var proveedorExistente = await _proveedoresRepository.GetByRuc(cliente.Ruc);
                        if (proveedorExistente != null)
                        {
                            proveedor = proveedorExistente;
                            tipos.Add(1);
                        }
                    }
                    break;

                case 1: // Proveedor
                    proveedor = await _proveedoresRepository.GetById(id);
                    if (proveedor == null) return null;

                    // Si el RUC es inválido, mapeamos directamente a persona
                    if (string.IsNullOrEmpty(proveedor.Ruc) || !IsValidRuc(proveedor.Ruc))
                    {
                        persona = MapProveedorToPersona(proveedor);
                        tipos.Add(1);
                    }
                    else
                    {
                        // Solo buscamos persona si el RUC es válido
                        persona = await _personaRepository.GetByRuc(proveedor.Ruc);
                        persona ??= MapProveedorToPersona(proveedor);
                        tipos.Add(1);

                        // Verificamos si también es cliente
                        var clienteExistente = await _clienteRepository.GetByRuc(proveedor.Ruc);
                        if (clienteExistente != null)
                        {
                            cliente = clienteExistente;
                            tipos.Add(0);
                        }
                    }
                    break;

                default:
                    return null; // Tipo no válido
            }

            var result = new CrearPersonaDTO
            {
                persona = persona,
                Tipo = [.. tipos] // Incluimos todos los tipos encontrados
            };

            // Agregamos las entidades correspondientes según los tipos encontrados
            if (cliente != null)
            {

                result.Cliente = cliente;
                var precios = await _listaPrecioRepository.GetByCliente(cliente.Codigo);
                result.Precios = [.. precios.Select(p => (int)p.LpCodigo)];
            }

            if (proveedor != null)
            {
                result.Proveedor = proveedor;
            }

            return result;
        }

        private static Persona MapClienteToPersona(Cliente cliente)
        {
            return new Persona
            {
                Codigo = cliente.Codigo,
                CodigoInterno = cliente.Interno?.ToString() ?? "",
                RazonSocial = cliente.Razon ?? "",
                NombreFantasia = cliente.Descripcion ?? "",
                Ruc = cliente.Ruc ?? "",
                Ci = cliente.Ci ?? "",
                TipoDocumento = cliente.TipoDoc,
                Departamento = cliente.Departamento,
                Ciudad = cliente.Ciudad.HasValue ? (uint)cliente.Ciudad : 0,
                Direccion = cliente.Dir ?? "",
                Barrio = cliente.Barrio ?? "",
                Zona = cliente.Zona > 0 ? cliente.Zona : 0,
                Moneda = cliente.Moneda ?? 0,
                Email = cliente.Mail ?? "",
                Telefono = cliente.Tel ?? "",
                Estado = 1
            };
        }

        private static Persona MapProveedorToPersona(Proveedor proveedor)
        {
            return new Persona
            {
                Codigo = proveedor.Codigo,
                CodigoInterno = "",
                RazonSocial = proveedor.Razon ?? "",
                NombreFantasia = proveedor.NombreComun ?? "",
                Ruc = proveedor.Ruc ?? "",
                Ci = proveedor.Ruc ?? "",
                TipoDocumento = proveedor.TipoDoc,
                Departamento = 1,
                Ciudad = 1,
                Direccion = proveedor.Direccion ?? "",
                Barrio = "",
                Zona = 1,
                Moneda = proveedor.Moneda > 0 ? proveedor.Moneda : 0,
                Email = proveedor.Mail ?? "",
                Telefono = proveedor.Telefono ?? "",
                Estado = 1
            };
        }

        // Método auxiliar para determinar si un RUC es válido para agrupación
        private bool IsValidRuc(string ruc)
        {
            // Esta función asume que ruc no es null (se valida antes de llamarla)
            var rucTrimmed = ruc.Trim();

            // Verificar formato: xxxxx-x (mínimo 4 dígitos, guión, uno o más dígitos)
            var rucPattern = @"^\d{4,}-\d+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(rucTrimmed, rucPattern))
                return false;

            // Separar las partes del RUC
            var parts = rucTrimmed.Split('-');
            if (parts.Length != 2)
                return false;

            var firstPart = parts[0];
            var secondPart = parts[1];

            // Descartar RUCs obviamente falsos o genéricos
            var invalidFirstParts = new[] { "0000", "1111", "2222", "3333", "4444", "5555", "6666", "7777", "8888", "9999", "0001", "0002", "0003", "1234", "12345", "xxxx" };

            if (invalidFirstParts.Contains(firstPart))
                return false;

            // Verificar que no sea solo el mismo dígito repetido en la primera parte
            if (firstPart.All(c => c == firstPart[0]))
                return false;

            return true;
        }

        public async Task<ClienteViewModel?> GetClientePorDefecto()
        {
            var config = await _configuracionRepository.GetById(63);
            if (config == null)
                return null;

            return await _clienteRepository.GetClientePorId(uint.Parse(config.Valor));
        }
    }
}