/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { FbosEditComponent } from './fbos-edit.component';

let component: FbosEditComponent;
let fixture: ComponentFixture<FbosEditComponent>;

describe('fbos-edit component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ FbosEditComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(FbosEditComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});