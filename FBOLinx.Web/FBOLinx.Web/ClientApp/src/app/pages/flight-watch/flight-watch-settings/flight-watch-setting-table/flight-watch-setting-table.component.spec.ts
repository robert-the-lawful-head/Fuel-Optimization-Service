/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { FlightWatchSettingTableComponent } from './flight-watch-setting-table.component';

describe('FlightWatchSettingTableComponent', () => {
  let component: FlightWatchSettingTableComponent;
  let fixture: ComponentFixture<FlightWatchSettingTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FlightWatchSettingTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FlightWatchSettingTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
