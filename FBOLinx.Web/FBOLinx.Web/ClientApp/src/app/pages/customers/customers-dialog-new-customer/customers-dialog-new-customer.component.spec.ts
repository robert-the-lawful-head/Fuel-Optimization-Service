/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { CustomersDialogNewCustomerComponent } from './customers-dialog-new-customer.component';

let component: CustomersDialogNewCustomerComponent;
let fixture: ComponentFixture<CustomersDialogNewCustomerComponent>;

describe('customers-dialog-new-customer component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ CustomersDialogNewCustomerComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(CustomersDialogNewCustomerComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});