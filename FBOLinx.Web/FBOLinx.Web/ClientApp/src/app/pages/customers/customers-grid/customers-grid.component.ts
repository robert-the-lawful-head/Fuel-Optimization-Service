import { Component, EventEmitter, Input, Output, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { MatDialog } from '@angular/material';
import * as _ from 'lodash';

//Services
import { AuthenticationService } from '../../../services/authentication.service'
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
import { CustomersService } from '../../../services/customers.service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import { CustomersDialogNewCustomerComponent } from '../customers-dialog-new-customer/customers-dialog-new-customer.component';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';

import * as XLSX from 'xlsx';
import { CustomermarginsService } from '../../../services/customermargins.service';
import { CustomersviewedbyfboService } from '../../../services/customersviewedbyfbo.service';

@Component({
    selector: 'app-customers-grid',
    templateUrl: './customers-grid.component.html',
    styleUrls: ['./customers-grid.component.scss']
})
/** customers-grid component*/
export class CustomersGridComponent implements OnInit {

    //Input/Output Bindings
    @Output() editCustomerClicked = new EventEmitter<any>();
    @Output() customerDeleted = new EventEmitter<any>();
    @Input() customersData: Array<any>;

    @Input() pricingTemplatesData: Array<any>;

    //Public Members
    @ViewChild('customerTableContainer') table: ElementRef;
    public customersDataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = ['selectAll', 'needsAttention', 'company', 'customerCompanyTypeName', 'pricingTemplateId', 'allInPrice', 'fleetSize', 'delete'];
    public resultsLength: number = 0;
    public allCustomerAircraft: any[];
    public customerFilterType: number = 0;

    public selectAll: boolean = false;
    public selectedRows: number;
    public globalMargin: any;
    public pageIndex: number = 0;
    public pricingTemplatesDataSource: MatTableDataSource<any> = null;
    

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    /** customers-grid ctor */
    constructor(
        public newCustomerDialog: MatDialog,
        private AuthenticationService: AuthenticationService,
        private customersService: CustomersService,
        private sharedService: SharedService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private customersViewedByFboService: CustomersviewedbyfboService,
        public deleteCustomerDialog: MatDialog,
        public customeraircraftsService: CustomeraircraftsService,
        public customerMarginsService: CustomermarginsService
    ) {}

    ngOnInit() {
        this.loadCustomerAircraftFullList();
        this.pricingTemplatesDataSource = new MatTableDataSource(this.pricingTemplatesData);
        if (!this.customersData)
            return;
        this.refreshCustomerDataSource();
        this.selectAll = false;

        if (sessionStorage.getItem('isCustomerEdit')) {
            if (sessionStorage.getItem('isCustomerEdit') == '1') {
                if (sessionStorage.getItem('pageIndex')) {
                    this.paginator.pageIndex = sessionStorage.getItem('pageIndex') as any;
                    sessionStorage.removeItem('pageIndex');
                    sessionStorage.removeItem('isCustomerEdit');
                }
                else {
                    this.paginator.pageIndex = 0;
                }
            }
        }
        else {
            this.paginator.pageIndex = 0;
        }
    }

    onPageChanged(e) {
        sessionStorage.setItem('pageIndex', e.pageIndex);
    }

    //Public Methods
    public deleteCustomer(customer) {
        const dialogRef = this.deleteCustomerDialog.open(DeleteConfirmationComponent, {
            data: { item: customer, description: 'customer' }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (!result)
                return;
            this.customerInfoByGroupService.remove({ oid: result.item.customerInfoByGroupId}).subscribe((data: any) => {
                this.customerDeleted.emit();
            });
        });
    }

    public editCustomer(customer, $event) {
        if ($event.srcElement) {
            if ($event.srcElement.nodeName.toLowerCase() == 'button' || $event.srcElement.nodeName.toLowerCase() == 'select' || ($event.srcElement.nodeName.toLowerCase() == 'input' && $event.srcElement.getAttribute('type') == 'checkbox')) {
                $event.stopPropagation();
                return;
            }
        }

        const clonedCustomer = Object.assign({}, customer);
        this.editCustomerClicked.emit(clonedCustomer);
    }

    public selectAction()
    {
        this.customersData.forEach(fee => {
            fee.selectAll = this.selectAll ? true : false;
        });
        this.selectedRows = this.selectAll ? this.customersData.length: 0;
    }

    public selectUnique() {
        if (this.selectedRows == this.customersData.length) {
            this.selectAll = false;
            this.selectedRows = this.selectedRows - 1;
        }
    }

    public newCustomer() {
        var customerInfo = { oid: 0 };
        const dialogRef = this.newCustomerDialog.open(CustomersDialogNewCustomerComponent, {
            data: customerInfo
        });

        dialogRef.afterClosed().subscribe(result => {
            if (!result) return;
            this.customersService.add(result).subscribe((data: any) => {
                result.customerId = data.oid;
               
                result.GroupId = this.sharedService.currentUser.groupId;

                this.customersViewedByFboService.add({ fboId: this.sharedService.currentUser.fboId, groupId: this.sharedService.currentUser.groupId, customerId: result.customerId }).subscribe((data:
                    any) => {
                });

                this.customerInfoByGroupService.add(result).subscribe((customerInfoByGroupData: any) => {
                    result.customerInfoByGroupId = customerInfoByGroupData.oid;
                    this.editCustomer(result,Event);
                });
            });
        });
    }

    public applyFilter(filterValue: string) {
        this.customersDataSource.filter = filterValue.trim().toLowerCase();
    }

    public exportCustomersToExcel() {
        //Export the filtered results to an excel spreadsheet
        let exportData = _.clone(this.customersDataSource.filteredData);
        exportData = _.map(exportData, item => {
            return {
                Company: item.company,
                Source: item.customerCompanyTypeName == 'FuelerLinx' ? 'FBOLinx Network' : item.customerCompanyTypeName,
                'Assigned Price Tier': item.pricingTemplateName,
                Price: item.allInPrice
            }
        });
        exportData = _.sortBy(exportData, [
            item => {
                return item.Company.toLowerCase();
            }
        ]);
        const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData);//converts a DOM TABLE element to a worksheet
        const wb: XLSX.WorkBook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, 'Customers');

        /* save to file */
        XLSX.writeFile(wb, 'Customers.xlsx');

    }

    public exportCustomerAircraftToExcel() {
        //Export the filtered results to an excel spreadsheet
        let exportData = _.clone(this.allCustomerAircraft);
        exportData = _.map(exportData, item => {
            return {
                Company: item.company,
                Tail: item.tailNumber,
                Make: item.make,
                Model: item.model,
                Size: item.aircraftSizeDescription,
                'Margin Template': item.pricingTemplateName
            };
        });
        exportData = _.sortBy(exportData, [
            item => {
                return item.Company.toLowerCase();
            },
            item => {
                return item.Tail.toLowerCase();
            }
        ]);
        const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData);//converts a DOM TABLE element to a worksheet
        const wb: XLSX.WorkBook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, 'Aircraft');

        /* save to file */
        XLSX.writeFile(wb, 'Aircraft.xlsx');

    }

    public customerFilterTypeChanged(event) {
        this.refreshCustomerDataSource();
    }

    //Private Methods
    private loadCustomerAircraftFullList() {
        this.customeraircraftsService.getCustomerAircraftsByGroup(this.sharedService.currentUser.groupId).subscribe((data: any) => {
            this.allCustomerAircraft = data;
        });
    }

    private refreshCustomerDataSource() {
        this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
        this.customersData.forEach(c => {
            c.needsAttention = c.pricingTemplateName === 'Default Template' ? true : false;
        });
        this.customersDataSource = new MatTableDataSource(this.customersData.filter((element: any, index: number, array: any[]) => {
            if (this.customerFilterType == 0)
                return true;
            return !element.needsAttention;
        }));
        this.customersDataSource.sort = this.sort;
        this.customersDataSource.paginator = this.paginator;
        this.resultsLength = this.customersData.length;
    }

    private onMarginChange(newValue, customer) {
        const changedPricingTemplate = _.find(customer.pricingTemplatesList, p => {
            return customer.pricingTemplateName && customer.pricingTemplateName == p.name;
        });
        let filteredList = this.customersData.filter(function (item) {
            return item.selectAll == true
        });

        if (filteredList.length > 0) {

            var listCustomers= [];

            filteredList.forEach(selectedItem => {
                
                selectedItem.pricingTemplateName = newValue;
                selectedItem.allInPrice = changedPricingTemplate ? changedPricingTemplate.intoPlanePrice : null;
                
                let vm = {
                    id: selectedItem.customerId,
                    customerMarginName: newValue,
                    fboid: this.sharedService.currentUser.fboId
                }

                listCustomers.push(vm);
            });

            if (listCustomers.length > 0) {
                this.customerMarginsService.updatemultiplecustomermargin(listCustomers).subscribe((data: any) => {
                    this.refreshCustomerDataSource();
                });
            }
        }
        else {
            customer.allInPrice = changedPricingTemplate ? changedPricingTemplate.intoPlanePrice : null;
            let vm = {
                id: customer.customerId,
                customerMarginName: newValue,
                fboid: this.sharedService.currentUser.fboId
            }
            this.customerMarginsService.updatecustomermargin(vm).subscribe((data: any) => {
                this.refreshCustomerDataSource();
            });
        }
    }
}
