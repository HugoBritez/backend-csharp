using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Api.Models.ViewModels;
namespace Api.Services.Implementations
{
    public class PresupuestoService : IPresupuestoService
    {
        private readonly IPresupuestosRepository _presupuestoRepository;
        private readonly IDetallePresupuestoRepository _detallePresupuestoRepository;
        private readonly IPresupuestoObservacionRepository _presupuestoObservacionRepository;

        public PresupuestoService(
            IPresupuestosRepository presu,
            IDetallePresupuestoRepository detalle,
            IPresupuestoObservacionRepository presuObs
            )
        {
            _presupuestoRepository = presu;
            _detallePresupuestoRepository = detalle;
            _presupuestoObservacionRepository = presuObs;
        }

        public async Task<ResponseViewModel<Presupuesto>> CrearPresupuesto(Presupuesto presupuesto, PresupuestoObservacion observacion, IEnumerable<DetallePresupuesto> detalle)
        {
            if (presupuesto.Codigo != 0)
            {
                var presupuestoActualizado = await _presupuestoRepository.Update(presupuesto);
                //eliminamos los detalles viejos del presu
                await _detallePresupuestoRepository.Delete(presupuesto.Codigo);
                //e insertamos los nuevos que vienen con el endpoint
                foreach (DetallePresupuesto det in detalle)
                {
                    det.Presupuesto = presupuestoActualizado.Codigo;
                    await _detallePresupuestoRepository.Crear(det);
                }

                //verificamos si existe obervacion y actualizamos si es el caso
                var presupuestoObservacion = await _presupuestoObservacionRepository.GetById(presupuesto.Codigo);
                if (presupuestoObservacion != null)
                {
                    await _presupuestoObservacionRepository.Update(observacion);
                }
                else
                {
                    await _presupuestoObservacionRepository.Crear(observacion);
                }
                return new ResponseViewModel<Presupuesto>
                {
                    Success = true,
                    Message = "Presupuesto encontrado y actualizado",
                    Data = presupuestoActualizado
                };
            }
            else
            {
                var presupuestoNuevoCreado = await _presupuestoRepository.Crear(presupuesto);
                var presupuestoObservacionCreado = await _presupuestoObservacionRepository.Crear(observacion);
                foreach (DetallePresupuesto det in detalle)
                {
                    det.Presupuesto = presupuestoNuevoCreado.Codigo;
                    await _detallePresupuestoRepository.Crear(det);
                }
                return new ResponseViewModel<Presupuesto>
                {
                    Success = true,
                    Message = "Presupuesto creado con exito",
                    Data = presupuestoNuevoCreado

                };
            }
        }
        public async Task<RecuperarPresupuestoViewModel> RecuperarPresupuesto(uint idPresupuesto)
        {
            var presupuesto = await _presupuestoRepository.GetById(idPresupuesto);
            var detalles = await _detallePresupuestoRepository.GetByPresupuesto(idPresupuesto);


            return new RecuperarPresupuestoViewModel
            {
                Presupuesto = presupuesto,
                Detalles = detalles
            };
        }
    }
}