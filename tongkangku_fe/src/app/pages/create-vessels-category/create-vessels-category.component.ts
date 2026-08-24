import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CategoryVesselService } from '../../core/services/category-vessel.service';
import { categoryVessel } from '../../shared/interface/category-vessel';

@Component({
  selector: 'app-create-vessels-category',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './create-vessels-category.component.html',
  styleUrl: './create-vessels-category.component.css'
})
export class CreateVesselsCategoryComponent {
  private fb = inject(FormBuilder);
  private vesselCategoryService = inject(CategoryVesselService);
  private router = inject(Router);

  isSubmitting = false;
  errorMessage: string | null = null;

  categoryForm: FormGroup = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3)]],
    description: ['', [Validators.required]],
  });

  onSubmit(): void {
    if (this.categoryForm.invalid) {
      this.categoryForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = null;

    const payload: categoryVessel = this.categoryForm.value;

    this.vesselCategoryService.createCategory(payload).subscribe({
      next: (res) => {
        this.isSubmitting = false;
        alert("Kategori kapal berhasil ditambahkan!");
        this.router.navigate(['/vessels/own']);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.message || 'Gagal menambahkan kategori kapal.';
      }
    });
  }
}