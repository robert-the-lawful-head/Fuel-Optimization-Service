/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import {
    TestBed,
    async,
    ComponentFixture,
    ComponentFixtureAutoDetect
} from '@angular/core/testing';
import { BrowserModule, By } from '@angular/platform-browser';
import { TooltipModalComponent } from './tooltip-modal.component';

let component: TooltipModalComponent;
let fixture: ComponentFixture<TooltipModalComponent>;

describe('close-confirmation component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [TooltipModalComponent],
            imports: [BrowserModule],
            providers: [{ provide: ComponentFixtureAutoDetect, useValue: true }]
        });
        fixture = TestBed.createComponent(TooltipModalComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});
