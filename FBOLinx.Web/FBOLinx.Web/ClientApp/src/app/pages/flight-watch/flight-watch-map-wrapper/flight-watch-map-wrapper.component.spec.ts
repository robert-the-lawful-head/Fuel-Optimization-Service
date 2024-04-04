/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { FlightWatchMapWrapperComponent } from './flight-watch-map-wrapper.component';

describe('FlightWatchMapWrapperComponent', () => {
  let component: FlightWatchMapWrapperComponent;
  let fixture: ComponentFixture<FlightWatchMapWrapperComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FlightWatchMapWrapperComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FlightWatchMapWrapperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
