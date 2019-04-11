/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { GroupsHomeComponent } from './groups-home.component';

let component: GroupsHomeComponent;
let fixture: ComponentFixture<GroupsHomeComponent>;

describe('groups-home component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ GroupsHomeComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(GroupsHomeComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});