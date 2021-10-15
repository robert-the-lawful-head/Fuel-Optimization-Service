import {
    AfterViewInit,
    Component,
    OnDestroy,
    OnInit,
    ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
/*import FlatfileImporter from 'flatfile-csv-importer';*/

import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvents from '../../../models/sharedEvents';
import { AircraftsService } from '../../../services/aircrafts.service';
import { Parametri } from '../../../services/paremeters.service';
// Services
import { RampfeesService } from '../../../services/rampfees.service';
import { RampFeesCategoryComponent } from '../ramp-fees-category/ramp-fees-category.component';
// Components
import { RampFeesDialogNewFeeComponent } from '../ramp-fees-dialog-new-fee/ramp-fees-dialog-new-fee.component';
import { RampFeesImportInformationComponent } from '../ramp-fees-import-information-dialog/ramp-fees-import-information-dialog.component';

const BREADCRUMBS: any[] = [
    {
        link: '/default-layout',
        title: 'Main',
    },
    {
        link: '/default-layout/ramp-fees',
        title: 'Ramp Fees',
    },
];

@Component({
    selector: 'app-ramp-fees-home',
    styleUrls: ['./ramp-fees-home.component.scss'],
    templateUrl: './ramp-fees-home.component.html',
})
export class RampFeesHomeComponent implements OnInit, AfterViewInit, OnDestroy {
    @ViewChild('customRampFeeCat')
    public customRampFeeCat: RampFeesCategoryComponent;

    public pageTitle = 'Ramp Fees';
    public breadcrumb: any[] = BREADCRUMBS;
    public rampFees: any[];
    public requiresUpdate = false;
    public expirationDate: any;
    public locationChangedSubscription: any;
    public aircraftTypes: any[];

    /*LICENSE_KEY = '9eef62bd-4c20-452c-98fd-aa781f5ac111';*/

    results = '[]';

    /*private importer: FlatfileImporter;*/

    constructor(
        private rampFeesService: RampfeesService,
        private sharedService: SharedService,
        private aircraftsService: AircraftsService,
        public newRampFeeDialog: MatDialog,
        public importFeesInfoDialog: MatDialog,
        private messageService: Parametri
    ) {
        this.sharedService.titleChange(this.pageTitle);
        this.aircraftsService
            .getAll()
            .subscribe((data: any) => (this.aircraftTypes = data));
    }

    ngOnInit() {
        this.initRampfees();
    }

    ngAfterViewInit() {
        this.locationChangedSubscription =
            this.sharedService.changeEmitted$.subscribe((message) => {
                if (message === SharedEvents.locationChangedEvent) {
                    this.rampFees = null;
                    this.initRampfees();
                }
            });
    }

    ngOnDestroy() {
        if (this.locationChangedSubscription) {
            this.locationChangedSubscription.unsubscribe();
        }
    }

    public initRampfees() {
        this.rampFeesService
            .getForFbo({ oid: this.sharedService.currentUser.fboId })
            .subscribe((data: any) => {
                this.rampFees = data;

                if (data) {
                    this.expirationDate = data[1].expirationDate;
                }

                this.messageService.updateMessage(this.expirationDate);
                this.messageService
                    .getMessage()
                    .subscribe(
                        (mymessage: any) => (this.expirationDate = mymessage)
                    );
            });

        //FlatfileImporter.setVersion(2);
        //this.initializeImporter();
        //this.importer.setCustomer({
        //    name: 'WebsiteImport',
        //    userId: '1',
        //});
    }

    public addNewRampFeeClicked() {
        const dialogRef = this.newRampFeeDialog.open(
            RampFeesDialogNewFeeComponent,
            {
                data: {},
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            result.fboId = this.sharedService.currentUser.fboId;
            this.rampFeesService.add(result).subscribe((data: any) => {
                this.loadRampFees();
            });
        });
    }

    public rampFeeFieldChanged() {
        this.requiresUpdate = true;
    }

    public rampFeeDeleted() {
        this.loadRampFees();
    }

    public rampFeeRequiresUpdate() {
        this.rampFees.forEach((fee) => {
            fee.ExpirationDate = this.expirationDate;
            this.updateRampFee(fee);
        });
        this.messageService.updateMessage(this.expirationDate);
        this.loadRampFees();
    }

    public saveChanges() {
        this.rampFees.forEach((fee) => {
            if (fee.requiresUpdate) {
                this.updateRampFee(fee);
            }
        });
        this.requiresUpdate = false;
    }

    //public informForRampFees() {
    //    const dialogRef = this.importFeesInfoDialog.open(
    //        RampFeesImportInformationComponent,
    //        {}
    //    );

    //    dialogRef.afterClosed().subscribe((result) => {
    //        if (!result) {
    //            return;
    //        } else {
    //            this.launchImporter();
    //        }
    //    });
    //}


    //[#hz0jtd] FlatFile importer was requested to be removed
    //async launchImporter() {
    //    if (!this.LICENSE_KEY) {
    //        return alert('Set LICENSE_KEY on Line 13 before continuing.');
    //    }
    //    try {
    //        const results = await this.importer.requestDataFromUser();
    //        this.importer.displayLoader();

    //        if (results) {
    //            results.data.forEach((result) => {
    //                result.fboid = this.sharedService.currentUser.fboId;
    //            });

    //            this.rampFeesService
    //                .importrampfees(results.data)
    //                .subscribe((data: any) => {
    //                    if (data) {
    //                        this.importer.displaySuccess(
    //                            'Data successfully imported!'
    //                        );
    //                        setTimeout(() => {
    //                            this.loadRampFees();
    //                        }, 1500);
    //                    }
    //                });
    //        }
    //    } catch (e) {}
    //}

    //initializeImporter() {
    //    this.importer = new FlatfileImporter(this.LICENSE_KEY, {
    //        allowCustom: true,
    //        allowInvalidSubmit: true,
    //        disableManualInput: false,
    //        fields: [
    //            {
    //                alternates: ['Icao'],
    //                description: 'Icao',
    //                key: 'icao',
    //                label: 'ICAO',
    //            },
    //            {
    //                alternates: ['fbo'],
    //                description: 'FBO',
    //                key: 'fbo',
    //                label: 'FBO',
    //            },
    //            {
    //                alternates: ['make'],
    //                description: 'Aircraft Make',
    //                key: 'Make',
    //                label: 'Make',
    //            },
    //            {
    //                alternates: ['model'],
    //                description: 'Tail',
    //                key: 'Model',
    //                label: 'Model',
    //            },
    //            {
    //                alternates: ['Ramp Fee', 'ramp fee'],
    //                description: 'Ramp Fee',
    //                key: 'RampFee',
    //                label: 'Ramp Fee ($)',
    //            },
    //            {
    //                alternates: ['Waived At', 'waived at'],
    //                description: 'Waived Fees',
    //                key: 'WaivedAt',
    //                label: 'Waived At (gal)',
    //            },
    //            {
    //                alternates: ['landing'],
    //                description: 'Landing Fees',
    //                key: 'Landing',
    //                label: 'Landing',
    //            },
    //            {
    //                alternates: ['overnight', 'overnight fees'],
    //                description: 'Overnight Fees',
    //                key: 'Overnight',
    //                label: 'Overnight',
    //            },
    //            {
    //                alternates: ['tail number', 'tail-number'],
    //                description: 'Tail Number',
    //                key: 'TailNumber',
    //                label: 'Tail Number',
    //            },
    //            {
    //                alternates: ['aircreft size'],
    //                description: 'Aircraft Size',
    //                key: 'aircraftsize',
    //                label: 'Aircraft Size',
    //            },
    //            {
    //                alternates: ['avoidance'],
    //                description: 'Avoidance',
    //                key: 'avoidance',
    //                label: 'Avoidance',
    //            },
    //        ],
    //        managed: true,
    //        type: 'RampFees',
    //    });
    //}

    // Private Methods
    private updateRampFee(fee) {
        if (!fee.fboid) {
            fee.fboid = this.sharedService.currentUser.fboId;
        }
        if (fee.oid && fee.oid > 0) {
            this.rampFeesService.update(fee).subscribe((data: any) => {});
        } else {
            this.rampFeesService.add(fee).subscribe((data: any) => {
                fee.oid = data.oid;
            });
        }
    }

    private loadRampFees() {
        this.rampFees = undefined;
        this.rampFeesService
            .getForFbo({ oid: this.sharedService.currentUser.fboId })
            .subscribe((data: any) => {
                this.rampFees = data;
                if (this.customRampFeeCat) {
                    this.customRampFeeCat.refreshData();
                }
            });
    }
}
