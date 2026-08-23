import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';

import { CategoryVesselService } from '../../core/services/category-vessel.service';
import { categoryVessel } from '../../shared/interface/category-vessel';
import { portInterface } from '../../shared/interface/port';
import { PortService } from '../../core/services/port.service';
import { VesselService } from '../../core/services/vessel.service';
import {
  VesselResponseDto,
  VesselStatus
} from '../../shared/interface/InterfaceVessel';

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
  private vesselSvc = inject(VesselService);
  private router = inject(Router);

 


  vesselCategoryData: categoryVessel[] | null = null;
  portData: portInterface[] | null = null;
  myVesselsData: VesselResponseDto[] = [];



  vesselStatus = VesselStatus;

  activeTab: 'list' | 'create' = 'list';

  isLoadingCategory = true;
  isLoadingPort = true;
  isLoading = true;
  isSubmitting = false;

  errorMessage = '';



  createVessel = this.fb.group({
    name: ['', [Validators.required]],
    categoryId: ['', [Validators.required]],
    portId: ['', [Validators.required]],
    capacityFeed: [null, [Validators.required, Validators.min(0)]],
    dwtCapacity: [null, [Validators.required, Validators.min(0)]],
    year: [new Date().getFullYear(), [Validators.required]],
    ratePerDay: [null, [Validators.required, Validators.min(0)]],
    status: [VesselStatus.Available, [Validators.required]]
  });

  ngOnInit(): void {
    this.fetchMyVessels();
  }

  showMyVessels(): void {
    this.activeTab = 'list';
    this.errorMessage = '';

    this.fetchMyVessels();
  }

  showCreateForm(): void {
    this.activeTab = 'create';
    this.errorMessage = '';

    // Load data form hanya ketika tab create dibuka
    if (!this.vesselCategoryData) {
      this.fetchCategoryVessel();
    }

    if (!this.portData) {
      this.fetchPort();
    }
  }

  fetchMyVessels(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.vesselSvc.GetMyVessels().subscribe({

      next: (response) => {

        console.log('My Vessels:', response);

        if (response.success && response.data) {
          this.myVesselsData = response.data;
        } else {
          this.myVesselsData = [];
        }

        this.isLoading = false;
      },

      error: (err) => {

        console.error('Gagal mengambil data kapal saya:', err);

        this.errorMessage =
          err.error?.message || 'Gagal memuat data kapal Anda.';

        this.myVesselsData = [];
        this.isLoading = false;
      }

    });
  }

  fetchCategoryVessel(): void {

    this.isLoadingCategory = true;

    this.vesselCategorySvc.GetAllCategory().subscribe({

      next: (response: any) => {

        this.vesselCategoryData = response.data;

        console.log(
          'Data kategori kapal:',
          this.vesselCategoryData
        );

        this.isLoadingCategory = false;
      },

      error: (err) => {

        console.error(
          'Gagal mengambil data kategori:',
          err
        );

        this.isLoadingCategory = false;
      }

    });
  }

  fetchPort(): void {

    this.isLoadingPort = true;

    this.portSvc.getAll().subscribe({

      next: (response: any) => {

        this.portData = response.data;

        console.log(
          'Data pelabuhan:',
          this.portData
        );

        this.isLoadingPort = false;
      },

      error: (err) => {

        console.error(
          'Gagal mengambil data pelabuhan:',
          err
        );

        this.isLoadingPort = false;
      }

    });
  }

  onSubmit(): void {

    if (this.createVessel.invalid) {

      this.createVessel.markAllAsTouched();

      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    const payload = this.createVessel.value;

    console.log(
      'Payload data kapal:',
      payload
    );

    this.vesselSvc.CreateVessel(payload).subscribe({

      next: () => {

        alert('Data vessel berhasil ditambahkan!');

        this.isSubmitting = false;

        // Reset form
        this.createVessel.reset({
          name: '',
          categoryId: '',
          portId: '',
          capacityFeed: null,
          dwtCapacity: null,
          year: new Date().getFullYear(),
          ratePerDay: null,
          status: VesselStatus.Available
        });
        this.activeTab = 'list';
        this.fetchMyVessels();
      },

      error: (err) => {

        console.error(
          'Gagal membuat vessel:',
          err
        );

        this.errorMessage =
          err.error?.message ||
          'Gagal Membuat Vessel!';

        this.isSubmitting = false;
      }

    });
  }
 deleteVessel(vessel: VesselResponseDto): void {

    const confirmed = confirm(
      `Apakah kamu yakin ingin menghapus kapal "${vessel.name}"?`
    );

    if (!confirmed) {
      return;
    }

    this.vesselSvc.DeleteVessels(vessel.id).subscribe({

      next: () => {

        alert('Kapal berhasil dihapus');
        this.myVesselsData =
          this.myVesselsData.filter(
            v => v.id !== vessel.id
          );
      },

      error: (err) => {

        console.error(
          'Gagal menghapus kapal:',
          err
        );

        alert(
          err.error?.message ||
          'Gagal menghapus kapal'
        );
      }

    });
  }
}