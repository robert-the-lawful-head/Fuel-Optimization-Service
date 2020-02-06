import { Component } from '@angular/core';
import { MatDialog } from '@angular/material';
import { Router } from '@angular/router';

//Services
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';

const BREADCRUMBS: any[] = [
    {
        title: 'Main',
        link: '#/default-layout'
    },
    {
        title: 'Margin Templates',
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
    public pageTitle: string = 'Margin Templates';
    public breadcrumb: any[] = BREADCRUMBS;
    public pricingTemplatesData: Array<any>;
    public currentPricingTemplate: any;

    constructor(
        private router: Router,
        private pricingTemplatesService: PricingtemplatesService,
        public newPricingTemplateDialog: MatDialog,
        private sharedService: SharedService,
        public deleteFBODialog: MatDialog
    ) {
        this.sharedService.emitChange(this.pageTitle);
        this.currentPricingTemplate = null;
    }

    public editPricingTemplateClicked(pricingTemplate) {
        this.router.navigate(['/default-layout/pricing-templates/' + pricingTemplate.oid]);
    }

    public deletePricingTemplateClicked(pricingTemplate) {
        const dialogRef = this.deleteFBODialog.open(DeleteConfirmationComponent, {
            data: { item: pricingTemplate, description: 'margin template' }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (!result) return;
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

    public newPricingTemplateAdded(event) {
        this.pricingTemplatesData = null;
        this.pricingTemplatesService.getByFbo(this.sharedService.currentUser.fboId).subscribe((data: any) => {
            this.pricingTemplatesData = data;
            this.currentPricingTemplate = null;
        });
    }
}
