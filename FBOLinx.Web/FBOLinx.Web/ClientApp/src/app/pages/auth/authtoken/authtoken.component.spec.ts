/// <reference path="../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { AuthtokenComponent } from './authtoken.component';

let component: AuthtokenComponent;
let fixture: ComponentFixture<AuthtokenComponent>;

describe('authtoken component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ AuthtokenComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(AuthtokenComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});