/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { MapboxService } from './mapboxBase';

describe('Service: Mapbox', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MapboxService]
    });
  });

  it('should ...', inject([MapboxService], (service: MapboxService) => {
    expect(service).toBeTruthy();
  }));
});
