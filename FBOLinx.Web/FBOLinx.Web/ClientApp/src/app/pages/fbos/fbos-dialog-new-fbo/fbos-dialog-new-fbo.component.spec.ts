/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { FbosDialogNewFboComponent } from './fbos-dialog-new-fbo.component';

let component: FbosDialogNewFboComponent;
let fixture: ComponentFixture<FbosDialogNewFboComponent>;

describe('fbos-dialog-new-fbo component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ FbosDialogNewFboComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(FbosDialogNewFboComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});