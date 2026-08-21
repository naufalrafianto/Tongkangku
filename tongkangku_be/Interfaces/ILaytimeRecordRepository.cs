using tongkangku_be.Models;
using tongkangku_be.Repositories;

namespace tongkangku_be.Interfaces
{
    public interface ILaytimeRecordRepository
      : IRepository<LaytimeRecord>
    {
        Task<List<LaytimeRecord>> GetByContractIdAsync(
            Guid contractId,
            params string[] includes
        );
    }
}
