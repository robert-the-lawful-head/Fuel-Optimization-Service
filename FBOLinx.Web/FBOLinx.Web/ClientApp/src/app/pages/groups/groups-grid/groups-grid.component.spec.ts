/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { GroupsGridComponent } from './groups-grid.component';

let component: GroupsGridComponent;
let fixture: ComponentFixture<GroupsGridComponent>;

describe('groups-grid component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ GroupsGridComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(GroupsGridComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});