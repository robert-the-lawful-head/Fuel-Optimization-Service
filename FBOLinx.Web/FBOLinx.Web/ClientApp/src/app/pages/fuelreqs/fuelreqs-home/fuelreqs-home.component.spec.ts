/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { FuelreqsHomeComponent } from './fuelreqs-home.component';

describe('FuelreqsGridComponent', () => {
    let component: FuelreqsHomeComponent;
    let fixture: ComponentFixture<FuelreqsHomeComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [FuelreqsHomeComponent]
        })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(FuelreqsHomeComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
