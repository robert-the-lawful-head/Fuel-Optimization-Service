/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { ContactsEditComponent } from './contacts-edit.component';

let component: ContactsEditComponent;
let fixture: ComponentFixture<ContactsEditComponent>;

describe('contacts-edit component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ ContactsEditComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(ContactsEditComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});