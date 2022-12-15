import {
    Component,
    ElementRef,
    EventEmitter,
    Input,
    OnInit,
    Output,
    ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSelectChange } from '@angular/material/select';
import { MatSort, MatSortHeader, SortDirection } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
/*import FlatFileImporter from 'flatfile-csv-importer';*/
import { find, forEach, map, sortBy } from 'lodash';
import { MultiSelect } from 'primeng/multiselect';
import { Subscription } from 'rxjs';
import { VirtualScrollBase } from 'src/app/services/tables/VirtualScrollBase';
import { TagsService } from 'src/app/services/tags.service';
import * as XLSX from 'xlsx';

import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvents from '../../../models/sharedEvents';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { CustomermarginsService } from '../../../services/customermargins.service';
// Services
import { CustomersService } from '../../../services/customers.service';
import { FbofeesandtaxesService } from '../../../services/fbofeesandtaxes.service';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';
import { PriceBreakdownComponent } from '../../../shared/components/price-breakdown/price-breakdown.component';
import {
    ColumnType,
    TableSettingsComponent,
} from '../../../shared/components/table-settings/table-settings.component';
import { CustomerGridState } from '../../../store/reducers/customer';
import { CustomerTagDialogComponent } from '../customer-tag-dialog/customer-tag-dialog.component';
// Components
import { CustomersDialogNewCustomerComponent } from '../customers-dialog-new-customer/customers-dialog-new-customer.component';

const initialColumns: ColumnType[] = [
    {
        id: 'selectAll',
        name: 'Select All',
    },
    {
        id: 'company',
        name: 'Company',
        sort: 'asc',
    },
    {
        id: 'needsAttention',
        name: 'Needs Attention',
    },
    {
        id: 'pricingTemplateName',
        name: 'ITP Margin Template',
    },
    {
        id: 'pricingFormula',
        name: 'PricingFormula',
    },
    {
        id: 'allInPrice',
        name: 'All In Price',
    },
    {
        id: 'tags',
        name: 'Tags',
    },
    {
        id: 'isFuelerLinxCustomer',
        name: 'FuelerLinx Network',
    },
    {
        id: 'fleetSize',
        name: 'Fleet Size',
    },
    {
        id: 'fuelVendors',
        name: 'Fuel Vendors',
    },
    {
        id: 'certificateTypeDescription',
        name: 'Certificate Type',
    },
    {
        id: 'customerCompanyTypeName',
        name: 'Customer Type',
    },

    //{
    //    id: 'aircraftsVisits',
    //    name: 'Previous Visits',
    //},
    {
        id: 'delete',
        name: 'Actions',
    },
];

@Component({
    selector: 'app-customers-grid',
    styleUrls: ['./customers-grid.component.scss'],
    templateUrl: './customers-grid.component.html',
})
export class CustomersGridComponent extends VirtualScrollBase implements OnInit {
    @ViewChild('priceBreakdownPreview')
    priceBreakdownPreview: PriceBreakdownComponent;
    @ViewChild('customerTableContainer') table: ElementRef;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild("multiInput") multiInput: MultiSelect;
    // Input/Output Bindings
    @Input() customersData: any[];
    @Input() pricingTemplatesData: any[];
    @Input() aircraftData: any[];
    @Input() customerGridState: CustomerGridState;
    @Input() fuelVendors: any[];
    @Input () tags : any [];

    @Output() editCustomerClicked = new EventEmitter<any>();
    @Output() customerDeleted = new EventEmitter<any>();
    @Output() customerPriceClicked = new EventEmitter<any>();

    // Members
    tableLocalStorageKey = 'customer-manager-table-settings';

    customersDataSource: any = null;
    customersTableDataSource: MatTableDataSource<any> = new MatTableDataSource();

    customerFilterType: number = 0;
    selectAll = false;
    selectedRows: number;
    pageIndex = 0;
    pageSize = 100;
    columns: ColumnType[] = [];
    airportWatchStartDate: Date = new Date();

    /*LICENSE_KEY = '9eef62bd-4c20-452c-98fd-aa781f5ac111';*/
    dialogOpen : boolean = false;
    results = '[]';

    feesAndTaxes: any[];
    focusedCustomer: any;

    feesAndTaxesSubscription: Subscription;

    /*private importer: FlatFileImporter;*/

    start: number = 0;
    limit: number = 20;
    end: number = this.limit + this.start;

    constructor(
        private newCustomerDialog: MatDialog,
        private deleteCustomerDialog: MatDialog,
        private tableSettingsDialog: MatDialog,
        private customersService: CustomersService,
        private sharedService: SharedService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private customerMarginsService: CustomermarginsService,
        private fboFeesAndTaxesService: FbofeesandtaxesService,
        private tagsService: TagsService,
        private dialog: MatDialog ,
        private route : ActivatedRoute
    ) { super(); }

    ngOnInit() {
        /*this.initializeImporter();*/
        if (this.customerGridState.filterType) {
            this.customerFilterType = this.customerGridState.filterType;
        }

        if (localStorage.getItem(this.tableLocalStorageKey)) {
            this.columns = JSON.parse(
                localStorage.getItem(this.tableLocalStorageKey)
            );
            if (this.columns.length !== initialColumns.length) {
                this.columns = initialColumns;
            }
        } else {
            this.columns = initialColumns;
        }

        this.refreshCustomerDataSource();

        if (this.customerGridState.filter) {
            this.customersDataSource.filterCollection = JSON.parse(
                this.customerGridState.filter
            );
        }
        // if (this.customerGridState.page) {
        //     this.paginator?.pageIndex = this.customerGridState.page;
        // }
        if (this.customerGridState.order) {
            this.sort.active = this.customerGridState.order;
        }
        if (this.customerGridState.orderBy) {
            this.sort.direction = this.customerGridState
                .orderBy as SortDirection;
        }
        //this.airportWatchService.getStartDate().subscribe((date) => {
        //this.airportWatchStartDate = new Date(date);
        //});
        this.airportWatchStartDate = new Date("10/6/2022");
    }

    onPageChanged(event: any) {
        localStorage.setItem('pageIndex', event.pageIndex);
        sessionStorage.setItem(
            'pageSizeValue',
            this.paginator.pageSize.toString()
        );
        this.selectAll = false;
        forEach(this.customersData, (customer) => {
            customer.selectAll = false;
        });
    }

    // Methods
    deleteCustomer(customer) {
        const dialogRef = this.deleteCustomerDialog.open(
            DeleteConfirmationComponent,
            {
                autoFocus: false,
                data: { description: 'customer', item: customer },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            this.customerInfoByGroupService
                .remove({ oid: result.item.customerInfoByGroupId } )
                .subscribe(() => {
                    this.customerDeleted.emit();
                });
        });
    }

    editCustomer(customer) {

        this.editCustomerClicked.emit({
            customerInfoByGroupId: customer.customerInfoByGroupId,
            filter: this.customersDataSource.filter,
            filterType: this.customerFilterType,
            order: this.customersDataSource.sort.active,
            orderBy: this.customersDataSource.sort.direction,
            page: this.customersDataSource.paginator?.pageIndex,
        });
    }

    selectAction() {
        const pageCustomersData = this.customersDataSource.connect().value;
        forEach(pageCustomersData, (customer) => {
            customer.selectAll = this.selectAll;
        });
        this.selectedRows = this.selectAll ? pageCustomersData.length : 0;
    }

    selectUnique() {
        if (this.selectedRows === this.customersData.length) {
            this.selectAll = false;
            this.selectedRows = this.selectedRows - 1;
        }
    }

    newCustomer() {
        const dialogRef = this.newCustomerDialog.open(
            CustomersDialogNewCustomerComponent,
            {
                height: '500px',
                width: '1140px',
            }
        );

        dialogRef.afterClosed().subscribe((customerInfoByGroupId) => {
            if (!customerInfoByGroupId) {
                return;
            }
            this.editCustomer({
                customerInfoByGroupId,
            });
        });
    }

    exportCustomersToExcel() {
        // Export the filtered results to an excel spreadsheet
        const filteredList = this.customersDataSource.filteredData.filter(
            (item) => item.selectAll === true
        );
        let exportData;
        if (filteredList.length > 0) {
            exportData = filteredList;
        } else {
            exportData = this.customersDataSource.filteredData;
        }
        exportData = map(exportData, (item) => ({
            'Certificate Type': item.certificateTypeDescription,
            Company: item.company,
            'Customer Type': item.customerCompanyTypeName,
            'Fleet Size': item.fleetSize,
            'FuelerLinx Network': item.isFuelerLinxCustomer ? 'YES' : 'NO',
            'ITP Margin Template': item.pricingTemplateName,
            'Needs Attention': item.needsAttention ? 'Needs Attention' : '',
            'Previous Visits': item.aircraftsVisits,
        }));
        exportData = sortBy(exportData, [(item) => item.Company.toLowerCase()]);
        const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData); // converts a DOM TABLE element to a worksheet
        const wb: XLSX.WorkBook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, 'Customers');

        /* save to file */
        XLSX.writeFile(wb, 'Customers.xlsx');
    }

    exportCustomerAircraftToExcel() {
        // Export the filtered results to an excel spreadsheet
        let exportData = map(this.aircraftData, (item) => ({
            Company: item.company,
            Tail: item.tailNumber,
            Type: item.make + ' ' + item.model,
            Size: item.aircraftSizeDescription,
            'Company Pricing': item.pricingTemplateName,
        }));
        exportData = sortBy(exportData, [
            (item) => item.Company.toLowerCase(),
            (item) => item.Tail.toLowerCase(),
        ]);
        const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData); // converts a DOM TABLE element to a worksheet
        const wb: XLSX.WorkBook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, 'Aircraft');

        /* save to file */
        XLSX.writeFile(wb, 'Aircraft.xlsx');
    }

    customerFilterTypeChanged() {
        this.refreshCustomerDataSource();
    }

    onMarginChange(changedPricingTemplateId: any, customer: any) {
        const changedPricingTemplate = find(
            this.pricingTemplatesData,
            (p) => p.oid === parseInt(changedPricingTemplateId, 10)
        );

        customer.needsAttention = changedPricingTemplate.default;

        if (customer.needsAttention) {
            customer.needsAttentionReason =
                'Customer was assigned to the default template and has not been changed yet.';
        }

        customer.pricingFormula = changedPricingTemplate.pricingFormula;
        customer.allInPrice = changedPricingTemplate.allInPrice;

        const vm = {
            fboid: this.sharedService.currentUser.fboId,
            id: customer.customerId,
            pricingTemplateId: changedPricingTemplate.oid,
            userId: this.sharedService.currentUser.oid
        };
        const id = this.route.snapshot.paramMap.get('id');
        this.customerMarginsService.updatecustomermargin(vm).subscribe(() => {
            this.sharedService.emitChange(SharedEvents.customerUpdatedEvent);
        });
    }

    bulkMarginTemplateUpdate(event: MatSelectChange) {
        const listCustomers = [];

        forEach(this.customersData, (customer) => {
            if (customer.selectAll === true) {
                customer.needsAttention = event.value.default;
                customer.pricingTemplateName = event.value.name;
                customer.pricingTemplateId = event.value.oid;
                customer.allInPrice = event.value.allInPrice;
                customer.pricingFormula = event.value.pricingFormula;
                if (customer.needsAttention) {
                    customer.needsAttentionReason =
                        'Customer was assigned to the default template and has not been changed yet.';
                }
                listCustomers.push({
                    fboid: this.sharedService.currentUser.fboId,
                    id: customer.customerId,
                    pricingTemplateId: event.value.oid,
                    userId: this.sharedService.currentUser.oid
                });
            }
        });

        this.customerMarginsService
            .updatemultiplecustomermargin(listCustomers)
            .subscribe(() => {
                this.sharedService.emitChange(
                    SharedEvents.customerUpdatedEvent
                );
            });
    }

    anySelected() {
        const filteredList = this.customersData.filter(
            (item) => item.selectAll === true
        );
        return filteredList.length > 0;
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
    //            results.data.forEach((result) => {
    //                result.groupid = this.sharedService.currentUser.groupId;
    //            });

    //            this.customersService
    //                .importcustomers(results.data)
    //                .subscribe((data: any) => {
    //                    if (data) {
    //                        this.importer.displaySuccess(
    //                            'Data successfully imported!'
    //                        );
    //                        setTimeout(() => {
    //                            this.customerDeleted.emit();
    //                        }, 1500);
    //                    }
    //                });
    //        }
    //    } catch (e) { }
    //}

    //initializeImporter() {
    //    FlatFileImporter.setVersion(2);
    //    this.importer = new FlatFileImporter(this.LICENSE_KEY, {
    //        allowCustom: true,
    //        allowInvalidSubmit: true,
    //        disableManualInput: false,
    //        fields: [
    //            {
    //                alternates: ['Id', 'CompanyId'],
    //                description: 'Company Id Value',
    //                key: 'CompanyId',
    //                label: 'Company Id',
    //            },
    //            {
    //                alternates: ['Company Name', 'Name'],
    //                description: 'Company Name Value',
    //                key: 'CompanyName',
    //                label: 'CompanyName',
    //                validators: [
    //                    {
    //                        error: 'this field is required',
    //                        validate: 'required',
    //                    },
    //                ],
    //            },
    //            {
    //                alternates: ['activate'],
    //                description: 'Activate Flag',
    //                key: 'Activate',
    //                label: 'Activate',
    //            },
    //            {
    //                alternates: ['tail', 'plane tail', 'N-number', 'Nnumber'],
    //                description: 'Tail',
    //                key: 'Tail',
    //                label: 'Tail',
    //            },
    //            {
    //                alternates: [
    //                    'Make',
    //                    'make',
    //                    'aircraft make',
    //                    'aircraft',
    //                    'Manufacturer',
    //                    'Aircraft Manufacturer',
    //                ],
    //                description: 'Aircraft Make',
    //                key: 'AircraftMake',
    //                label: 'Aircraft Make',
    //            },
    //            {
    //                alternates: [
    //                    'Aircraft Model',
    //                    'aircraft model',
    //                    'model',
    //                    'aircraft type',
    //                    'type',
    //                ],
    //                description: 'Aircraft Model',
    //                key: 'AircraftModel',
    //                label: 'Model',
    //            },
    //            {
    //                alternates: ['Aircraft Size', 'Plane Size'],
    //                description: 'Aircraft Size',
    //                key: 'AircraftSize',
    //                label: 'Size',
    //            },
    //            {
    //                alternates: ['first name', 'name'],
    //                description: 'First Name',
    //                key: 'FirstName',
    //                label: 'First Name',
    //            },
    //            {
    //                alternates: ['last name', 'lname'],
    //                description: 'Last Name',
    //                key: 'LastName',
    //                label: 'Last Name',
    //            },
    //            {
    //                alternates: ['title'],
    //                description: 'Title',
    //                key: 'Title',
    //                label: 'Title',
    //            },
    //            {
    //                alternates: ['email address', 'email'],
    //                description: 'Email',
    //                key: 'Email',
    //                label: 'Email',
    //            },
    //            {
    //                alternates: ['mobile', 'cell', 'cell phone'],
    //                description: 'Mobile',
    //                key: 'Mobile',
    //                label: 'Mobile',
    //            },
    //            {
    //                alternates: ['phone'],
    //                description: 'Phone',
    //                key: 'Phone',
    //                label: 'Phone',
    //            },
    //        ],
    //        managed: true,
    //        type: 'Customers',
    //    });
    //    this.importer.setCustomer({
    //        name: 'WebsiteImport',
    //        userId: '1',
    //    });
    //}

    getTableColumns() {
        return this.columns
            .filter((column) => !column.hidden)
            .map((column) => column.id);
    }

    openSettings() {
        const dialogRef = this.tableSettingsDialog.open(
            TableSettingsComponent,
            {
                data: this.columns,
            }
        );
        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.columns = [...result];
                this.refreshSort();
                this.saveSettings();
            }
        });
    }

    saveSettings() {
        localStorage.setItem(
            this.tableLocalStorageKey,
            JSON.stringify(this.columns)
        );
    }

    onFuelVendorUpdate(event: any, customer: any) {
        if (
            customer.fuelVendors.find(
                (fv) => fv.value === event.itemValue.value
            )
        ) {
            customer.fuelVendors = customer.fuelVendors.filter(
                (fv) => fv.value !== event.itemValue.value
            );
        } else {
            customer.fuelVendors = sortBy(
                [...customer.fuelVendors, event.itemValue],
                (fv) => fv.value
            );
        }
    }

    onCustomerTagUpdate(event: any, customer: any) {
        if (event.itemValue.customerId != customer.customerId)
        {
            this.addCustomerTag(event.itemValue, customer);
        }
        else{
            this.removeCustomerTag(event.itemValue, customer);
        }
    }

    onCustomerTagPanelShow(event: any, customer: any){
        this.loadCustomerTags(customer);
    }

    removeCustomerTag(tag, customer) {
        this.tagsService.remove(tag).subscribe(response => {
            this.loadCustomerTags(customer);
        })
    }
    addCustomerTag(tag, customer) {
        tag['groupId'] = this.sharedService.currentUser.groupId;
        tag['customerId'] = customer.customerId;
        tag['oid'] = 0;
        this.tagsService.add(tag).subscribe(response => {
            this.loadCustomerTags(customer);
        });
    }

    loadCustomerTags(customer: any){
        this.tagsService.getTags({
            groupId: this.sharedService.currentUser.groupId,
            customerId: customer.customerId,
            isFuelerLinx: customer.isFuelerLinxCustomer
        }).subscribe(response => {
            customer.availableTags = response;
            customer.tags = customer.availableTags.filter(x=>x.customerId == customer.customerId);
        })
    }

    newCustomTag(customer: any) {
        this.dialogOpen = true;
        const data = {
            customerId: customer.customerId,
            groupId: this.sharedService.currentUser.groupId,
        };
        const dialogRef = this.dialog.open(CustomerTagDialogComponent, {
            data,
        });

        dialogRef.afterClosed().subscribe((response) => {
            this.dialogOpen = false;
            if (!response) {
                return;
            }
            this.tagsService
                .add(response)
                .subscribe((result: any) => {
                    this.loadCustomerTags(customer);
                });
        });
    }

    checkState() {
        if (this.dialogOpen)
            this.multiInput.show();
    }

    onCustomerPriceShown(customer: any) {
        this.focusedCustomer = customer;
        this.feesAndTaxesSubscription = this.fboFeesAndTaxesService
            .getByFboAndCustomer(
                this.sharedService.currentUser.fboId,
                customer.customerId
            )
            .subscribe((response: any[]) => {
                this.feesAndTaxes = response;
            });
    }

    onCustomerPriceHidden() {
        if (this.feesAndTaxesSubscription) {
            this.feesAndTaxesSubscription.unsubscribe();
            this.feesAndTaxesSubscription = null;
        }
        this.feesAndTaxes = null;
        this.focusedCustomer = null;
    }

    onCustomerPriceClicked(customer) {
        this.customerPriceClicked.emit({
            customerInfoByGroupId: customer.customerInfoByGroupId,
            filter: this.customersDataSource.filter,
            filterType: this.customerFilterType,
            order: this.customersDataSource.sort.active,
            orderBy: this.customersDataSource.sort.direction,
            page: this.customersDataSource.paginator?.pageIndex,
            pricingTemplateId: customer.pricingTemplateId
        });
    }

    private refreshCustomerDataSource() {
        this.sort.sortChange.subscribe(() => {
            this.columns = this.columns.map((column) =>
                column.id === this.sort.active
                    ? { ...column, sort: this.sort.direction }
                    : {
                        hidden: column.hidden,
                        id: column.id,
                        name: column.name,
                    }
            );
            // this.paginator?.pageIndex = 0;
            this.saveSettings();
        });
        if (!this.customersDataSource) {
            this.customersDataSource = new MatTableDataSource();
        }
        this.customersDataSource.data = this.customersData.filter(
            (element: any) => {
               if (this.customerFilterType != 1) {
                   return true;
                }
                return element.needsAttention;

            }
        );

        this.sort.active = 'allInPrice';
        this.customersDataSource.sort = this.sort;
        // this.customersDataSource.paginator = this.paginator;

        this.setVirtualScrollVariables();
    }
    setVirtualScrollVariables(){
        this.data = this.customersData;
        this.dataSource.data = this.getTableData(this.start, this.end);
        this.updateIndex();
    }
    private refreshSort() {
        const sortedColumn = this.columns.find(
            (column) => !column.hidden && column.sort
        );
        this.sort.sort({
            disableClear: false,
            id: null,
            start: sortedColumn?.sort || 'asc',
        });
        this.sort.sort({
            disableClear: false,
            id: sortedColumn?.id,
            start: sortedColumn?.sort || 'asc',
        });
        (
            this.sort.sortables.get(sortedColumn?.id) as MatSortHeader
        )?._setAnimationTransitionState({ toState: 'active' });
    }

    private loadCustomerFeesAndTaxes(customerInfoByGroupId: number): void {
        this.fboFeesAndTaxesService
            .getByFboAndCustomer(
                this.sharedService.currentUser.fboId,
                customerInfoByGroupId
            )
            .subscribe((response: any[]) => {
                this.feesAndTaxes = response;
            });
    }
    loadFilteredDataSource(filteredDataSource: any){
        console.log("🚀 ~ file: customers-grid.component.ts:770 ~ CustomersGridComponent ~ loadFilteredDataSource ~ filteredDataSource", filteredDataSource)
        if(filteredDataSource.filter.length == 2){
            this.refreshCustomerDataSource();
            return;
        }
        this.dataSource = filteredDataSource;

        this.start = 0;
        this.limit = 20;
    }
}
