/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { DashboardAdminComponent } from './dashboard-admin.component';

let component: DashboardAdminComponent;
let fixture: ComponentFixture<DashboardAdminComponent>;

describe('dashboard-admin component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ DashboardAdminComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(DashboardAdminComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});