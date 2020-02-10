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

    //Public Members
    public customerAircraftssDataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = ['tailNumber', 'aircraftType', 'aircraftSize', 'aircraftPricingTemplate', 'delete'];
    public resultsLength: number = 0;
    public aircraftSizes: Array<any>;
    public aircraftTypes: Array<any>;
    public isLoadingAircraftTypes: boolean = false;

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
        this.customerAircraftssDataSource = new MatTableDataSource(this.customerAircraftsData);
        this.customerAircraftssDataSource.sort = this.sort;
        this.customerAircraftssDataSource.paginator = this.paginator;
        this.resultsLength = this.customerAircraftsData.length;
    }

    public newCustomerAircraft() {
        const dialogRef = this.newCustomerAircraftDialog.open(CustomerAircraftsDialogNewAircraftComponent, {
            width: '450px',
            data: { oid: 0 }
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('Dialog data: ', result);
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
        console.log(customerAircraft);

        if (customerAircraft) {
            const dialogRef = this.editCustomerAircraftDialog.open(CustomerAircraftsEditComponent, {
                width: '450px',
                data: { oid: customerAircraft.oid }
            });

            dialogRef.afterClosed().subscribe(result => {
                console.log('Dialog data: ', result);
                if (!result)
                    return;

                if (result.toDelete) {
                    this.customerAircraftsService.remove(result).subscribe((data: any) => {
                        console.log(data);
                    });
                }
                
                this.editCustomerAircraftClicked.emit(result);
            });
        }

        //const clonedRecord = Object.assign({}, customerAircraft);
        //this.editCustomerAircraftClicked.emit(clonedRecord);
    }

    public deleteCustomerAircraft(customerAircraft) {
        //TODO: add delete prompt and logic
    }

    public applyFilter(filterValue: string) {
        this.customerAircraftssDataSource.filter = filterValue.trim().toLowerCase();
    }
}
