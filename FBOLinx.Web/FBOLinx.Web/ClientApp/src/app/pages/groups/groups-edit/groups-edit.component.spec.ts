/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { GroupsEditComponent } from './groups-edit.component';

let component: GroupsEditComponent;
let fixture: ComponentFixture<GroupsEditComponent>;

describe('groups-edit component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ GroupsEditComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(GroupsEditComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});