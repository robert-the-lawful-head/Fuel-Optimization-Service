/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { UsersGridComponent } from './users-grid.component';

let component: UsersGridComponent;
let fixture: ComponentFixture<UsersGridComponent>;

describe('users-grid component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ UsersGridComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(UsersGridComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});