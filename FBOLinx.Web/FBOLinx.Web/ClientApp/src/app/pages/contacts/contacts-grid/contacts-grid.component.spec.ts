/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { ContactsGridComponent } from './contacts-grid.component';

let component: ContactsGridComponent;
let fixture: ComponentFixture<ContactsGridComponent>;

describe('contacts-grid component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ ContactsGridComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(ContactsGridComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});