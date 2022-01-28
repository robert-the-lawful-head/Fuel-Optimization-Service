import { AfterViewInit, Component, OnDestroy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';

import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvents from '../../../models/sharedEvents';
// Services
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { FbopricesService } from "../../../services/fboprices.service";

// Components
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';
import { pricingTemplateGridSet } from '../../../store/actions';
import { State } from '../../../store/reducers';


const BREADCRUMBS: any[] = [
    {
        link: '/default-layout',
        title: 'Main',
    },
    {
        link: '/default-layout/pricing-templates',
        title: 'ITP Margin Templates',
    },
];

@Component({
    selector: 'app-pricing-templates-home',
    styleUrls: ['./pricing-templates-home.component.scss'],
    templateUrl: './pricing-templates-home.component.html',
})
export class PricingTemplatesHomeComponent implements AfterViewInit, OnDestroy {
    // Public Members
    public pageTitle = 'ITP Margin Templates';
    public breadcrumb: any[] = BREADCRUMBS;
    public pricingTemplatesData: Array<any>;
    public locationChangedSubscription: any;

    constructor(
        private router: Router,
        private store: Store<State>,
        private pricingTemplatesService: PricingtemplatesService,
        private sharedService: SharedService,
        private fboPricesService: FbopricesService,
        public deleteFBODialog: MatDialog
    ) {
        this.sharedService.titleChange(this.pageTitle);
        this.loadPricingTemplateData();
    }

    ngAfterViewInit() {
        this.locationChangedSubscription =
            this.sharedService.changeEmitted$.subscribe((message) => {
                if (message === SharedEvents.locationChangedEvent) {
                    this.loadPricingTemplateData();
                }
            });
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

    public editPricingTemplateClicked($event) {
        this.store.dispatch(
            pricingTemplateGridSet({
                filter: $event.filter,
                order: $event.order,
                orderBy: $event.orderBy,
                page: $event.page,
            })
        );
        this.router
            .navigate([
                '/default-layout/pricing-templates/' + $event.pricingTemplateId,
            ])
            .then(() => {});
    }

    public deletePricingTemplateClicked(pricingTemplate) {
        const dialogRef = this.deleteFBODialog.open(
            DeleteConfirmationComponent,
            {
                autoFocus: false,
                data: { description: 'margin template', item: pricingTemplate },
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

                    this.fboPricesService.handlePriceChangeCleanUp(this.sharedService.currentUser.fboId);
                });

            this.sharedService.NotifyPricingTemplateComponent(
                'updateComponent'
            );
        });
    }

    public newPricingTemplateAdded() {
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
