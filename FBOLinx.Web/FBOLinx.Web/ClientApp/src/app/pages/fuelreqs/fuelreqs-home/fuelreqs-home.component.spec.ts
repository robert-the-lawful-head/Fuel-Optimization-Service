/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { FuelreqsHomeComponent } from './fuelreqs-home.component';

let component: FuelreqsHomeComponent;
let fixture: ComponentFixture<FuelreqsHomeComponent>;

describe('fuelreqs-home component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ FuelreqsHomeComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(FuelreqsHomeComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});