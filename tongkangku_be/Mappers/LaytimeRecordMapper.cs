using tongkangku_be.Dtos.LaytimeRecord;
using tongkangku_be.Models;

namespace tongkangku_be.Mappers
{
    public static class LaytimeRecordMapper
    {
        public static LaytimeRecordResponseDto ToDto(LaytimeRecord record)
        {
            return new LaytimeRecordResponseDto
            {
                Id = record.Id,
                ContractId = record.ContractId,
                ContractNum = record.Contract?.ContractNum ?? string.Empty,

                OperationType = record.OperationType,

                StartTime = record.StartTime,
                EndTime = record.EndTime,

                LaytimeHours = record.LaytimeHours,
                ActualDurationHours = record.ActualDurationHours,
                OvertimeHours = record.OvertimeHours,
                SavedHours = record.SavedHours,

                DemurrageRate = record.DemurrageRate,
                DemurrageAmount = record.DemurrageAmount,

                DespatchRate = record.DespatchRate,
                DespatchAmount = record.DespatchAmount,

                NetLaytimeAmount = record.NetLaytimeAmount,

                Notes = record.Notes,

                CreatedAt = record.CreatedAt,
                UpdatedAt = record.UpdatedAt
            };
        }
    }
}
