/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { PricingExpiredNotificationComponent } from './pricing-expired-notification.component';

let component: PricingExpiredNotificationComponent;
let fixture: ComponentFixture<PricingExpiredNotificationComponent>;

describe('pricing-expired-notification component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ PricingExpiredNotificationComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(PricingExpiredNotificationComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});