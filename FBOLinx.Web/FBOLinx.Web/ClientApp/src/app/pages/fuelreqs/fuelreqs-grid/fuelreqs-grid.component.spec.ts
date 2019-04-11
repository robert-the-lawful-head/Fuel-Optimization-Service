/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { FuelreqsGridComponent } from './fuelreqs-grid.component';

let component: FuelreqsGridComponent;
let fixture: ComponentFixture<FuelreqsGridComponent>;

describe('fuelreqs-grid component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ FuelreqsGridComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(FuelreqsGridComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});