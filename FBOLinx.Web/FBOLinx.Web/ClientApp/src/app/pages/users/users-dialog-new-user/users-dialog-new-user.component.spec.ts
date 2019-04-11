/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { UsersDialogNewUserComponent } from './users-dialog-new-user.component';

let component: UsersDialogNewUserComponent;
let fixture: ComponentFixture<UsersDialogNewUserComponent>;

describe('users-dialog-new-user component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ UsersDialogNewUserComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(UsersDialogNewUserComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});