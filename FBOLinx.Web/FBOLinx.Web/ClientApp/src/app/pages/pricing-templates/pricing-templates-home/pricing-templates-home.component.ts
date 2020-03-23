import { Component, OnInit, AfterViewInit, OnDestroy } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { Router } from "@angular/router";

// Services
import { PricingtemplatesService } from "../../../services/pricingtemplates.service";
import { SharedService } from "../../../layouts/shared-service";

import * as SharedEvents from "../../../models/sharedEvents";

// Components
import { DeleteConfirmationComponent } from "../../../shared/components/delete-confirmation/delete-confirmation.component";

const BREADCRUMBS: any[] = [
    {
        title: "Main",
        link: "#/default-layout",
    },
    {
        title: "Margin Templates",
        link: "#/default-layout/pricing-templates",
    },
];

@Component({
    selector: "app-pricing-templates-home",
    templateUrl: "./pricing-templates-home.component.html",
    styleUrls: ["./pricing-templates-home.component.scss"],
})
export class PricingTemplatesHomeComponent implements AfterViewInit, OnDestroy {
    // Public Members
    public pageTitle = "Margin Templates";
    public breadcrumb: any[] = BREADCRUMBS;
    public pricingTemplatesData: Array<any>;
    public locationChangedSubscription: any;

    constructor(
        private router: Router,
        private pricingTemplatesService: PricingtemplatesService,
        public newPricingTemplateDialog: MatDialog,
        private sharedService: SharedService,
        public deleteFBODialog: MatDialog
    ) {
        this.sharedService.emitChange(this.pageTitle);
        this.loadPricingTemplateData();
    }

    ngAfterViewInit() {
        this.locationChangedSubscription = this.sharedService.changeEmitted$.subscribe(
            (message) => {
                if (message === SharedEvents.locationChangedEvent) {
                    this.loadPricingTemplateData();
                }
            }
        );
    }

    ngOnDestroy() {
        if (this.locationChangedSubscription) {
            this.locationChangedSubscription.unsubscribe();
        }
    }

    public loadPricingTemplateData() {
        this.pricingTemplatesData = null;
        this.pricingTemplatesService
            .getByFbo(
                this.sharedService.currentUser.fboId,
                this.sharedService.currentUser.groupId
            )
            .subscribe((data: any) => (this.pricingTemplatesData = data));
    }

    public editPricingTemplateClicked(pricingTemplate) {
        this.router.navigate([
            "/default-layout/pricing-templates/" + pricingTemplate.oid,
        ]);
    }

    public deletePricingTemplateClicked(pricingTemplate) {
        const dialogRef = this.deleteFBODialog.open(
            DeleteConfirmationComponent,
            {
                data: { item: pricingTemplate, description: "margin template" },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            this.pricingTemplatesData = null;
            this.pricingTemplatesService
                .remove({
                    fboId: this.sharedService.currentUser.fboId,
                    oid: pricingTemplate.oid,
                })
                .subscribe(() => {
                    this.pricingTemplatesService
                        .getByFbo(
                            this.sharedService.currentUser.fboId,
                            this.sharedService.currentUser.groupId
                        )
                        .subscribe((data: any) => {
                            this.pricingTemplatesData = data;
                        });
                });

            this.sharedService.NotifyPricingTemplateComponent(
                "updatecomponent"
            );
        });
    }

    public newPricingTemplateAdded(event) {
        this.pricingTemplatesData = null;
        this.pricingTemplatesService
            .getByFbo(
                this.sharedService.currentUser.fboId,
                this.sharedService.currentUser.groupId
            )
            .subscribe((data: any) => {
                this.pricingTemplatesData = data;
            });
    }
}
