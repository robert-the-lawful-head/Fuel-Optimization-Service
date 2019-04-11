/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { FbosGridComponent } from './fbos-grid.component';

let component: FbosGridComponent;
let fixture: ComponentFixture<FbosGridComponent>;

describe('fbos-grid component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ FbosGridComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(FbosGridComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});