using tongkangku_be.Models.Enums;

namespace tongkangku_be.Dtos.LaytimeRecord
{
    public class CreateLaytimeRecordDto
    {
        public Guid ContractId { get; set; }
        public OperationType OperationType { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public decimal LaytimeHours { get; set; }

        public string? Notes { get; set; }
    }
}
