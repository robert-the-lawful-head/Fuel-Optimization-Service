/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { CloseConfirmationComponent } from './close-confirmation.component';

let component: CloseConfirmationComponent;
let fixture: ComponentFixture<CloseConfirmationComponent>;

describe('close-confirmation component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ CloseConfirmationComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(CloseConfirmationComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});