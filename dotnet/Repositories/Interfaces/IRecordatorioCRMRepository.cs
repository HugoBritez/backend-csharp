using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Repositories.Interfaces
{
    public interface IRecordatorioCRMRepository
    {
        Task<IEnumerable<RecordatorioCRMViewModel>> GetAll();
        Task<RecordatorioCRMViewModel?> GetById(uint id);
        Task<RecordatorioCRM> Create(RecordatorioCRM recordatorio);
        Task<RecordatorioCRM> Update(RecordatorioCRM recordatorio);

    }
}