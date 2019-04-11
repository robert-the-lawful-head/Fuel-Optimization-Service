/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { ContactsHomeComponent } from './contacts-home.component';

let component: ContactsHomeComponent;
let fixture: ComponentFixture<ContactsHomeComponent>;

describe('contacts-home component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ ContactsHomeComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(ContactsHomeComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});