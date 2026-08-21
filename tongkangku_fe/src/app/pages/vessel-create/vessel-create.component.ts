import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { CategoryVesselService } from '../../core/services/category-vessel.service';
import { categoryVessel } from '../../shared/interface/category-vessel';
import { portInterface } from '../../shared/interface/port';
import { PortService } from '../../core/services/port.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-vessel-create',
  standalone: true,
  imports: [
    CommonModule, 
    RouterLink,
    ReactiveFormsModule
  ],
  templateUrl: './vessel-create.component.html',
  styleUrl: './vessel-create.component.css'
})
export class VesselCreateComponent implements OnInit {
  private vesselCategorySvc = inject(CategoryVesselService);
  private portSvc = inject(PortService);
  private fb = inject(FormBuilder);
  private authSvc = inject(AuthService)

  vesselCategoryData: categoryVessel[] | null = null;
  portData: portInterface[] | null = null;

  isLoadingCategory: boolean = true;
  isLoadingPort: boolean = true;
  isSubmitting: boolean = false;

  createVessel = this.fb.group({
    name: ['', [Validators.required]],
    categoryId: ['', [Validators.required]],
    portId: ['', [Validators.required]],
    capacityFeed: [null, [Validators.required, Validators.min(0)]],
    dwtCapacity: [null, [Validators.required, Validators.min(0)]],
    year: [new Date().getFullYear(), [Validators.required]],
    ratePerDay: [null, [Validators.required, Validators.min(0)]],
    status: [1, [Validators.required]] 
  });

  ngOnInit(): void {
    this.fetchCategoryVessel();
    this.fetchPort();
  }

  fetchPort(): void {
    this.isLoadingPort = true;
    this.portSvc.GetAllPort().subscribe({
      next: (response) => {
        this.portData = response;
        this.isLoadingPort = false;
      },
      error: (err) => {
        console.error('Gagal mengambil data pelabuhan:', err);
        this.isLoadingPort = false;
      }
    });
  }

  fetchCategoryVessel(): void {
    this.isLoadingCategory = true;
    this.vesselCategorySvc.GetAllCategory().subscribe({
      next: (response) => {
        this.vesselCategoryData = response;
        this.isLoadingCategory = false;
      },
      error: (err) => {
        console.error('Gagal mengambil data kategori:', err);
        this.isLoadingCategory = false;
      }
    });
  }

  onSubmit(): void {
    if (this.createVessel.invalid) {
      this.createVessel.markAllAsTouched();
      return;
    }
    const UserId = this.authSvc.getUserId();
      
    const payload = this.createVessel.value;
    console.log('Payload data kapal:', payload,UserId);
  }
}