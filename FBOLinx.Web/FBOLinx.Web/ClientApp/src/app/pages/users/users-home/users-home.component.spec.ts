/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { UsersHomeComponent } from './users-home.component';

let component: UsersHomeComponent;
let fixture: ComponentFixture<UsersHomeComponent>;

describe('users-home component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ UsersHomeComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(UsersHomeComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});