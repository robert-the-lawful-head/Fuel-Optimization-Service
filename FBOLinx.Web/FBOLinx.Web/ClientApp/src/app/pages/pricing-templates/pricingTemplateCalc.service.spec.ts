/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { PricingTemplateCalcService } from './pricingTemplateCalc.service';

describe('Service: PricingTemplateCalcs', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PricingTemplateCalcService]
    });
  });

  it('should ...', inject([PricingTemplateCalcService], (service: PricingTemplateCalcService) => {
    expect(service).toBeTruthy();
  }));
});
