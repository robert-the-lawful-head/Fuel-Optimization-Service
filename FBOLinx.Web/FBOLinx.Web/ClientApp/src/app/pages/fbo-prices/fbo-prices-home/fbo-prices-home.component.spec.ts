/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { FboPricesHomeComponent } from './fbo-prices-home.component';
import { element, by } from 'protractor';

let component: FboPricesHomeComponent;
let fixture: ComponentFixture<FboPricesHomeComponent>;

describe('fbo-prices-home component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ FboPricesHomeComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(FboPricesHomeComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));

    it('should format numbers', function () {
        expect(element(by.binding('-val | number:4')).getText()).toBe('-1,234.5679');
    });
});
