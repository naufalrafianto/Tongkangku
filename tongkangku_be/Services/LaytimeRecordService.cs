using tongkangku_be.Data;
using tongkangku_be.Dtos.LaytimeRecord;
using tongkangku_be.Interfaces;
using tongkangku_be.Mappers;
using tongkangku_be.Models.Enums;
using tongkangku_be.Models;
using tongkangku_be.Shared;

namespace tongkangku_be.Services
{
    public class LaytimeRecordService(
        ILaytimeRecordRepository laytimeRecordRepository,
        IRentalContractRepository rentalContractRepository,
        ApplicationDbContext context) : ILaytimeRecordService
    {
        private readonly ILaytimeRecordRepository _laytimeRecordRepository = laytimeRecordRepository;
        private readonly IRentalContractRepository _rentalContractRepository = rentalContractRepository;
        private readonly ApplicationDbContext _context = context;

        public async Task<LaytimeRecordResponseDto> GetByIdAsync(Guid id)
        {
            var record = await _laytimeRecordRepository.GetByIdAsync(id, "Contract");

            return record == null
                ? throw new NotFoundException($"Laytime record with id '{id}' was not found.")
                : LaytimeRecordMapper.ToDto(record);
        }

        public async Task<List<LaytimeRecordResponseDto>> GetAllAsync()
        {
            var records = await _laytimeRecordRepository.GetAllAsync("Contract");

            if (records.Count == 0)
            {
                throw new NotFoundException("Laytime records not found.");
            }

            return records.Select(LaytimeRecordMapper.ToDto).ToList();
        }

        public async Task<List<LaytimeRecordResponseDto>> GetByContractIdAsync(Guid contractId)
        {
            var contract = await _rentalContractRepository.GetByIdAsync(contractId);

            if (contract == null)
            {
                throw new NotFoundException($"Rental contract with id '{contractId}' was not found.");
            }

            var records = await _laytimeRecordRepository.GetByContractIdAsync(contractId, "Contract");

            return records.Select(LaytimeRecordMapper.ToDto).ToList();
        }

        public async Task<LaytimeRecordResponseDto> CreateAsync(
    CreateLaytimeRecordDto dto)
        {
            if (dto.EndTime <= dto.StartTime)
            {
                throw new ValidationException(new
                {
                    EndTime = "End time must be after start time."
                });
            }

            if (dto.LaytimeHours <= 0)
            {
                throw new ValidationException(new
                {
                    LaytimeHours = "Laytime hours must be greater than 0."
                });
            }

            var contract = await _rentalContractRepository.GetByIdAsync(
                dto.ContractId
            );

            if (contract == null)
            {
                throw new NotFoundException(
                    $"Rental contract with id '{dto.ContractId}' was not found."
                );
            }

            if (contract.Status != RentalContractStatus.Active)
            {
                throw new ValidationException(new
                {
                    ContractId = "Laytime can only be recorded for active contracts."
                });
            }

            var (
                actualHours,
                overtimeHours,
                savedHours,
                demurrageAmount,
                despatchAmount,
                netAmount
            ) = CalculateLaytime(
                dto.StartTime,
                dto.EndTime,
                dto.LaytimeHours,
                contract.DemurrageRate,
                contract.DespatchRate
            );

            return await _context.ExecuteInTransactionAsync(async () =>
            {
                var record = new LaytimeRecord
                {
                    Id = Guid.NewGuid(),

                    ContractId = dto.ContractId,
                    OperationType = dto.OperationType,

                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,

                    LaytimeHours = dto.LaytimeHours,
                    ActualDurationHours = actualHours,

                    OvertimeHours = overtimeHours,
                    SavedHours = savedHours,

                    DemurrageRate = contract.DemurrageRate,
                    DemurrageAmount = demurrageAmount,

                    DespatchRate = contract.DespatchRate,
                    DespatchAmount = despatchAmount,

                    NetLaytimeAmount = netAmount,

                    Notes = dto.Notes ?? string.Empty,

                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _laytimeRecordRepository.AddAsync(record);
                await _laytimeRecordRepository.SaveChangesAsync();

                var created = await _laytimeRecordRepository.GetByIdAsync(
                    record.Id,
                    nameof(LaytimeRecord.Contract)
                );

                if (created == null)
                {
                    throw new NotFoundException(
                        $"Laytime record with id '{record.Id}' was not found after creation."
                    );
                }

                return LaytimeRecordMapper.ToDto(created);
            });
        }

        public async Task<LaytimeRecordResponseDto> UpdateAsync(Guid id, UpdateLaytimeRecordDto dto)
        {
            if (dto.EndTime <= dto.StartTime)
            {
                throw new ValidationException(new
                {
                    EndTime = "End time must be after start time."
                });
            }

            if (dto.LaytimeHours <= 0)
            {
                throw new ValidationException(new
                {
                    LaytimeHours = "Laytime hours must be greater than 0."
                });
            }

            var record = await _laytimeRecordRepository.GetByIdAsync(id, "Contract");

            if (record == null)
            {
                throw new NotFoundException($"Laytime record with id '{id}' was not found.");
            }

            var (actualHours, overtimeHours, savedHours, demurrageAmount, despatchAmount, netAmount) =
                CalculateLaytime(dto.StartTime, dto.EndTime, dto.LaytimeHours, record.DemurrageRate, record.DespatchRate);

            await _context.ExecuteInTransactionAsync(async () =>
            {
                record.StartTime = dto.StartTime;
                record.EndTime = dto.EndTime;
                record.LaytimeHours = dto.LaytimeHours;

                record.ActualDurationHours = actualHours;
                record.OvertimeHours = overtimeHours;
                record.SavedHours = savedHours;

                record.DemurrageAmount = demurrageAmount;
                record.DespatchAmount = despatchAmount;
                record.NetLaytimeAmount = netAmount;

                record.Notes = dto.Notes ?? string.Empty;
                record.UpdatedAt = DateTime.UtcNow;

                _laytimeRecordRepository.Update(record);
                await _laytimeRecordRepository.SaveChangesAsync();
            });

            return LaytimeRecordMapper.ToDto(record);
        }

        public async Task DeleteAsync(Guid id)
        {
            var record = await _laytimeRecordRepository.GetByIdAsync(id);

            if (record == null)
            {
                throw new NotFoundException($"Laytime record with id '{id}' was not found.");
            }

            _laytimeRecordRepository.Delete(record);
            await _laytimeRecordRepository.SaveChangesAsync();
        }

        private (
    int ActualHours,
    int OvertimeHours,
    int SavedHours,
    decimal DemurrageAmount,
    decimal DespatchAmount,
    decimal NetAmount
) CalculateLaytime(
    DateTime startTime,
    DateTime endTime,
    int laytimeHours,
    decimal demurrageRate,
    decimal despatchRate)
        {
            var duration = endTime - startTime;

            var actualHours = (int)Math.Ceiling(
                duration.TotalHours
            );

            var overtimeHours = Math.Max(
                actualHours - laytimeHours,
                0
            );

            var savedHours = Math.Max(
                laytimeHours - actualHours,
                0
            );

            var demurrageAmount =
                overtimeHours * demurrageRate;

            var despatchAmount =
                savedHours * despatchRate;

            var netAmount =
                demurrageAmount - despatchAmount;

            return (
                actualHours,
                overtimeHours,
                savedHours,
                demurrageAmount,
                despatchAmount,
                netAmount
            );
        }
    }
}
