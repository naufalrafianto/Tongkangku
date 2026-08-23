import { Component, EventEmitter, inject, Input, OnChanges, OnInit, Output, signal, SimpleChanges } from '@angular/core';
import { CurrencyPipe, DatePipe, DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';
import { LaytimeRecordService } from '../../core/services/laytime.service';
import { RentalContractService } from '../../core/services/rental-contract.service';
import {
  CreateLaytimeRecordDto,
  LaytimeOperationType,
  LaytimeRecord,
} from '../../shared/types/laytime/laytime-record.type';

@Component({
  selector: 'app-laytime-section',
  standalone: true,
  imports: [CurrencyPipe, DatePipe, FormsModule],
  templateUrl: './laytime-record.component.html',
})
export class LaytimeSectionComponent implements OnInit, OnChanges {
  private laytimeService = inject(LaytimeRecordService);
  private contractService = inject(RentalContractService);

  @Input({ required: true }) contractId!: string;

  @Input() isActive = false;

  @Output() completed = new EventEmitter<void>();

  readonly LaytimeOperationType = LaytimeOperationType;

  records = signal<LaytimeRecord[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);

  showForm = signal(false);
  saving = signal(false);
  formError = signal<string | null>(null);

  deletingId = signal<string | null>(null);

  completing = signal(false);
  completeError = signal<string | null>(null);

  form: {
    operationType: LaytimeOperationType;
    startTime: string;
    endTime: string;
    laytimeHours: number | null;
    notes: string;
  } = this.emptyForm();

  ngOnInit(): void {
    this.loadRecords();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['contractId'] && !changes['contractId'].firstChange) {
      this.loadRecords();
    }
  }

  private emptyForm() {
    return {
      operationType: LaytimeOperationType.Loading,
      startTime: '',
      endTime: '',
      laytimeHours: null,
      notes: '',
    };
  }

  loadRecords(): void {
    this.loading.set(true);
    this.error.set(null);

    this.laytimeService
      .getByContractId(this.contractId)
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (res) => this.records.set(res ?? []),
        error: (err) => {
          // 404 dari NotFoundException backend saat belum ada record sama sekali
          if (err?.status === 404) {
            this.records.set([]);
            return;
          }
          this.error.set(
            err?.error?.message ?? 'Gagal memuat data laytime.',
          );
        },
      });
  }

  toggleForm(): void {
    this.showForm.update((v) => !v);
    this.formError.set(null);
    if (!this.showForm()) {
      this.form = this.emptyForm();
    }
  }

  submitForm(): void {
    this.formError.set(null);

    if (!this.form.startTime || !this.form.endTime) {
      this.formError.set('Waktu mulai dan selesai wajib diisi.');
      return;
    }

    if (new Date(this.form.endTime) <= new Date(this.form.startTime)) {
      this.formError.set('Waktu selesai harus setelah waktu mulai.');
      return;
    }

    if (!this.form.laytimeHours || this.form.laytimeHours <= 0) {
      this.formError.set('Jam laytime harus lebih besar dari 0.');
      return;
    }

    const dto: CreateLaytimeRecordDto = {
      contractId: this.contractId,
      operationType: this.form.operationType,
      startTime: new Date(this.form.startTime).toISOString(),
      endTime: new Date(this.form.endTime).toISOString(),
      laytimeHours: this.form.laytimeHours,
      notes: this.form.notes || undefined,
    };

    this.saving.set(true);

    this.laytimeService
      .create(dto)
      .pipe(finalize(() => this.saving.set(false)))
      .subscribe({
        next: (record) => {
          this.records.update((list) => [record, ...list]);
          this.form = this.emptyForm();
          this.showForm.set(false);
        },
        error: (err) => {
          this.formError.set(
            err?.error?.message ?? 'Gagal menyimpan data laytime.',
          );
        },
      });
  }

  deleteRecord(id: string): void {
    if (!confirm('Hapus catatan laytime ini?')) return;

    this.deletingId.set(id);

    this.laytimeService
      .delete(id)
      .pipe(finalize(() => this.deletingId.set(null)))
      .subscribe({
        next: () =>
          this.records.update((list) => list.filter((r) => r.id !== id)),
        error: (err) => {
          this.error.set(
            err?.error?.message ?? 'Gagal menghapus data laytime.',
          );
        },
      });
  }

  completeContract(): void {
    if (
      !confirm(
        'Selesaikan kontrak ini? Total laytime adjustment dan final settlement akan dihitung otomatis dan tidak bisa diubah lagi setelah ini.',
      )
    ) {
      return;
    }

    this.completeError.set(null);
    this.completing.set(true);

    this.contractService
      .complete(this.contractId)
      .pipe(finalize(() => this.completing.set(false)))
      .subscribe({
        next: () => this.completed.emit(),
        error: (err) => {
          this.completeError.set(
            err?.error?.message ?? 'Gagal menyelesaikan kontrak.',
          );
        },
      });
  }
}
