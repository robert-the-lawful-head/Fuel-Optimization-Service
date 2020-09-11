import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";

import FlatfileImporter from "flatfile-csv-importer";
import { Store } from "@ngrx/store";

import { State } from "../../../store/reducers";
import { breadcrumbSet } from "../../../store/actions";

// Services
import { RampfeesService } from "../../../services/rampfees.service";
import { AircraftsService } from "../../../services/aircrafts.service";
import { SharedService } from "../../../layouts/shared-service";
import { Parametri } from "../../../services/paremeters.service";

import * as SharedEvents from "../../../models/sharedEvents";

// Components
import { RampFeesDialogNewFeeComponent } from "../ramp-fees-dialog-new-fee/ramp-fees-dialog-new-fee.component";
import { RampFeesImportInformationComponent } from "../ramp-fees-import-information-dialog/ramp-fees-import-information-dialog.component";
import { RampFeesCategoryComponent } from "../ramp-fees-category/ramp-fees-category.component";

const BREADCRUMBS: any[] = [
    {
        title: "Main",
        link: "/default-layout",
    },
    {
        title: "Ramp Fees",
        link: "/default-layout/ramp-fees",
    },
];

@Component({
    selector: "app-ramp-fees-home",
    templateUrl: "./ramp-fees-home.component.html",
    styleUrls: ["./ramp-fees-home.component.scss"],
})
export class RampFeesHomeComponent implements OnInit, AfterViewInit, OnDestroy {
    @ViewChild("customRampFeeCat")
    public customRampFeeCat: RampFeesCategoryComponent;

    public pageTitle = "Ramp Fees";
    public breadcrumb: any[] = BREADCRUMBS;
    public rampFees: any[];
    public requiresUpdate = false;
    public expirationDate: any;
    public locationChangedSubscription: any;
    public aircraftTypes: any[];

    LICENSE_KEY = "9eef62bd-4c20-452c-98fd-aa781f5ac111";

    results = "[]";

    private importer: FlatfileImporter;

    constructor(
        private store: Store<State>,
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
        this.store.dispatch(breadcrumbSet({ breadcrumbs: BREADCRUMBS }));
        this.initRampfees();
    }

    ngAfterViewInit() {
        this.locationChangedSubscription = this.sharedService.changeEmitted$.subscribe(
            (message) => {
                if (message === SharedEvents.locationChangedEvent) {
                    this.rampFees = null;
                    this.initRampfees();
                }
            }
        );
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

        FlatfileImporter.setVersion(2);
        this.initializeImporter();
        this.importer.setCustomer({
            userId: "1",
            name: "WebsiteImport",
        });
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

    public informForRampFees() {
        const dialogRef = this.importFeesInfoDialog.open(
            RampFeesImportInformationComponent,
            {}
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            else {
                this.launchImporter();
            }

        });
    }

    async launchImporter() {
        if (!this.LICENSE_KEY) {
            return alert("Set LICENSE_KEY on Line 13 before continuing.");
        }
        try {
            const results = await this.importer.requestDataFromUser();
            this.importer.displayLoader();

            if (results) {
                results.data.forEach((result) => {
                    result.fboid = this.sharedService.currentUser.fboId;
                });
                console.log(results.data);

                this.rampFeesService
                    .importrampfees(results.data)
                    .subscribe((data: any) => {
                        if (data) {
                            this.importer.displaySuccess(
                                "Data successfully imported!"
                            );
                            setTimeout(() => {
                                this.loadRampFees();
                            }, 1500);
                        }
                    });
            }
        } catch (e) { }
    }

    initializeImporter() {
        this.importer = new FlatfileImporter(this.LICENSE_KEY, {
            fields: [
                {
                    label: "ICAO",
                    alternates: ["Icao"],
                    key: "icao",
                    description: "Icao",
                },
                {
                    label: "FBO",
                    alternates: ["fbo"],
                    key: "fbo",
                    description: "FBO",
                },
                {
                    label: "Make",
                    alternates: ["make"],
                    key: "Make",
                    description: "Aircraft Make",
                },
                {
                    label: "Model",
                    alternates: ["model"],
                    key: "Model",
                    description: "Tail",
                },
                {
                    label: "Ramp Fee ($)",
                    alternates: ["Ramp Fee", "ramp fee"],
                    key: "RampFee",
                    description: "Ramp Fee",
                },
                {
                    label: "Waived At (gal)",
                    alternates: ["Waived At", "waived at"],
                    key: "WaivedAt",
                    description: "Waived Fees",
                },
                {
                    label: "Landing",
                    alternates: ["landing"],
                    key: "Landing",
                    description: "Landing Fees",
                },
                {
                    label: "Overnight",
                    alternates: ["overnight", "overnight fees"],
                    key: "Overnight",
                    description: "Overnight Fees",
                },
                {
                    label: "Tail Number",
                    alternates: ["tail number", "tail-number"],
                    key: "TailNumber",
                    description: "Tail Number",
                },
                {
                    label: "Aircraft Size",
                    alternates: ["aircreft size"],
                    key: "aircraftsize",
                    description: "Aircraft Size",
                },
                {
                    label: "Avoidance",
                    alternates: ["avoidance"],
                    key: "avoidance",
                    description: "Avoidance",
                },
            ],
            type: "RampFees",
            allowInvalidSubmit: true,
            managed: true,
            allowCustom: true,
            disableManualInput: false,
        });
    }
}
