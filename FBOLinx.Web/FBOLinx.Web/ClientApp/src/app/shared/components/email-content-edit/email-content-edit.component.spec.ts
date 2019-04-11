/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { EmailContentEditComponent } from './email-content-edit.component';

let component: EmailContentEditComponent;
let fixture: ComponentFixture<EmailContentEditComponent>;

describe('email-content-edit component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ EmailContentEditComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(EmailContentEditComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});