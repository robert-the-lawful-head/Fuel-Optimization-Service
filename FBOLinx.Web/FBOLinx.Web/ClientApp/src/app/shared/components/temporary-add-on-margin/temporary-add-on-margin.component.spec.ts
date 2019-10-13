/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { TemporaryAddOnMarginComponent } from './temporary-add-on-margin.component';

let component: TemporaryAddOnMarginComponent;
let fixture: ComponentFixture<TemporaryAddOnMarginComponent>;

describe('temprorary-add-on-margin component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [TemporaryAddOnMarginComponent],
            imports: [BrowserModule],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(TemporaryAddOnMarginComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});
