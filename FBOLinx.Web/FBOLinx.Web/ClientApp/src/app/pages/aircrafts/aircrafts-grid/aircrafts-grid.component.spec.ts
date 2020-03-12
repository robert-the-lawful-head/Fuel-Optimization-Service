/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { AircraftsGridComponent } from './aircrafts-grid.component';

let component: AircraftsGridComponent;
let fixture: ComponentFixture<AircraftsGridComponent>;

describe('aircrafts-grid component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ AircraftsGridComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(AircraftsGridComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});