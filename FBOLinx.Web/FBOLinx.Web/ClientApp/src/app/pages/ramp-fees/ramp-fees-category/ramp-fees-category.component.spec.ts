/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { RampFeesCategoryComponent } from './ramp-fees-category.component';

let component: RampFeesCategoryComponent;
let fixture: ComponentFixture<RampFeesCategoryComponent>;

describe('ramp-fees-category component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ RampFeesCategoryComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(RampFeesCategoryComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});