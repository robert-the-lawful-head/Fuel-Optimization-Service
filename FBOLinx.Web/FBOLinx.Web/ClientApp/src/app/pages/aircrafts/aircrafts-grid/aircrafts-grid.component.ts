import { Component, EventEmitter, Input, OnInit, Output, ViewChild, } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSelectChange } from '@angular/material/select';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

import { SharedService } from '../../../layouts/shared-service';
// Services
import { AircraftsService } from '../../../services/aircrafts.service';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
import { CustomerAircraftsEditComponent } from '../../customer-aircrafts/customer-aircrafts-edit/customer-aircrafts-edit.component';

@Component({
    selector: 'app-aircrafts-grid',
    styleUrls: [ './aircrafts-grid.component.scss' ],
    templateUrl: './aircrafts-grid.component.html',
})
export class AircraftsGridComponent implements OnInit {
    // Input/Output Bindings
    @Output() editAircraftClicked = new EventEmitter<any>();
    @Input() aircraftsData: Array<any>;
    @Input() pricingTemplatesData: Array<any>;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    // Public Members
    public aircraftsDataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = [
        'tailNumber',
        'aircraftType',
        'aircraftSize',
        'company',
        'aircraftPricingTemplate',
    ];
    public resultsLength = 0;
    public aircraftSizes: Array<any>;
    public aircraftTypes: Array<any>;
    public isLoadingAircraftTypes = false;
    public pageIndex = 0;

    constructor(
        public newCustomerAircraftDialog: MatDialog,
        public editCustomerAircraftDialog: MatDialog,
        private aircraftsService: AircraftsService,
        private customerAircraftsService: CustomeraircraftsService,
        private sharedService: SharedService
    ) {
        this.isLoadingAircraftTypes = true;
        this.aircraftsService.getAll().subscribe((data: any) => {
            this.aircraftTypes = data;
            this.isLoadingAircraftTypes = false;
        });
    }

    ngOnInit() {
        if (!this.aircraftsData) {
            return;
        }

        this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
        this.aircraftsDataSource = new MatTableDataSource(this.aircraftsData);
        this.aircraftsDataSource.sort = this.sort;
        this.aircraftsDataSource.paginator = this.paginator;

        if (sessionStorage.getItem('pageIndex')) {
            this.paginator.pageIndex = sessionStorage.getItem(
                'pageIndex'
            ) as any;
            sessionStorage.removeItem('pageIndex');
            sessionStorage.removeItem('isCustomerEdit');
        } else {
            this.paginator.pageIndex = 0;
        }

        this.resultsLength = this.aircraftsData.length;
        this.aircraftsDataSource.sortingDataAccessor = (item, property) => {
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
    }

    public editCustomerAircraft(customerAircraft) {
        if (customerAircraft) {
            const dialogRef = this.editCustomerAircraftDialog.open(
                CustomerAircraftsEditComponent,
                {
                    data: {
                        disableDelete: customerAircraft.isFuelerlinxNetwork && customerAircraft.addedFrom,
                        oid: customerAircraft.oid,
                    },
                    width: '450px',
                }
            );

            dialogRef.afterClosed().subscribe((result) => {
                if (!result) {
                    return;
                }

                if (result.toDelete) {
                    this.customerAircraftsService
                        .remove(result)
                        .subscribe(() => {
                            this.customerAircraftsService
                                .getCustomerAircraftsByGroup(
                                    this.sharedService.currentUser.groupId
                                )
                                .subscribe((data: any) => {
                                    this.aircraftsData = data;
                                    this.aircraftsDataSource = new MatTableDataSource(
                                        this.aircraftsData
                                    );
                                    this.aircraftsDataSource.sort = this.sort;
                                    this.aircraftsDataSource.paginator = this.paginator;
                                });
                        });
                }

                let aircraftSizes = [];
                this.aircraftsService
                    .getAircraftSizes()
                    .subscribe((data: any) => {
                        if (data) {
                            aircraftSizes = data;
                        }
                    });

                this.customerAircraftsService
                    .get({ oid: result.oid })
                    .subscribe((data: any) => {
                        if (data) {
                            const selectedAircraft = this.aircraftsData.find(
                                (x) => x.oid === result.oid
                            );

                            if (selectedAircraft) {
                                selectedAircraft.tailNumber = data.tailNumber;
                                selectedAircraft.make = data.make;
                                selectedAircraft.model = data.model;
                                selectedAircraft.size = data.size;

                                if (data.size) {
                                    const sizeText = aircraftSizes.find(
                                        (x) => x.value === data.size
                                    );

                                    if (sizeText) {
                                        selectedAircraft.aircraftSizeDescription =
                                            sizeText.description;
                                    }
                                }
                            }
                        }
                    });

                this.editAircraftClicked.emit(result);
            });
        }
    }

    public applyFilter(filterValue: string) {
        this.aircraftsDataSource.filter = filterValue.trim().toLowerCase();
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
            .subscribe((data: any) => {
                customerAircraft.pricingTemplateId = event.value;
            });
    }

    onPageChanged(e: any) {
        sessionStorage.setItem('pageIndex', e.pageIndex);
    }
}
