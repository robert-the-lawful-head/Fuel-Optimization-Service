import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { CustomermarginsService } from '../../../services/customermargins.service';
import { SharedService } from '../../../layouts/shared-service';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '#/'
    },
    {
        title: 'Pricing Templates',
        link: '#/default-layout/pricing-templates'
    }
];

@Component({
    selector: 'app-pricing-templates-home',
    templateUrl: './pricing-templates-home.component.html',
    styleUrls: ['./pricing-templates-home.component.scss']
})
/** pricing-templates-home component*/
export class PricingTemplatesHomeComponent {

    //Public Members
    public pageTitle: string = 'Pricing Templates';
    public breadcrumb: any[] = BREADCRUMBS;
    public pricingTemplatesData: Array<any>;
    public currentPricingTemplate: any;

    constructor(private route: ActivatedRoute,
        private router: Router,
        private pricingTemplatesService: PricingtemplatesService,
        public newPricingTemplateDialog: MatDialog,
        private customerMarginsService: CustomermarginsService,
        private sharedService: SharedService    ) {

        this.sharedService.emitChange(this.pageTitle);
        pricingTemplatesService.getByFbo(this.sharedService.currentUser.fboId).subscribe((data: any) => this.pricingTemplatesData = data);
        this.currentPricingTemplate = null;
    }

    public editPricingTemplateClicked(pricingTemplate) {
        this.router.navigate(['/default-layout/pricing-templates/' + pricingTemplate.oid]);
    }

    public deletePricingTemplateClicked(pricingTemplate) {
        
    }

    public savePricingTemplateClicked() {
        this.pricingTemplatesService.update(this.currentPricingTemplate).subscribe((data: any) => {
            this.pricingTemplatesService.getByFbo(this.sharedService.currentUser.fboId).subscribe((data: any) => {
                this.pricingTemplatesData = data;
                this.currentPricingTemplate = null;
            });
        });
    }

    public cancelPricingTemplateEditclicked() {
        this.currentPricingTemplate = null;
    }
}
