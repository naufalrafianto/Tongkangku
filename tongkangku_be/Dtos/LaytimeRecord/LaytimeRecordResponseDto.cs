using tongkangku_be.Models.Enums;

namespace tongkangku_be.Dtos.LaytimeRecord
{
    public class LaytimeRecordResponseDto
    {
        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public string ContractNum { get; set; } = string.Empty;

        public OperationType OperationType { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public decimal LaytimeHours { get; set; }
        public decimal ActualDurationHours { get; set; }
        public decimal OvertimeHours { get; set; }
        public decimal SavedHours { get; set; }

        public decimal DemurrageRate { get; set; }
        public decimal DemurrageAmount { get; set; }

        public decimal DespatchRate { get; set; }
        public decimal DespatchAmount { get; set; }

        public decimal NetLaytimeAmount { get; set; }

        public string Notes { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
