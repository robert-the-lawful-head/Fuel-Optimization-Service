import { Component, EventEmitter, Input, Output, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

//Services
import { AircraftsService } from '../../../services/aircrafts.service';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import { CustomerAircraftsDialogNewAircraftComponent } from '../customer-aircrafts-dialog-new-aircraft/customer-aircrafts-dialog-new-aircraft.component';
import { CustomerAircraftsEditComponent } from '../customer-aircrafts-edit/customer-aircrafts-edit.component';

@Component({
    selector: 'app-customer-aircrafts-grid',
    templateUrl: './customer-aircrafts-grid.component.html',
    styleUrls: ['./customer-aircrafts-grid.component.scss']
})
/** customer-aircrafts-grid component*/
export class CustomerAircraftsGridComponent implements OnInit {

    //Input/Output Bindings
    @Output() editCustomerAircraftClicked = new EventEmitter<any>();
    @Output() newCustomerAircraftAdded = new EventEmitter<any>();
    @Input() customer: any;
    @Input() customerAircraftsData: Array<any>;
    @Input() pricingTemplatesData: Array<any>;

    //Public Members
    public customerAircraftsDataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = ['tailNumber', 'aircraftType', 'aircraftSize', 'aircraftPricingTemplate', 'delete'];
    public resultsLength: number = 0;
    public aircraftSizes: Array<any>;
    public aircraftTypes: Array<any>;
    public isLoadingAircraftTypes: boolean = false;
    public pageIndex: number = 0;

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    /** customers-grid ctor */
    constructor(public newCustomerAircraftDialog: MatDialog, public editCustomerAircraftDialog: MatDialog,
        private aircraftsService: AircraftsService,
        private customerAircraftsService: CustomeraircraftsService,
        private sharedService: SharedService) {

        this.isLoadingAircraftTypes = true;
        this.aircraftsService.getAll().subscribe((data: any) => {
            this.aircraftTypes = data;
            this.isLoadingAircraftTypes = false;
        });
    }

    ngOnInit() {
        if (!this.customerAircraftsData)
            return;
        this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
        this.customerAircraftsDataSource = new MatTableDataSource(this.customerAircraftsData);
        this.customerAircraftsDataSource.sort = this.sort;
        this.customerAircraftsDataSource.paginator = this.paginator;

        if (sessionStorage.getItem('pageIndex')) {
            this.paginator.pageIndex = sessionStorage.getItem('pageIndex') as any;
            sessionStorage.removeItem('pageIndex');
            sessionStorage.removeItem('isCustomerEdit');
        }
        else {
            this.paginator.pageIndex = 0;
        }

        this.resultsLength = this.customerAircraftsData.length;
        this.customerAircraftsDataSource.sortingDataAccessor = (item, property) => {            
            switch(property) {
                case 'aircraftType':
                    return item.make + ' ' + item.model;
                case 'aircraftSize':
                    switch(item.size) {
                        case 8: //Single Engine Piston
                            return 1;
                        case 11: //Light Twin
                            return 2;
                        case 12: //Heavy Twin
                            return 3;
                        case 4: //Light Helicopter
                            return 4;
                        case 9: //Medium Helicopter
                            return 5;
                        case 10: //Heavy Helicopter
                            return 6;
                        case 13: //Light Turboprop
                            return 7;
                        case 6: //Single Turboprop
                            return 8;
                        case 14: //Medium Turboprop
                            return 9;
                        case 15: //Heavy Turboprop
                            return 10;
                        case 7: //Very Light Jet
                            return 11;
                        case 1: //Light Jet
                            return 12;
                        case 2: //Medium Jet
                            return 13;
                        case 3: //Heavy Jet
                            return 14;
                        case 16: //Super Heavy Jet
                            return 15;
                        case 5: //Wide Body
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

    public newCustomerAircraft() {
        const dialogRef = this.newCustomerAircraftDialog.open(CustomerAircraftsDialogNewAircraftComponent, {
            width: '450px',
            data: { oid: 0 }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (!result)
                return;
            result.groupId = this.sharedService.currentUser.groupId;
            result.customerId = this.customer.customerId;
            this.customerAircraftsService.add(result).subscribe((data: any) => {
                this.newCustomerAircraftAdded.emit(data);
            });
        });
    }

    public editCustomerAircraft(customerAircraft) {
        if (customerAircraft) {
            const dialogRef = this.editCustomerAircraftDialog.open(CustomerAircraftsEditComponent, {
                width: '450px',
                data: { oid: customerAircraft.oid }
            });

            dialogRef.afterClosed().subscribe(result => {
                if (!result)
                    return;

                if (result.toDelete) {
                    this.customerAircraftsService.remove(result).subscribe((data: any) => {
                    });
                }

                let aircraftSizes = [];
                this.aircraftsService.getAircraftSizes().subscribe((data: any) => {
                    if (data) {

                        aircraftSizes = data;
                    }

                });

                this.customerAircraftsService.get({ oid: result.oid })
                    .subscribe((data: any) => {
                        if (data) {

                            let selectedAircraft = this.customerAircraftsData.find(x => x.oid == result.oid);

                            if (selectedAircraft) {
                                selectedAircraft.tailNumber = data.tailNumber;
                                selectedAircraft.make = data.make;
                                selectedAircraft.model = data.model;
                                selectedAircraft.size = data.size;

                                if (data.size) {
                                    let sizeText = aircraftSizes.find(x => x.value == data.size);

                                    if (sizeText) {
                                        selectedAircraft.aircraftSizeDescription = sizeText.description;
                                    }
                                }
                            }
                        }
                    });
            });
        }
    }

    public deleteCustomerAircraft(customerAircraft: any) {
        //TODO: add delete prompt and logic
    }

    public applyFilter(filterValue: string) {
        this.customerAircraftsDataSource.filter = filterValue.trim().toLowerCase();
    }

    onPageChanged(e: any) {
        sessionStorage.setItem('pageIndex', e.pageIndex);
    }

    private onMarginChange(newValue: any, customerAircraft: any) {
        const { oid, aircraftId, tailNumber, groupId, customerId, make, model, size, pricingTemplateName } = customerAircraft;
        this.customerAircraftsService.updateTemplate(this.sharedService.currentUser.fboId, {
            oid,
            aircraftId,
            tailNumber,
            groupId,
            customerId,
            make,
            model,
            size,
            pricingTemplateName
        }).subscribe((data: any) => {});
    }
}
