/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { DashboardFboComponent } from './dashboard-fbo.component';

let component: DashboardFboComponent;
let fixture: ComponentFixture<DashboardFboComponent>;

describe('dashboard-fbo component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ DashboardFboComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(DashboardFboComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});