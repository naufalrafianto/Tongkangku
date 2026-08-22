using tongkangku_be.Dtos.LaytimeRecord;

namespace tongkangku_be.Interfaces
{
    public interface ILaytimeRecordService
    {
        Task<LaytimeRecordResponseDto> GetByIdAsync(Guid id);
        Task<List<LaytimeRecordResponseDto>> GetAllAsync();
        Task<List<LaytimeRecordResponseDto>> GetByContractIdAsync(Guid contractId);

        Task<LaytimeRecordResponseDto> CreateAsync(CreateLaytimeRecordDto dto);
        Task<LaytimeRecordResponseDto> UpdateAsync(Guid id, UpdateLaytimeRecordDto dto);
        Task DeleteAsync(Guid id);
    }
}
