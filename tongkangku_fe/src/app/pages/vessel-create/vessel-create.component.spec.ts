import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VesselCreateComponent } from './vessel-create.component';

describe('VesselCreateComponent', () => {
  let component: VesselCreateComponent;
  let fixture: ComponentFixture<VesselCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VesselCreateComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(VesselCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
