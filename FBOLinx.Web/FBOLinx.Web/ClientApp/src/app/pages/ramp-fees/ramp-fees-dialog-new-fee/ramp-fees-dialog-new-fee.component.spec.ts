/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { RampFeesDialogNewFeeComponent } from './ramp-fees-dialog-new-fee.component';

let component: RampFeesDialogNewFeeComponent;
let fixture: ComponentFixture<RampFeesDialogNewFeeComponent>;

describe('ramp-fees-dialog-new-fee component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ RampFeesDialogNewFeeComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(RampFeesDialogNewFeeComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});