/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { RampFeesHomeComponent } from './ramp-fees-home.component';

let component: RampFeesHomeComponent;
let fixture: ComponentFixture<RampFeesHomeComponent>;

describe('ramp-fees-home component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ RampFeesHomeComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(RampFeesHomeComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});