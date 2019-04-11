/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { FbosHomeComponent } from './fbos-home.component';

let component: FbosHomeComponent;
let fixture: ComponentFixture<FbosHomeComponent>;

describe('fbos-home component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ FbosHomeComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(FbosHomeComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});