import {
  Component,
  inject,
  OnInit,
  signal,
  Input,
  Output,
  EventEmitter,
} from '@angular/core';
import { CurrencyPipe, DatePipe, DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { finalize, forkJoin, Observable, of, switchMap } from 'rxjs';
import {
  CreateLaytimeRecordDto,
  LaytimeOperationType,
  LaytimeRecord,
} from '../../../shared/types/laytime/laytime-record.type';
import { RentalContractService } from '../../../core/services/rental-contract.service';
import { LaytimeRecordService } from '../../../core/services/laytime.service';
import { RentalContract, RentalContractStatus } from '../../../shared/types/rental-contract/rentral-contract.type';

interface LaytimeRowForm {
  operationType: LaytimeOperationType;
  laytimeHours: number | null;
  startTime: string;
  endTime: string;
  notes: string;
}

interface LaytimeRowPreview {
  actualHours: number;
  overtimeHours: number;
  savedHours: number;
  demurrageAmount: number;
  despatchAmount: number;
  netAmount: number;
}

@Component({
  selector: 'app-contract-detail',
  standalone: true,
  imports: [CurrencyPipe, DatePipe, DecimalPipe, FormsModule],
  templateUrl: './contract-detail.component.html',
})
export class ContractDetailComponent implements OnInit {
  private readonly contractService = inject(RentalContractService);
  private readonly laytimeService = inject(LaytimeRecordService);
  private readonly route = inject(ActivatedRoute);

  // Input properties
  @Input() contractIdInput: string | null = null;
  @Input() contract: RentalContract | null = null;
  @Input() loading = false;
  @Input() error: string | null = null;
  @Input() chartererName = '';

  // Output events
  @Output() retry = new EventEmitter<void>();
  @Output() completed = new EventEmitter<void>();

  readonly OperationType = LaytimeOperationType;
  readonly showFullContract = signal(false);

  readonly laytimeRecords = signal<LaytimeRecord[]>([]);
  readonly laytimeLoading = signal(false);
  readonly laytimeError = signal<string | null>(null);

  readonly showFinishModal = signal(false);
  readonly finishSubmitting = signal(false);
  readonly finishError = signal<string | null>(null);
  readonly laytimeRows = signal<LaytimeRowForm[]>([this.emptyRow()]);

  // Harus sama dengan LaytimeRecordService di backend
  private readonly gracePeriodMinutes = 30;
  private readonly maxOvertimeHoursCap = 720;

  // Mendapatkan contractId dari Input atau dari Route parameter jika diakses via URL
  get effectiveContractId(): string {
    return (
      this.contractIdInput ||
      this.contract?.id ||
      this.route.snapshot.paramMap.get('id') ||
      ''
    );
  }

  ngOnInit(): void {
    // Jika komponen dipakai via Router (bukan dipassing contract dari parent), muat data kontrak
    if (!this.contract && this.effectiveContractId) {
      this.loadContract();
    }
  }

  onRetry(): void {
    if (this.retry.observed) {
      this.retry.emit();
    } else {
      this.loadContract();
    }
  }

  private loadContract(): void {
    if (!this.effectiveContractId) return;

    this.loading = true;
    this.error = null;

    this.contractService.getById(this.effectiveContractId).subscribe({
      next: (res: any) => {
        if (res?.success && res.data) {
          this.contract = res.data;
          this.chartererName = res.data.chartererName ?? '';
        } else {
          this.error = res?.message || 'Rental contract not found.';
        }
        this.loading = false;
      },
      error: (err) => {
        this.error = err?.error?.message ?? 'Gagal memuat dokumen kontrak.';
        this.loading = false;
      },
    });
  }

  toggleFullContract(): void {
    const next = !this.showFullContract();
    this.showFullContract.set(next);

    if (next && this.laytimeRecords().length === 0) {
      this.loadLaytimeRecords();
    }
  }

  private loadLaytimeRecords(): void {
    if (!this.effectiveContractId) return;

    this.laytimeLoading.set(true);
    this.laytimeError.set(null);

    this.laytimeService.getByContractId(this.effectiveContractId).subscribe({
      next: (records) => {
        this.laytimeRecords.set(records ?? []);
        this.laytimeLoading.set(false);
      },
      error: (err) => {
        if (err?.status === 404) {
          this.laytimeRecords.set([]);
        } else {
          this.laytimeError.set(
            err?.error?.message ?? 'Gagal memuat rincian laytime.',
          );
        }
        this.laytimeLoading.set(false);
      },
    });
  }

  canFinishContract(): boolean {
    return this.contract?.status === RentalContractStatus.Active;
  }

  openFinishModal(): void {
    this.finishError.set(null);
    this.laytimeRows.set([this.emptyRow()]);
    this.showFinishModal.set(true);
  }

  closeFinishModal(): void {
    if (this.finishSubmitting()) return;
    this.showFinishModal.set(false);
  }

  private emptyRow(): LaytimeRowForm {
    return {
      operationType: LaytimeOperationType.Loading,
      laytimeHours: null,
      startTime: '',
      endTime: '',
      notes: '',
    };
  }

  addLaytimeRow(): void {
    this.laytimeRows.update((rows) => [...rows, this.emptyRow()]);
  }

  removeLaytimeRow(index: number): void {
    this.laytimeRows.update((rows) => rows.filter((_, i) => i !== index));
  }

  rowPreview(row: LaytimeRowForm): LaytimeRowPreview | null {
    if (!this.contract) return null;
    if (!row.startTime || !row.endTime) return null;
    if (!row.laytimeHours || row.laytimeHours <= 0) return null;

    const start = new Date(row.startTime);
    const end = new Date(row.endTime);

    if (
      Number.isNaN(start.getTime()) ||
      Number.isNaN(end.getTime()) ||
      end <= start
    ) {
      return null;
    }

    const durationMinutes = (end.getTime() - start.getTime()) / 60000;
    const wholeHours = Math.floor(durationMinutes / 60);
    const remainderMinutes = durationMinutes - wholeHours * 60;

    const actualHours =
      remainderMinutes >= this.gracePeriodMinutes ? wholeHours + 1 : wholeHours;

    const rawOvertimeHours = Math.max(actualHours - row.laytimeHours, 0);
    const savedHours = Math.max(row.laytimeHours - actualHours, 0);
    const overtimeHours = Math.min(rawOvertimeHours, this.maxOvertimeHoursCap);

    const demurrageAmount = overtimeHours * this.contract.demurrageRate;
    const despatchAmount = savedHours * this.contract.despatchRate;
    const netAmount = demurrageAmount - despatchAmount;

    return {
      actualHours,
      overtimeHours,
      savedHours,
      demurrageAmount,
      despatchAmount,
      netAmount,
    };
  }

  submitFinishContract(): void {
    this.finishError.set(null);

    const rows = this.laytimeRows();

    for (const row of rows) {
      if (!row.startTime || !row.endTime) {
        this.finishError.set(
          'Waktu mulai dan selesai wajib diisi di semua baris.',
        );
        return;
      }
      if (new Date(row.endTime) <= new Date(row.startTime)) {
        this.finishError.set('Waktu selesai harus setelah waktu mulai.');
        return;
      }
      if (!row.laytimeHours || row.laytimeHours <= 0) {
        this.finishError.set(
          'Jam laytime harus lebih besar dari 0 di semua baris.',
        );
        return;
      }
    }

    const dtos: CreateLaytimeRecordDto[] = rows.map((row) => ({
      contractId: this.effectiveContractId,
      operationType: row.operationType,
      startTime: new Date(row.startTime).toISOString(),
      endTime: new Date(row.endTime).toISOString(),
      laytimeHours: row.laytimeHours!,
      notes: row.notes || undefined,
    }));

    this.finishSubmitting.set(true);

    const createCalls = dtos.map((dto) => this.laytimeService.create(dto));
    const createLaytime$: Observable<unknown> =
      dtos.length > 0 ? forkJoin(createCalls) : of(null);

    createLaytime$
      .pipe(
        switchMap(() => this.contractService.complete(this.effectiveContractId)),
        finalize(() => this.finishSubmitting.set(false)),
      )
      .subscribe({
        next: () => {
          this.showFinishModal.set(false);
          this.laytimeRecords.set([]);
          this.loadContract();
          this.loadLaytimeRecords();
          this.completed.emit();
        },
        error: (err) => {
          this.finishError.set(
            err?.error?.message ?? 'Gagal menyelesaikan kontrak.',
          );
        },
      });
  }
}
