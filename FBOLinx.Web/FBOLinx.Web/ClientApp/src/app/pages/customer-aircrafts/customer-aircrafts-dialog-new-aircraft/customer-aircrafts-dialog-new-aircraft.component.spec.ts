/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { CustomerAircraftsDialogNewAircraftComponent } from './customer-aircrafts-dialog-new-aircraft.component';

let component: CustomerAircraftsDialogNewAircraftComponent;
let fixture: ComponentFixture<CustomerAircraftsDialogNewAircraftComponent>;

describe('customer-aircrafts-dialog-new-aircraft component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ CustomerAircraftsDialogNewAircraftComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(CustomerAircraftsDialogNewAircraftComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});