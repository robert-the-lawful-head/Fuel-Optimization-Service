/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { FeeAndTaxBreakdownDialogWrapperComponent } from './fee-and-tax-breakdown-dialog-wrapper.component';

describe('FeeAndTaxBreakdownDialogWrapperComponent', () => {
  let component: FeeAndTaxBreakdownDialogWrapperComponent;
  let fixture: ComponentFixture<FeeAndTaxBreakdownDialogWrapperComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FeeAndTaxBreakdownDialogWrapperComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FeeAndTaxBreakdownDialogWrapperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
