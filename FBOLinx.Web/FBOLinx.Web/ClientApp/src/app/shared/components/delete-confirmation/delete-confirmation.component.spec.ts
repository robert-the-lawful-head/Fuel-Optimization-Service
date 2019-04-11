/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { DeleteConfirmationComponent } from './delete-confirmation.component';

let component: DeleteConfirmationComponent;
let fixture: ComponentFixture<DeleteConfirmationComponent>;

describe('delete-confirmation component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ DeleteConfirmationComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(DeleteConfirmationComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});