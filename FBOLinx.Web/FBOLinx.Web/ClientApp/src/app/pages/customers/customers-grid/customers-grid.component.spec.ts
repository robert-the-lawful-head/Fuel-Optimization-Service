/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { CustomersGridComponent } from './customers-grid.component';

let component: CustomersGridComponent;
let fixture: ComponentFixture<CustomersGridComponent>;

describe('customers-grid component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ CustomersGridComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(CustomersGridComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});