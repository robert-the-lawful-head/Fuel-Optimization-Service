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
import FlatFileImporter from 'flatfile-csv-importer';
import { find, forEach, map, sortBy } from 'lodash';
import * as XLSX from 'xlsx';

import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvents from '../../../models/sharedEvents';
import { AirportWatchService } from '../../../services/airportwatch.service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { CustomermarginsService } from '../../../services/customermargins.service';
// Services
import { CustomersService } from '../../../services/customers.service';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';
import {
    ColumnType,
    TableSettingsComponent,
} from '../../../shared/components/table-settings/table-settings.component';
import { CustomerGridState } from '../../../store/reducers/customer';
// Components
import { CustomersDialogNewCustomerComponent } from '../customers-dialog-new-customer/customers-dialog-new-customer.component';

const initialColumns: ColumnType[] = [
    {
        id: 'selectAll',
        name: 'Select All',
    },
    {
        id: 'needsAttention',
        name: 'Needs Attention',
    },
    {
        id: 'company',
        name: 'Company',
        sort: 'asc',
    },
    {
        id: 'customerCompanyTypeName',
        name: 'Customer Type',
    },
    {
        id: 'isFuelerLinxCustomer',
        name: 'FuelerLinx Network',
    },
    {
        id: 'certificateTypeDescription',
        name: 'Certificate Type',
    },
    {
        id: 'pricingTemplateName',
        name: 'ITP Margin Template',
    },
    {
        id: 'fuelVendors',
        name: 'Fuel Vendors',
    },
    {
        id: 'allInPrice',
        name: 'All In Price',
    },
    {
        id: 'fleetSize',
        name: 'Fleet Size',
    },
    {
        id: 'aircraftsVisits',
        name: 'Previous Visits',
    },
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
export class CustomersGridComponent implements OnInit {
    // Input/Output Bindings
    @Input() customersData: any[];
    @Input() pricingTemplatesData: any[];
    @Input() aircraftData: any[];
    @Input() customerGridState: CustomerGridState;
    @Input() fuelVendors: any[];

    @Output() editCustomerClicked = new EventEmitter<any>();
    @Output() customerDeleted = new EventEmitter<any>();

    // Members
    @ViewChild('customerTableContainer') table: ElementRef;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    tableLocalStorageKey = 'customer-manager-table-settings';

    customersDataSource: any = null;
    customerFilterType: number = undefined;
    selectAll = false;
    selectedRows: number;
    pageIndex = 0;
    pageSize = 100;
    columns: ColumnType[] = [];
    airportWatchStartDate: Date = new Date();

    LICENSE_KEY = '9eef62bd-4c20-452c-98fd-aa781f5ac111';

    results = '[]';

    private importer: FlatFileImporter;

    constructor(
        private newCustomerDialog: MatDialog,
        private deleteCustomerDialog: MatDialog,
        private tableSettingsDialog: MatDialog,
        private customersService: CustomersService,
        private sharedService: SharedService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private customerMarginsService: CustomermarginsService,
        private airportWatchService: AirportWatchService
    ) {}

    ngOnInit() {
        this.initializeImporter();

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
        if (this.customerGridState.page) {
            this.paginator.pageIndex = this.customerGridState.page;
        }
        if (this.customerGridState.order) {
            this.sort.active = this.customerGridState.order;
        }
        if (this.customerGridState.orderBy) {
            this.sort.direction = this.customerGridState
                .orderBy as SortDirection;
        }
        this.airportWatchService.getStartDate().subscribe((date) => {
            this.airportWatchStartDate = new Date(date);
        });
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
                .remove({ oid: result.item.customerInfoByGroupId })
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
            page: this.customersDataSource.paginator.pageIndex,
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
            'Company Pricing': item.pricingTemplateName,
            Size: item.aircraftSizeDescription,
            Tail: item.tailNumber,
            Type: item.make + ' ' + item.model,
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
        customer.allInPrice = changedPricingTemplate.intoPlanePrice;

        if (customer.needsAttention) {
            customer.needsAttentionReason =
                'Customer was assigned to the default template and has not been changed yet.';
        }

        const vm = {
            fboid: this.sharedService.currentUser.fboId,
            id: customer.customerId,
            pricingTemplateId: changedPricingTemplate.oid,
        };
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
                customer.allInPrice = event.value.intoPlanePrice;
                if (customer.needsAttention) {
                    customer.needsAttentionReason =
                        'Customer was assigned to the default template and has not been changed yet.';
                }
                listCustomers.push({
                    fboid: this.sharedService.currentUser.fboId,
                    id: customer.customerId,
                    pricingTemplateId: event.value.oid,
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

    async launchImporter() {
        if (!this.LICENSE_KEY) {
            return alert('Set LICENSE_KEY on Line 13 before continuing.');
        }
        try {
            const results = await this.importer.requestDataFromUser();
            this.importer.displayLoader();

            if (results) {
                results.data.forEach((result) => {
                    result.groupid = this.sharedService.currentUser.groupId;
                });

                this.customersService
                    .importcustomers(results.data)
                    .subscribe((data: any) => {
                        if (data) {
                            this.importer.displaySuccess(
                                'Data successfully imported!'
                            );
                            setTimeout(() => {
                                this.customerDeleted.emit();
                            }, 1500);
                        }
                    });
            }
        } catch (e) {}
    }

    initializeImporter() {
        FlatFileImporter.setVersion(2);
        this.importer = new FlatFileImporter(this.LICENSE_KEY, {
            allowCustom: true,
            allowInvalidSubmit: true,
            disableManualInput: false,
            fields: [
                {
                    alternates: ['Id', 'CompanyId'],
                    description: 'Company Id Value',
                    key: 'CompanyId',
                    label: 'Company Id',
                },
                {
                    alternates: ['Company Name', 'Name'],
                    description: 'Company Name Value',
                    key: 'CompanyName',
                    label: 'CompanyName',
                    validators: [
                        {
                            error: 'this field is required',
                            validate: 'required',
                        },
                    ],
                },
                {
                    alternates: ['activate'],
                    description: 'Activate Flag',
                    key: 'Activate',
                    label: 'Activate',
                },
                {
                    alternates: ['tail', 'plane tail', 'N-number', 'Nnumber'],
                    description: 'Tail',
                    key: 'Tail',
                    label: 'Tail',
                },
                {
                    alternates: [
                        'Make',
                        'make',
                        'aircraft make',
                        'aircraft',
                        'Manufacturer',
                        'Aircraft Manufacturer',
                    ],
                    description: 'Aircraft Make',
                    key: 'AircraftMake',
                    label: 'Aircraft Make',
                },
                {
                    alternates: [
                        'Aircraft Model',
                        'aircraft model',
                        'model',
                        'aircraft type',
                        'type',
                    ],
                    description: 'Aircraft Model',
                    key: 'AircraftModel',
                    label: 'Model',
                },
                {
                    alternates: ['Aircraft Size', 'Plane Size'],
                    description: 'Aircraft Size',
                    key: 'AircraftSize',
                    label: 'Size',
                },
                {
                    alternates: ['first name', 'name'],
                    description: 'First Name',
                    key: 'FirstName',
                    label: 'First Name',
                },
                {
                    alternates: ['last name', 'lname'],
                    description: 'Last Name',
                    key: 'LastName',
                    label: 'Last Name',
                },
                {
                    alternates: ['title'],
                    description: 'Title',
                    key: 'Title',
                    label: 'Title',
                },
                {
                    alternates: ['email address', 'email'],
                    description: 'Email',
                    key: 'Email',
                    label: 'Email',
                },
                {
                    alternates: ['mobile', 'cell', 'cell phone'],
                    description: 'Mobile',
                    key: 'Mobile',
                    label: 'Mobile',
                },
                {
                    alternates: ['phone'],
                    description: 'Phone',
                    key: 'Phone',
                    label: 'Phone',
                },
            ],
            managed: true,
            type: 'Customers',
        });
        this.importer.setCustomer({
            name: 'WebsiteImport',
            userId: '1',
        });
    }

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
            this.paginator.pageIndex = 0;
            this.saveSettings();
        });
        if (!this.customersDataSource) {
            this.customersDataSource = new MatTableDataSource();
        }
        this.customersDataSource.data = this.customersData.filter(
            (element: any) => {
                if (this.customerFilterType !== 1) {
                    return true;
                }
                return element.needsAttention;
            }
        );
        this.sort.active = 'allInPrice';
        this.customersDataSource.sort = this.sort;
        this.customersDataSource.paginator = this.paginator;
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
}
