import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { CategoryVesselService } from '../../core/services/category-vessel.service';
import { categoryVessel } from '../../shared/interface/category-vessel';
import { portInterface } from '../../shared/interface/port';
import { PortService } from '../../core/services/port.service';
import { AuthService } from '../../core/services/auth.service';
import { VesselService } from '../../core/services/vessel.service';

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
  private vesselSvc = inject(VesselService);

  vesselCategoryData: categoryVessel[] | null = null;
  portData: portInterface[] | null = null;

  isLoadingCategory: boolean = true;
  isLoadingPort: boolean = true;
  isSubmitting: boolean = false;
  errorMessage = "";

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
    this.portSvc.getAll().subscribe({
      next: (response: any) => {
        this.portData = response.data;
        
        console.log(this.portData);
        
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
        console.log("data kapal lawut",response);
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
    const payload = this.createVessel.value;
    console.log('Payload data kapal:', payload);
    this.vesselSvc.CreateVessel(payload).
    subscribe({
      next: () => {
        alert("data vessel berhasil ditambahkan!");
        
      },
      error : (err) => {
        this.errorMessage = err.error?.
        message || 'Gagal Membuat Vessel!'
      }
    })

  }
}