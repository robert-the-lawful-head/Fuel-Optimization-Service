/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { IncomingFavoriteAircraftInfoComponent } from './incoming-favorite-aircraft-info.component';

describe('IncomingFavoriteAircraftInfoComponent', () => {
  let component: IncomingFavoriteAircraftInfoComponent;
  let fixture: ComponentFixture<IncomingFavoriteAircraftInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncomingFavoriteAircraftInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncomingFavoriteAircraftInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
