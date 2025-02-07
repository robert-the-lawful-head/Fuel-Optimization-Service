import {
    Component,
    ElementRef,
    EventEmitter,
    Input,
    OnInit,
    Output,
    SimpleChanges,
    ViewChild,
} from '@angular/core';
import { MatLegacyDialog as MatDialog } from '@angular/material/legacy-dialog';
import { MatLegacyPaginator as MatPaginator } from '@angular/material/legacy-paginator';
import { MatLegacySelectChange as MatSelectChange } from '@angular/material/legacy-select';
import { MatSort, SortDirection } from '@angular/material/sort';
import { ActivatedRoute } from '@angular/router';
import { find, forEach, sortBy } from 'lodash';
import { MultiSelect } from 'primeng/multiselect';
import { Subscription } from 'rxjs';
import { csvFileOptions, GridBase } from 'src/app/services/tables/GridBase';
import { TagsService } from 'src/app/services/tags.service';

import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvents from '../../../constants/sharedEvents';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { CustomermarginsService } from '../../../services/customermargins.service';
// Services
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
import { CallbackComponent } from 'src/app/shared/components/favorite-icon/favorite-icon.component';
import { CurrencyPresicionPipe } from 'src/app/shared/pipes/decimal/currencyPresicion.pipe';
import { CloseConfirmationComponent } from '../../../shared/components/close-confirmation/close-confirmation.component';
import { ContactinfobyfboService } from '../../../services/contactinfobyfbo.service';

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
        id: 'delete',
        name: 'Actions',
    },
    {
        id: 'invoiceEmail',
        name: 'Invoice Email',
    }
];

@Component({
    selector: 'app-customers-grid',
    styleUrls: ['./customers-grid.component.scss'],
    templateUrl: './customers-grid.component.html',
})
export class CustomersGridComponent extends GridBase implements OnInit {
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
    @Output() exportAircraftClick = new EventEmitter<any>();
    @Output() refreshAircrafts = new EventEmitter<void>();
    @Output() onCompanyFilterApplied = new EventEmitter<string[]>();

    // Members
    tableLocalStorageKey = 'customer-manager-table-settings';

    customerFilterType: number = 0;
    selectAll = false;
    selectedRows: number;
    columns: ColumnType[] = [];
    airportWatchStartDate: Date = new Date();
    needsAttentionOptions: any[];

    /*LICENSE_KEY = '9eef62bd-4c20-452c-98fd-aa781f5ac111';*/
    dialogOpen : boolean = false;
    results = '[]';

    feesAndTaxes: any[];
    focusedCustomer: any;

    feesAndTaxesSubscription: Subscription;

    /*private importer: FlatFileImporter;*/
    customersCsvOptions: csvFileOptions = { fileName: 'Customers', sheetName: 'Customers' };
    aircraftCsvOptions: csvFileOptions = { fileName: 'Aircraft', sheetName: 'Aircraft' };

    start: number = 0;
    limit: number = 20;
    end: number = this.limit + this.start;

    sortChangeSubscription: Subscription;
    constructor(
        private newCustomerDialog: MatDialog,
        private deleteCustomerDialog: MatDialog,
        private tableSettingsDialog: MatDialog,
        private sharedService: SharedService,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private customerMarginsService: CustomermarginsService,
        private fboFeesAndTaxesService: FbofeesandtaxesService,
        private tagsService: TagsService,
        private dialog: MatDialog ,
        private route : ActivatedRoute,
        private currencyPresicion: CurrencyPresicionPipe,
        public closeConfirmationDialog: MatDialog,
        public contactInfoByFboService: ContactinfobyfboService
    ) { super(); }
    ngOnChanges(changes: SimpleChanges): void {
        if(changes.customersData){
            //this.addContactToCustomerDataList();
            this.setVirtualScrollVariables(this.paginator, this.sort, this.customersData);
            this.refreshCustomerDataSource();
        }
    }
    ngOnInit() {
        /*this.initializeImporter();*/
        if (this.customerGridState.filterType) {
            this.customerFilterType = this.customerGridState.filterType;
        }

        this.columns = this.getClientSavedColumns(this.tableLocalStorageKey, initialColumns);

        if (this.customerGridState.filter) {
            this.dataSource.filterCollection = JSON.parse(
                this.customerGridState.filter
            );
        }
        //if (this.customerGridState.order) {
        //    this.sort.active = this.customerGridState.order;
        //}
        //if (this.customerGridState.orderBy) {
        //    this.sort.direction = this.customerGridState
        //        .orderBy as SortDirection;
        //}
        this.airportWatchStartDate = new Date("10/6/2022");

        var needsAttentionOptionsList = ['Email Required', 'Setup Required', 'Top Customer']
        this.needsAttentionOptions = needsAttentionOptionsList.map((nl) => ({
            label: nl,
            value: nl,
        }));
    }
    ngOnDestroy() {
        this.sortChangeSubscription?.unsubscribe();
    }
    // Methods
    addContactToCustomerDataList(){
        this.customersData.forEach(c => {
            c['email'] = c.contacts.map(x => x.email).join(' ');
            c['firstName'] = c.contacts.map(x => x.firstName).join(' ');
            c['lastName'] = c.contacts.map(x => x.lastName).join(' ');
        });
    }
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
            filter: this.dataSource.filter,
            filterType: this.customerFilterType,
            order: this.dataSource?.sort?.active,
            orderBy: this.dataSource?.sort?.direction,
            page: this.dataSource.paginator?.pageIndex,
        });
    }

    selectAction() {
        forEach( this.dataSource.filteredData, (customer) => {
            customer.selectAll = this.selectAll;
        });
        this.selectedRows = this.selectAll ? this.dataSource.data.length : 0;
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

    exportCustomersToExcel(exportSelectedCustomers: boolean = false) {
        let computePropertyFnc = (item: any[], id: string): any => {
            if(id == "allInPrice")
                return this.getAllIPriceDisplayString(item);
            else if(id == "isFuelerLinxCustomer")
                return this.getIsInNetworkDisplayString(item);
            else if(id == "tags")
                return  item[id].map(x => x.name).join(', ');
            else if(id == "fuelVendors")
                return  item[id].map(x => x.label).join(', ');
            else if(id == "needsAttention"){
                return this.getNeedsAttentionDisplayString(item);
            }
            else
                return null;
        }
        this.exportCsvFile(this.columns,this.customersCsvOptions.fileName,this.customersCsvOptions.sheetName,computePropertyFnc,exportSelectedCustomers);
    }
    getNeedsAttentionDisplayString(customer: any): any{
        let message = '';
        if(customer.needsAttention){
            message = 'Needs Attention';
        }
        if(!customer.isFuelerLinxCustomer && !customer.contactExists){
            message = message+' '+'This customer does not have any contacts setup to receive price distribution.';
        }
        return message;
    }
    getAllIPriceDisplayString(customer: any): any{
        return customer.allInPrice > 0 ? this.currencyPresicion.transform(customer.allInPrice) : customer.isPricingExpired == true ? "Expired" : "N/A";
    }
    getIsInNetworkDisplayString(customer: any): any{
        return customer.isFuelerLinxCustomer ? "YES" : "NO"
    }
    exportCustomerAircraftToExcel() {
        this.exportAircraftClick.emit();
    }

    customerFilterTypeChanged() {
        this.refreshCustomerDataSource();
    }

    onMarginChange(changedPricingTemplateId: any, customer: any) {
        let vm = {
            fboid: this.sharedService.currentUser.fboId,
            id: customer.customerId,
            pricingTemplateId: 0,
            userId: this.sharedService.currentUser.oid
        };

        if (changedPricingTemplateId > 0) {
            const changedPricingTemplate = find(
                this.pricingTemplatesData,
                (p) => p.oid === parseInt(changedPricingTemplateId, 10)
            );

            //customer.needsAttention = changedPricingTemplate.default;

            //if (customer.needsAttention) {
            //    customer.needsAttentionReason =
            //        'Customer was assigned to the default template and has not been changed yet.';
            //}

            customer.pricingFormula = changedPricingTemplate.pricingFormula;
            customer.allInPrice = changedPricingTemplate.allInPrice;
            customer.isPricingExpired = true;
            customer.customerActionStatusSetupRequired = false;
            customer.ToolTipSetupRequired = "";
            
            const id = this.route.snapshot.paramMap.get('id');

            vm.pricingTemplateId = changedPricingTemplate.oid;
        }
        else {
            customer.pricingFormula = "NULL";
            customer.allInPrice = 0;
            customer.isPricingExpired = false;
            customer.customerActionStatusSetupRequired = true;
            customer.ToolTipSetupRequired = "This customer was added and needs to be setup with an appropriate ITP template and/or contact email address.";
        }

        this.customerMarginsService.updatecustomermargin(vm).subscribe((data: number) => {
            this.sharedService.emitChange(SharedEvents.customerUpdatedEvent);

            if (data > 0 && !customer.isFuelerLinxCustomer) {
                const closeDialogRef = this.closeConfirmationDialog.open(
                    CloseConfirmationComponent,
                    {
                        autoFocus: false,
                        data: {
                            cancel: 'No',
                            customText: 'Would you like to enable email distribution for all contacts within this flight dept?',
                            customTitle: 'Enable Email Distribution?',
                            ok: 'Yes',
                        },
                    }
                );
                closeDialogRef.afterClosed().subscribe((result) => {
                    if (result === true) {
                        this.contactInfoByFboService.updateDistributionForAllCustomerContacts(customer.customerId, this.sharedService.currentUser.fboId, true).subscribe(() => {

                        });
                    }
                    else {
                        this.contactInfoByFboService.updateDistributionForAllCustomerContacts(customer.customerId, this.sharedService.currentUser.fboId, false).subscribe(() => {

                        });
                    }
                });
            }

        });
    }

    bulkMarginTemplateUpdate(event: MatSelectChange) {
        const listCustomers = [];
        forEach(this.dataSource.filteredData, (customer) => {
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
                this.refreshSort(this.sort, this.columns);
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
            filter: this.dataSource.filter,
            filterType: this.customerFilterType,
            order: this.dataSource?.sort?.active,
            orderBy: this.dataSource?.sort?.direction,
            page: this.dataSource.paginator?.pageIndex,
            pricingTemplateId: customer.pricingTemplateId
        });
    }

    private refreshCustomerDataSource() {
        this.sortChangeSubscription = this.sort.sortChange.subscribe(() => {
            this.columns = this.columns.map((column) =>
                column.id === this.sort.active
                    ? { ...column, sort: this.sort.direction }
                    : {
                        hidden: column.hidden,
                        id: column.id,
                        name: column.name,
                    }
            );
            this.saveSettings();
        });
        this.dataSource.data = this.customersData.filter(
            (element: any) => {
               if (this.customerFilterType != 1) {
                   return true;
                }
                //return element.needsAttention;

            }
        );
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
        if(filteredDataSource.filter.length == 2){
            this.refreshCustomerDataSource();
            return;
        }
        this.dataSource = filteredDataSource;

        this.start = 0;
        this.limit = 20;
    }
    setIsFavoriteProperty(customer: any): any {
        customer.isFavorite = customer.favoriteCompany != null;
        return customer;
    }
    toogleFavorite(favoriteData: any): void {
        if(favoriteData.isFavorite)
            this.refreshAircrafts.emit();
    }
    onFilterApplied(filteredDataSource: any): void {
        let customerFilteredTailNumbers = filteredDataSource.filteredData.map(x => x.company.toLowerCase());
        
        this.onCompanyFilterApplied.emit(customerFilteredTailNumbers);
    }
    get getCallBackComponent(): CallbackComponent{
        return CallbackComponent.Company;
    }
}
