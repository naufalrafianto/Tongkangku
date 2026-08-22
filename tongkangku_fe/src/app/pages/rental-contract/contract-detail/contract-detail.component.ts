import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CurrencyPipe, DatePipe, DecimalPipe } from '@angular/common';
import { RentalContract } from '../../../shared/types/rental-contract/rentral-contract.type';

@Component({
  selector: 'app-contract-detail',
  standalone: true,
  imports: [CurrencyPipe, DatePipe, DecimalPipe],
  templateUrl: './contract-detail.component.html',
  styleUrl: './contract-detail.component.css'
})
export class ContractDetailComponent {
  @Input() contract: RentalContract | null = null;

  @Input() loading = false;

  @Input() error: string | null = null;

  @Input() chartererName = '';

  @Output() retry = new EventEmitter<void>();

  onRetry(): void {
    this.retry.emit();
  }
}
