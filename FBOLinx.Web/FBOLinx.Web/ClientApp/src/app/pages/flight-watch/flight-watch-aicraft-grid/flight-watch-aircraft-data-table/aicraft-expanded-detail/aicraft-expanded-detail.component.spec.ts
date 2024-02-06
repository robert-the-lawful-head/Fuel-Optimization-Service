/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AicraftExpandedDetailComponent } from './aicraft-expanded-detail.component';

describe('AicraftExpandedDetailComponent', () => {
  let component: AicraftExpandedDetailComponent;
  let fixture: ComponentFixture<AicraftExpandedDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AicraftExpandedDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AicraftExpandedDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
