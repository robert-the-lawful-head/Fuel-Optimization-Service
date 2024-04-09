/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { FileHelper} from './file.helper';

describe('Helper: FileHelper', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FileHelper]
    });
  });

  it('should ...', inject([FileHelper], (service: FileHelper) => {
    expect(service).toBeTruthy();
  }));
});
