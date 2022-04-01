/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { FlightWatchAircraftInfoDialogComponent } from './flight-watch-aircraft-info-dialog.component';

describe('FlightWatchAircraftInfoDialogComponent', () => {
  let component: FlightWatchAircraftInfoDialogComponent;
  let fixture: ComponentFixture<FlightWatchAircraftInfoDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FlightWatchAircraftInfoDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FlightWatchAircraftInfoDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
