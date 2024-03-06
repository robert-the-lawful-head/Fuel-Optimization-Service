/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { FlightWatchMapSharedService } from './flight-watch-map-shared.service';

describe('Service: FlightWatchMapShared', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FlightWatchMapSharedService]
    });
  });

  it('should ...', inject([FlightWatchMapSharedService], (service: FlightWatchMapSharedService) => {
    expect(service).toBeTruthy();
  }));
});
