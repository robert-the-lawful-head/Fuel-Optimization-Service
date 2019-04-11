/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { AirportAutocompleteComponent } from './airport-autocomplete.component';

let component: AirportAutocompleteComponent;
let fixture: ComponentFixture<AirportAutocompleteComponent>;

describe('airport-autocomplete component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ AirportAutocompleteComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(AirportAutocompleteComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});