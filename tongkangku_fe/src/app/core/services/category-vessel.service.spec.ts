import { TestBed } from '@angular/core/testing';

import { CategoryVesselService } from './category-vessel.service';

describe('CategoryVesselService', () => {
  let service: CategoryVesselService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CategoryVesselService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
