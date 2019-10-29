/// <reference path="../../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { DistributionWizardReviewComponent } from './distribution-wizard-review.component';

let component: DistributionWizardReviewComponent;
let fixture: ComponentFixture<DistributionWizardReviewComponent>;

describe('distribution-wizard-review component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [DistributionWizardReviewComponent],
            imports: [BrowserModule],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(DistributionWizardReviewComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});
