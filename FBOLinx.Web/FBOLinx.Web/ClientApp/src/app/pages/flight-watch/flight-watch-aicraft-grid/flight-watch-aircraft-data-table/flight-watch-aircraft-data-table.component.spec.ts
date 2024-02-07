/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { FlightWatchAircraftDataTableComponent } from './flight-watch-aircraft-data-table.component';

describe('FlightWatchAircraftDataTableComponent', () => {
  let component: FlightWatchAircraftDataTableComponent;
  let fixture: ComponentFixture<FlightWatchAircraftDataTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FlightWatchAircraftDataTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FlightWatchAircraftDataTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
