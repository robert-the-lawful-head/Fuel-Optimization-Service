/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { CustomersHomeComponent } from './customers-home.component';

let component: CustomersHomeComponent;
let fixture: ComponentFixture<CustomersHomeComponent>;

describe('customers-home component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ CustomersHomeComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(CustomersHomeComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});