/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { AccountProfileComponent } from './account-profile.component';

let component: AccountProfileComponent;
let fixture: ComponentFixture<AccountProfileComponent>;

describe('account-profile component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ AccountProfileComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(AccountProfileComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});