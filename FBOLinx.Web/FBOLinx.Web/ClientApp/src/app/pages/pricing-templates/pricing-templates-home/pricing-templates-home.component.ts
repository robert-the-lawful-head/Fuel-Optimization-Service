import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { CustomermarginsService } from '../../../services/customermargins.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '#/default-layout'
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
        private sharedService: SharedService,
        public deleteFBODialog: MatDialog    ) {

        this.sharedService.emitChange(this.pageTitle);
        pricingTemplatesService.getByFbo(this.sharedService.currentUser.fboId).subscribe((data: any) => this.pricingTemplatesData = data);
        this.currentPricingTemplate = null;
    }

    public editPricingTemplateClicked(pricingTemplate) {
        this.router.navigate(['/default-layout/pricing-templates/' + pricingTemplate.oid]);
    }

    public deletePricingTemplateClicked(pricingTemplate) {
        const dialogRef = this.deleteFBODialog.open(DeleteConfirmationComponent, {
            data: { item: pricingTemplate, description: 'pricing template' }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (!result)
                return;
            this.pricingTemplatesData = null;
            this.pricingTemplatesService.remove(pricingTemplate).subscribe((data: any) => {
                this.pricingTemplatesService.getByFbo(this.sharedService.currentUser.fboId).subscribe((data: any) => {
                    this.pricingTemplatesData = data;
                    this.currentPricingTemplate = null;
                });
            });
        });
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
