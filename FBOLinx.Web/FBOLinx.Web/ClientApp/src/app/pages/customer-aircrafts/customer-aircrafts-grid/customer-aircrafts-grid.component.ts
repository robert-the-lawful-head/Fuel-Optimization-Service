import { identifierName } from '@angular/compiler';
import {
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSelectChange } from '@angular/material/select';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
/*import FlatfileImporter from 'flatfile-csv-importer';*/

import { SharedService } from '../../../layouts/shared-service';
import { AircraftpricesService } from '../../../services/aircraftprices.service';
// Services
import { AircraftsService } from '../../../services/aircrafts.service';
import { CustomcustomertypesService } from '../../../services/customcustomertypes.service';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
// Components
import { CustomerAircraftsDialogNewAircraftComponent } from '../customer-aircrafts-dialog-new-aircraft/customer-aircrafts-dialog-new-aircraft.component';
import { CustomerAircraftsEditComponent } from '../customer-aircrafts-edit/customer-aircrafts-edit.component';
import { CustomerAircraftSelectModelComponent } from '../customer-aircrafts-select-model-dialog/customer-aircrafts-select-model-dialog.component';

@Component({
    selector: 'app-customer-aircrafts-grid',
    styleUrls: ['./customer-aircrafts-grid.component.scss'],
    templateUrl: './customer-aircrafts-grid.component.html',
})
export class CustomerAircraftsGridComponent implements OnInit {
    // Input/Output Bindings
    @Output() updateCustomerPricingTemplate = new EventEmitter<any>();
    @Input() customer: any;
    @Input() customerAircraftsData: Array<any>;
    @Input() pricingTemplatesData: Array<any>;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    // Public Members
    public customerAircraftsDataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = [
        'tailNumber',
        'aircraftType',
        'aircraftSize',
        'aircraftPricingTemplate',
        'delete',
    ];
    public resultsLength = 0;
    public aircraftSizes: Array<any>;
    public aircraftTypes: Array<any>;
    public isLoadingAircraftTypes = false;
    public pageIndex = 0;
    public customerInfobyGroupId : any;

    /*LICENSE_KEY = '9eef62bd-4c20-452c-98fd-aa781f5ac111';*/

    results = '[]';

    /*private importer: FlatfileImporter;*/

    constructor(
        public newCustomerAircraftDialog: MatDialog,
        public editCustomerAircraftDialog: MatDialog,
        public selectModalAircraftDialog: MatDialog,
        private aircraftsService: AircraftsService,
        private customerAircraftsService: CustomeraircraftsService,
        private aircraftPricesService: AircraftpricesService,
        private customCustomerTypeService: CustomcustomertypesService,
        private sharedService: SharedService ,
        private route : ActivatedRoute
    ) {
        this.isLoadingAircraftTypes = true;
        this.aircraftsService.getAll().subscribe((data: any) => {
            this.aircraftTypes = data;
            this.isLoadingAircraftTypes = false;
        });
    }

    ngOnInit() {
        this.customerInfobyGroupId =  this.route.snapshot.paramMap.get('id');
        if (!this.customerAircraftsData) {
            return;
        }
        this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
        this.customerAircraftsDataSource = new MatTableDataSource(
            this.customerAircraftsData
        );
        this.customerAircraftsDataSource.sort = this.sort;
        this.customerAircraftsDataSource.paginator = this.paginator;

        if (sessionStorage.getItem('pageIndex')) {
            this.paginator.pageIndex = sessionStorage.getItem(
                'pageIndex'
            ) as any;
            sessionStorage.removeItem('pageIndex');
            sessionStorage.removeItem('isCustomerEdit');
        } else {
            this.paginator.pageIndex = 0;
        }

        this.resultsLength = this.customerAircraftsData.length;
        this.customerAircraftsDataSource.sortingDataAccessor = (
            item,
            property
        ) => {
            switch (property) {
                case 'aircraftType':
                    return item.make + ' ' + item.model;
                case 'aircraftSize':
                    switch (item.size) {
                        case 8: // Single Engine Piston
                            return 1;
                        case 11: // Light Twin
                            return 2;
                        case 12: // Heavy Twin
                            return 3;
                        case 4: // Light Helicopter
                            return 4;
                        case 9: // Medium Helicopter
                            return 5;
                        case 10: // Heavy Helicopter
                            return 6;
                        case 13: // Light Turboprop
                            return 7;
                        case 6: // Single Turboprop
                            return 8;
                        case 14: // Medium Turboprop
                            return 9;
                        case 15: // Heavy Turboprop
                            return 10;
                        case 7: // Very Light Jet
                            return 11;
                        case 1: // Light Jet
                            return 12;
                        case 2: // Medium Jet
                            return 13;
                        case 3: // Heavy Jet
                            return 14;
                        case 16: // Super Heavy Jet
                            return 15;
                        case 5: // Wide Body
                            return 16;
                        default:
                            return 17;
                    }
                case 'aircraftPricingTemplate':
                    return item.pricingTemplateName;
                default:
                    return item[property];
            }
        };

        //FlatfileImporter.setVersion(2);
        //this.initializeImporter();
        //this.importer.setCustomer({
        //    name: 'WebsiteImport',
        //    userId: '1',
        //});
    }

    public newCustomerAircraft() {
        const dialogRef = this.newCustomerAircraftDialog.open(
            CustomerAircraftsDialogNewAircraftComponent,
            {
                data: { oid: 0 },
                width: '450px',
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            const id = this.route.snapshot.paramMap.get('id');
            result.groupId = this.sharedService.currentUser.groupId;
            result.customerId = this.customer.customerId;
            this.customerAircraftsService.add(result , this.sharedService.currentUser.oid ,id ).subscribe(() => {
                this.customerAircraftsService
                    .getCustomerAircraftsByGroupAndCustomerId(
                        this.sharedService.currentUser.groupId,
                        this.sharedService.currentUser.fboId,
                        result.customerId
                    )
                    .subscribe((data: any) => {
                        this.customerAircraftsData = data;
                        this.customerAircraftsDataSource =
                            new MatTableDataSource(this.customerAircraftsData);
                        this.customerAircraftsDataSource.sort = this.sort;
                        this.customerAircraftsDataSource.paginator =
                            this.paginator;
                    });
            });
        });
    }

    public editCustomerAircraft(customerAircraft: any) {
        if (customerAircraft) {
            console.log(this.route.snapshot.paramMap.get('id'));
            const dialogRef = this.editCustomerAircraftDialog.open(
                CustomerAircraftsEditComponent,
                {
                    data: {
                        disableDelete:
                            customerAircraft.isFuelerlinxNetwork &&
                            customerAircraft.addedFrom === 1,
                        oid: customerAircraft.oid,
                       customerGroupId : this.route.snapshot.paramMap.get('id')
                    },
                    width: '450px',
                }
            );

            dialogRef.afterClosed().subscribe((result) => {
                if (!result) {
                    return;
                }

                if (result.toDelete) {
                    const id = this.route.snapshot.paramMap.get('id');
                    this.customerAircraftsService
                        .remove(result , this.sharedService.currentUser.oid , id)
                        .subscribe(() => {
                            this.customerAircraftsService
                                .getCustomerAircraftsByGroupAndCustomerId(
                                    this.sharedService.currentUser.groupId,
                                    this.sharedService.currentUser.fboId,
                                    result.customerId
                                )
                                .subscribe((data: any) => {
                                    this.customerAircraftsData = data;
                                    this.customerAircraftsDataSource =
                                        new MatTableDataSource(
                                            this.customerAircraftsData
                                        );
                                    this.customerAircraftsDataSource.sort =
                                        this.sort;
                                    this.customerAircraftsDataSource.paginator =
                                        this.paginator;
                                });
                        });
                }

                this.aircraftsService
                    .getAircraftSizes()
                    .subscribe((data: any) => {
                        if (data) {
                            this.customerAircraftsService
                                .getCustomerAircraftsByGroupAndCustomerId(
                                    this.sharedService.currentUser.groupId,
                                    this.sharedService.currentUser.fboId,
                                    result.customerId
                                )
                                .subscribe((dataOutput: any) => {
                                    this.customerAircraftsData = dataOutput;
                                    this.customerAircraftsDataSource =
                                        new MatTableDataSource(
                                            this.customerAircraftsData
                                        );
                                    this.customerAircraftsDataSource.sort =
                                        this.sort;
                                    this.customerAircraftsDataSource.paginator =
                                        this.paginator;
                                });
                        }
                    });
            });
        }
    }

    public applyFilter(filterValue: string) {
        this.customerAircraftsDataSource.filter = filterValue
            .trim()
            .toLowerCase();
    }

    onPageChanged(e: any) {
        sessionStorage.setItem('pageIndex', e.pageIndex);
    }

    public onMarginChange(event: MatSelectChange, customerAircraft: any) {
        const {
            aircraftId,
            customerId,
            groupId,
            make,
            model,
            oid,
            pricingTemplateId,
            size,
            tailNumber,
        } = customerAircraft;
        this.customerAircraftsService
            .updateTemplate(this.sharedService.currentUser.fboId, {
                aircraftId,
                customerId,
                groupId,
                make,
                model,
                oid,
                oldPricingTemplateId: pricingTemplateId,
                pricingTemplateId: event.value,
                size,
                tailNumber,
            })
            .subscribe(() => {
                customerAircraft.pricingTemplateId = event.value;
                const pricingTemplateIds =
                    this.customerAircraftsDataSource.data.map(
                        (d) => d.pricingTemplateId
                    );
                if (pricingTemplateIds.every((v) => v === pricingTemplateId)) {
                    this.customCustomerTypeService
                        .updateForFboAndCustomer({
                            customerId,
                            fboId: this.sharedService.currentUser.fboId,
                            pricingTemplateId,
                        })
                        .subscribe(() => {
                            this.updateCustomerPricingTemplate.emit(
                                pricingTemplateId
                            );
                        });

                    this.aircraftPricesService
                        .removeMultiple(this.customerAircraftsData)
                        .subscribe(() => {
                            this.customerAircraftsDataSource.data.forEach(
                                (element) => {
                                    element.pricingTemplateId = null;
                                    element.pricingTemplateName = '';
                                }
                            );
                        });
                }
            });
    }

    //[#hz0jtd] FlatFile importer was requested to be removed
    //async launchImporter() {
    //    if (!this.LICENSE_KEY) {
    //        return alert('Set LICENSE_KEY on Line 13 before continuing.');
    //    }
    //    try {
    //        const results = await this.importer.requestDataFromUser();
    //        this.importer.displayLoader();

    //        if (results) {
    //            let aircraftSizes = [];
    //            this.aircraftsService
    //                .getAircraftSizes()
    //                .subscribe((asizes: any) => {
    //                    if (asizes) {
    //                        aircraftSizes = asizes;
    //                        results.data.forEach((result) => {
    //                            result.groupid =
    //                                this.sharedService.currentUser.groupId;
    //                            result.customerId = this.customer.customerId;

    //                            if (result.Size) {
    //                                const sizeText = aircraftSizes.find(
    //                                    (x) => x.description === result.Size
    //                                );

    //                                if (sizeText) {
    //                                    result.Size = sizeText.value;
    //                                }
    //                            }
    //                        });

    //                        this.customerAircraftsService
    //                            .import(results.data)
    //                            .subscribe((cadata: any) => {
    //                                let allGood = true;
    //                                cadata.forEach((result) => {
    //                                    if (!result.isImported) {
    //                                        allGood = false;
    //                                    }
    //                                });
    //                                if (allGood) {
    //                                    this.importer.displaySuccess(
    //                                        'Data successfully imported!'
    //                                    );
    //                                    setTimeout(() => {
    //                                        this.customerAircraftsService
    //                                            .getCustomerAircraftsByGroupAndCustomerId(
    //                                                this.sharedService
    //                                                    .currentUser.groupId,
    //                                                this.sharedService
    //                                                    .currentUser.fboId,
    //                                                cadata[0].customerId
    //                                            )
    //                                            .subscribe((data: any) => {
    //                                                this.customerAircraftsData =
    //                                                    data;
    //                                                this.customerAircraftsDataSource =
    //                                                    new MatTableDataSource(
    //                                                        this.customerAircraftsData
    //                                                    );
    //                                                this.customerAircraftsDataSource.sort =
    //                                                    this.sort;
    //                                                this.customerAircraftsDataSource.paginator =
    //                                                    this.paginator;
    //                                            });
    //                                    }, 1500);
    //                                } else {
    //                                    this.importer.displaySuccess(
    //                                        'Import is finished, please click ok to see the results.'
    //                                    );
    //                                    const dialogRef =
    //                                        this.selectModalAircraftDialog.open(
    //                                            CustomerAircraftSelectModelComponent,
    //                                            {
    //                                                data: { aircrafts: cadata },
    //                                            }
    //                                        );

    //                                    dialogRef
    //                                        .afterClosed()
    //                                        .subscribe((result) => {
    //                                            if (!result) {
    //                                                this.customerAircraftsService
    //                                                    .getCustomerAircraftsByGroupAndCustomerId(
    //                                                        this.sharedService
    //                                                            .currentUser
    //                                                            .groupId,
    //                                                        this.sharedService
    //                                                            .currentUser
    //                                                            .fboId,
    //                                                        cadata[0].customerId
    //                                                    )
    //                                                    .subscribe(
    //                                                        (data: any) => {
    //                                                            this.customerAircraftsData =
    //                                                                data;
    //                                                            this.customerAircraftsDataSource =
    //                                                                new MatTableDataSource(
    //                                                                    this.customerAircraftsData
    //                                                                );
    //                                                            this.customerAircraftsDataSource.sort =
    //                                                                this.sort;
    //                                                            this.customerAircraftsDataSource.paginator =
    //                                                                this.paginator;
    //                                                        }
    //                                                    );
    //                                            }
    //                                        });
    //                                }
    //                            });
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
    //                alternates: [
    //                    'tail',
    //                    'plane tail',
    //                    'N-number',
    //                    'Nnumber',
    //                    'Tail Number',
    //                ],
    //                description: 'Tail',
    //                key: 'TailNumber',
    //                label: 'Tail',
    //                validators: [
    //                    {
    //                        error: 'this field is required',
    //                        validate: 'required',
    //                    },
    //                ],
    //            },
    //            {
    //                alternates: ['make', 'manufacturer'],
    //                description: 'Aircraft Make',
    //                key: 'AircraftMake',
    //                label: 'Make',
    //                validators: [
    //                    {
    //                        error: 'this field is required',
    //                        validate: 'required',
    //                    },
    //                ],
    //            },
    //            {
    //                alternates: ['model', 'plane model'],
    //                description: 'Aircraft Model',
    //                key: 'Model',
    //                label: 'Model',
    //                validators: [
    //                    {
    //                        error: 'this field is required',
    //                        validate: 'required',
    //                    },
    //                ],
    //            },
    //            {
    //                alternates: ['size', 'plane size'],
    //                description: 'Plane Size',
    //                key: 'Size',
    //                label: 'Size',
    //                validators: [
    //                    {
    //                        error: 'this field is required',
    //                        validate: 'required',
    //                    },
    //                ],
    //            },
    //        ],
    //        managed: true,
    //        type: 'Aircrafts',
    //    });
    //}
}
