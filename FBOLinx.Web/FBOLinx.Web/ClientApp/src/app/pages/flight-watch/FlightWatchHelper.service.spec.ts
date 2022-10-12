/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { FlightWatchHelperService } from './FlightWatchHelper.service';

describe('Service: FlightWatchHelper', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FlightWatchHelperService]
    });
  });

  it('should ...', inject([FlightWatchHelperService], (service: FlightWatchHelperService) => {
    expect(service).toBeTruthy();
  }));
});
