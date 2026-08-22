import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateRentalOfferComponent } from './create-rental-offer.component';

describe('CreateRentalOfferComponent', () => {
  let component: CreateRentalOfferComponent;
  let fixture: ComponentFixture<CreateRentalOfferComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateRentalOfferComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateRentalOfferComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
