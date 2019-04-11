/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { UsersEditComponent } from './users-edit.component';

let component: UsersEditComponent;
let fixture: ComponentFixture<UsersEditComponent>;

describe('users-edit component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ UsersEditComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(UsersEditComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});