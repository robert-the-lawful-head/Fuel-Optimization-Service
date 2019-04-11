/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { FbosContactsComponent } from './fbos-contacts.component';

let component: FbosContactsComponent;
let fixture: ComponentFixture<FbosContactsComponent>;

describe('fbos-contacts component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ FbosContactsComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(FbosContactsComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});