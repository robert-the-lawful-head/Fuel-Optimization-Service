/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { GroupsDialogNewGroupComponent } from './groups-dialog-new-group.component';

let component: GroupsDialogNewGroupComponent;
let fixture: ComponentFixture<GroupsDialogNewGroupComponent>;

describe('groups-dialog-new-group component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ GroupsDialogNewGroupComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(GroupsDialogNewGroupComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});