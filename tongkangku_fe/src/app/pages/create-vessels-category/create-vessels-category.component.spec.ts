import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateVesselsCategoryComponent } from './create-vessels-category.component';

describe('CreateVesselsCategoryComponent', () => {
  let component: CreateVesselsCategoryComponent;
  let fixture: ComponentFixture<CreateVesselsCategoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateVesselsCategoryComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateVesselsCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
