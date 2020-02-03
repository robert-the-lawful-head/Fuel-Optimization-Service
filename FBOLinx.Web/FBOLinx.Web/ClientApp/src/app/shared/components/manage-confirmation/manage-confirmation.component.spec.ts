/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { ManageConfirmationComponent } from './manage-confirmation.component';

let component: ManageConfirmationComponent;
let fixture: ComponentFixture<ManageConfirmationComponent>;

describe('manage-confirmation component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ ManageConfirmationComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(ManageConfirmationComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});