/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { EmailContentSelectionComponent } from './email-content-selection.component';

let component: EmailContentSelectionComponent;
let fixture: ComponentFixture<EmailContentSelectionComponent>;

describe('email-content-selection component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ EmailContentSelectionComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(EmailContentSelectionComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});