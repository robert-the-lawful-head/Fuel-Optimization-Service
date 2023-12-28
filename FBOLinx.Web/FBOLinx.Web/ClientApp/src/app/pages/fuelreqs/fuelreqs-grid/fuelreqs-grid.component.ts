import { trigger, state, style } from '@angular/animations';
import { CurrencyPipe, DatePipe } from '@angular/common';
import {
    ChangeDetectionStrategy,
    Component,
    ElementRef,
    EventEmitter,
    Input,
    OnChanges,
    OnInit,
    Output,
    QueryList,
    SimpleChanges,
    ViewChild,
    ViewChildren,
} from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, MatSortHeader } from '@angular/material/sort';
import { MatRow, MatTableDataSource } from '@angular/material/table';
import { isEqual } from 'lodash';
import { csvFileOptions, GridBase } from 'src/app/services/tables/GridBase';

// Services
import { SharedService } from '../../../layouts/shared-service';
// Shared components
import {
    ColumnType,
    TableSettingsComponent,
} from '../../../shared/components/table-settings/table-settings.component';
import { ServiceOrdersDialogOrderItemsComponent } from '../../service-orders/service-orders-dialog-order-items/service-orders-dialog-order-items.component';
import { FuelreqsService } from 'src/app/services/fuelreqs.service';
import { SnackBarService } from 'src/app/services/utils/snackBar.service';
import { ServiceOrder } from '../../../models/service-order';
import { ServiceOrdersDialogNewComponent } from '../../service-orders/service-orders-dialog-new/service-orders-dialog-new.component';
import { ServiceOrderAppliedDateTypes } from '../../../enums/service-order-applied-date-types';
import { FuelReq } from '../../../models/fuelreq';
import { ServiceOrderService } from '../../../services/serviceorder.service';
import { EntityResponseMessage } from '../../../models/entity-response-message';
import { ProceedConfirmationComponent } from '../../../shared/components/proceed-confirmation/proceed-confirmation.component';
import { FuelreqsGridServicesComponent } from '../fuelreqs-grid-services/fuelreqs-grid-services.component';
import { ServiceOrderItem } from '../../../models/service-order-item';
import * as moment from 'moment';

const initialColumns: ColumnType[] = [
    {
        id: 'archivedCheckbox',
        name: 'Archived',
    },
    {
        id: 'tailNumber',
        name: 'Tail #',
    },
    {
        id: 'customerName',
        name: 'Flight Dept.',
    },
    {
        id: 'eta',
        name: 'ETA',
        sort: 'desc',
    },
    {
        id: 'paymentMethod',
        name: 'Payment Method',
    },

    {
        id: 'quotedPpg',
        name: 'PPG',
    },
    {
        id: 'etd',
        name: 'ETD',
    },

    //SERVICES COMPLETED
    {
        id: 'services',
        name: 'Services',
    },
    {
        id: 'source',
        name: 'Source',
    },
    {
        id: 'email',
        name: 'Email',
    },
    {
        id: 'oid',
        name: 'ID',
        hidden: true
    },
    {
        id: 'fuelerlinxid',
        name: 'FuelerLinx ID',
        hidden: true
    },
    {
        id: 'created',
        name: 'Created',
        hidden: true
    }
];

@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-fuelreqs-grid',
    styleUrls: ['./fuelreqs-grid.component.scss'],
    templateUrl: './fuelreqs-grid.component.html',
    animations: [
        trigger('detailExpand', [
            state('collapsed, void', style({ height: '0px', minHeight: '0', display: 'none' })),
            state('expanded', style({ height: '*' }))
          ])
    ]
})
export class FuelreqsGridComponent extends GridBase implements OnInit, OnChanges {
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(FuelreqsGridServicesComponent, { static: true }) fuelReqGridServicesComponent: FuelreqsGridServicesComponent;
    @ViewChildren(MatRow, { read: ElementRef }) rows!: QueryList<ElementRef<HTMLTableRowElement>>;
    @Output() dateFilterChanged = new EventEmitter<any>();
    @Input() fuelreqsData: any[];
    @Input() filterStartDate: Date;
    @Input() filterEndDate: Date;
    @Input() servicesAndFees: string[];
    @Output() onArchivedChange = new EventEmitter<any>();

    tableLocalStorageKey = 'fuel-req-table-settings';

    fuelreqsDataSource: MatTableDataSource<any> = null;
    resultsLength = 0;
    columns: ColumnType[] = [];

    dashboardSettings: any;

    csvFileOptions: csvFileOptions = { fileName: 'FuelOrders', sheetName: 'Fuel Orders' };

    allColumnsToDisplay: string[];

    expandedElement: any[] = [];

    isConfirmedLoadingDictionary: { [key: string]: boolean; } = {};

    constructor(
        private sharedService: SharedService,
        private tableSettingsDialog: MatDialog,
        private datePipe: DatePipe,
        private currencyPipe: CurrencyPipe,
        private fuelreqsService: FuelreqsService,
        private snackBarService: SnackBarService,
        private newServiceOrderDialog: MatDialog,
        private serviceOrderService: ServiceOrderService,
        private templateDialog: MatDialog
    ) {
        super();
        this.dashboardSettings = this.sharedService.dashboardSettings;
    }

    ngOnChanges(changes: SimpleChanges): void {
        var searchFilter = localStorage.getItem("fuel-orders-filters");
        if (
            (searchFilter == null || searchFilter == '') &&
            changes.fuelreqsData &&
            !isEqual(
                changes.fuelreqsData.currentValue,
                changes.fuelreqsData.previousValue
            )
        ) {
            this.allColumnsToDisplay = this.getVisibleColumns();
            this.refreshTable();
        }
    }

    async ngOnInit() {
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

            this.saveSettings();
            this.paginator.pageIndex = 0;
        });

        if (localStorage.getItem('pageIndexFuelReqs')) {
            this.paginator.pageIndex = localStorage.getItem(
                'pageIndexFuelReqs'
            ) as any;
        } else {
            this.paginator.pageIndex = 0;
        }

        this.columns = this.getClientSavedColumns(this.tableLocalStorageKey, initialColumns);

        this.allColumnsToDisplay = this.getVisibleColumns();

        this.refreshTable();
    }
    getVisibleDataColumns() {
        return this.columns
            .filter((column) => !column.hidden)
            .map((column) => {
                if(column.id == 'customer')
                    return 'customerName'
                return column.id
            }) || [];
    }
    getVisibleColumns() {
        var result = ['expand-icon'];
        result.push(...
            this.getVisibleDataColumns()
        );
        result.push('receivedConfirmationButton');
        return result;
    }

    refreshTable(scrollIndex: number = 0) {
        let filter = '';
        if (this.fuelreqsDataSource) {
            filter = this.fuelreqsDataSource.filter;
        }
        this.fuelreqsDataSource = new MatTableDataSource(this.fuelreqsData);
        this.fuelreqsDataSource.sort = this.sort;
        this.fuelreqsDataSource.paginator = this.paginator;
        this.fuelreqsDataSource.filter = filter;
        this.resultsLength = this.fuelreqsData.length;

        this.refreshSort();

        this.setVirtualScrollVariables(this.paginator, this.sort, this.fuelreqsDataSource.data);

        if (scrollIndex > 0)
            this.scrollToIndex(scrollIndex);
    }

    refreshSort() {
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

    applyFilter(filterValue: string) {
        this.fuelreqsDataSource.filter = filterValue.trim().toLowerCase();
    }

    applyDateFilterChange() {
        this.dateFilterChanged.emit({
            filterEndDate: this.filterEndDate,
            filterStartDate: this.filterStartDate,
        });
    }

    exportCsv() {
        let computePropertyFnc = (item: any[], id: string): any => {
            if(id == "eta" || id == "etd" || id == "dateCreated")
                return this.datePipe.transform(item[id]);
            else if(id == "quotedPpg")
                return this.getPPGDisplayString(item);
            else
                return null;
        }
        this.exportCsvFile(this.columns,this.csvFileOptions.fileName,this.csvFileOptions.sheetName,computePropertyFnc);
    }
    getPPGDisplayString(fuelreq: any): any{
        return fuelreq.source == 'FuelerLinx' || fuelreq.source == ''
            ? this.currencyPipe.transform(fuelreq.quotedPpg, "USD", "symbol", "1.4-4")
            : "CONFIDENTIAL";
    }
    openSettings() {
        const dialogRef = this.tableSettingsDialog.open(
            TableSettingsComponent,
            {
                data: this.columns,
            }
        );
        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.columns = [...result];

            this.refreshSort();
            this.saveSettings();
        });
    }

    saveSettings() {
        localStorage.setItem(
            this.tableLocalStorageKey,
            JSON.stringify(this.columns)
        );
    }
    getNoDataToDisplayString(){
        return"No Fuel Request set on the selected range of dates";
    }
    isRowExpanded(elementId: any){
        return this.expandedElement.includes(elementId);
    }
    toogleExpandedRows(elementId: any) {
        if(this.isRowExpanded(elementId)){
            this.expandedElement = this.expandedElement.filter(function (item) {
                return item !== elementId
            });
        }else{
            this.expandedElement.push(elementId);
        }
    }
    
    sendConfirmationNotification(event: Event, fuelreq: any): void{
        event.stopPropagation();

        this.isConfirmedLoadingDictionary[fuelreq.sourceId] = true;

        this.fuelreqsService.sendOrderConfirmationNotification(fuelreq).subscribe(response => {
            fuelreq.isConfirmed = true;
            this.isConfirmedLoadingDictionary[fuelreq.sourceId] = false;
            this.snackBarService.showSuccessSnackBar("Confirmation Sent");
        }, error => {
            this.isConfirmedLoadingDictionary[fuelreq.sourceId] = false;
            console.log(error);
            this.snackBarService.showErrorSnackBar("Error sending confirmation try again later");
        });
    }
    isLoadignConfirmationButton(fuelreq: any): boolean{
        return this.isConfirmedLoadingDictionary[fuelreq.sourceId];
    }
    getConfirmationButtonText(fuelreq: any): string{
        return  this.isLoadignConfirmationButton(fuelreq) ? "" :
        fuelreq.isConfirmed ? "Confirmation Sent" : "Send Confirmation"
    }

    public addServiceOrderClicked() {
        var newServiceOrder: ServiceOrder = {
            oid: 0,
            fboId: this.sharedService.currentUser.fboId,
            serviceOrderItems: [],
            arrivalDateTimeUtc: null,
            arrivalDateTimeLocal: null,
            departureDateTimeUtc: null,
            departureDateTimeLocal: null,
            groupId: this.sharedService.currentUser.groupId,
            customerInfoByGroupId: 0,
            customerAircraftId: 0,
            associatedFuelOrderId: 0,
            serviceOn: ServiceOrderAppliedDateTypes.Arrival,
            numberOfCompletedItems: 0,
            isCompleted: false,
            customerInfoByGroup: null,
            customerAircraft: null,
            numberOfTotalServices: 0,
            isActive: false
        };
        const config: MatDialogConfig = {
            disableClose: true,
            data: newServiceOrder,
            autoFocus: false,
            maxWidth: '510px'
        };

        const dialogRef = this.newServiceOrderDialog.open(ServiceOrdersDialogNewComponent, config);


        dialogRef.afterClosed().subscribe((result: FuelReq) => {
            if (!result)
                return;
            result.serviceOrder = newServiceOrder;
            this.fuelreqsData.push(result);
            this.refreshTable(result.oid);
            this.toogleExpandedRows(result.oid.toString() + (result.sourceId == undefined ? 0 : result.sourceId).toString() + "0")
        });
    }

    public updateArchivedFlag(event: Event, fuelReq: FuelReq) {
        if (fuelReq.archived) {
            const dialogRef = this.templateDialog.open(
                ProceedConfirmationComponent,
                {
                    autoFocus: false,
                    data: {
                        buttonText: 'Yes, Close & Archive',
                        title: 'Are you sure you want to mark each item in this order as complete and archive it?',
                    },
                }
            );

            dialogRef.afterClosed().subscribe((result) => {
                if (!result) {
                    fuelReq.archived = false;
                    return;
                }

                fuelReq.serviceOrder.numberOfCompletedItems = fuelReq.serviceOrder.serviceOrderItems.length - 1;

                fuelReq.serviceOrder.serviceOrderItems.forEach((serviceOrderItem) => {
                    if (serviceOrderItem.serviceName != "")
                        serviceOrderItem.isCompleted = true;
                })

                if (this.fuelReqGridServicesComponent != null)
                    this.fuelReqGridServicesComponent.onArchiveServices(fuelReq.serviceOrder.serviceOrderItems);
                else {
                    this.saveServiceOrderItems(fuelReq.serviceOrder.serviceOrderItems);
                    this.completedServicesChanged(fuelReq, fuelReq.serviceOrder.serviceOrderItems.length)
                }

                var serviceOrderItems = fuelReq.serviceOrder.serviceOrderItems;
                fuelReq.serviceOrder.serviceOrderItems = null;

                this.fuelreqsService.updateArchived(fuelReq).subscribe(response => {
                    fuelReq.serviceOrder.serviceOrderItems = serviceOrderItems;
                });
            });
        }
        else {
            fuelReq.serviceOrder.numberOfCompletedItems = 0;

            fuelReq.serviceOrder.serviceOrderItems.forEach((serviceOrderItem) => {
                if (serviceOrderItem.serviceName != "")
                    serviceOrderItem.isCompleted = false;
            })

            if (this.fuelReqGridServicesComponent != null)
                this.fuelReqGridServicesComponent.onArchiveServices(fuelReq.serviceOrder.serviceOrderItems);
            else {
                this.completedServicesChanged(fuelReq, 0);
                this.saveServiceOrderItems(fuelReq.serviceOrder.serviceOrderItems);
            }

            var serviceOrderItems = fuelReq.serviceOrder.serviceOrderItems;
            fuelReq.serviceOrder.serviceOrderItems = null;

            this.fuelreqsService.updateArchived(fuelReq).subscribe(response => {
                fuelReq.serviceOrder.serviceOrderItems = serviceOrderItems;
            });
        }
    }

    completedServicesChanged(fuelReq: FuelReq, changes: any) {
        if (fuelReq.serviceOrder.numberOfTotalServices == changes.value)
            fuelReq.serviceOrder.numberOfCompletedItems = changes.value;
        else {
            if (changes.value > 0)
                fuelReq.serviceOrder.numberOfCompletedItems++;
            else if (changes.value < 0)
                fuelReq.serviceOrder.numberOfCompletedItems--;
            else {
                fuelReq.serviceOrder.numberOfCompletedItems = 0;
            }
        }

        if (fuelReq.serviceOrder.numberOfCompletedItems == 0) {
            fuelReq.serviceOrder.isActive = false;
            fuelReq.serviceOrder.isCompleted = false;
        }
        else if (fuelReq.serviceOrder.numberOfCompletedItems == 1 && fuelReq.serviceOrder.numberOfTotalServices > 1) {
            fuelReq.serviceOrder.isActive = true;
            fuelReq.serviceOrder.isCompleted = false;
        }
        else if (fuelReq.serviceOrder.numberOfCompletedItems == fuelReq.serviceOrder.numberOfTotalServices) {
            fuelReq.serviceOrder.isCompleted = true;
            fuelReq.serviceOrder.isActive = false;
        }
        else {
            fuelReq.serviceOrder.isCompleted = false;
            fuelReq.serviceOrder.isActive = true;
        }

        if (changes.fuelreqsServicesAndFeesGridDisplay != null)
            fuelReq.serviceOrder.serviceOrderItems = changes.fuelreqsServicesAndFeesGridDisplay.filter(f => f.serviceName != '');
    }

    totalServicesChanged(fuelReq: FuelReq, changes: any) {
        fuelReq.serviceOrder.numberOfTotalServices += changes.value;
        if (changes.fuelreqsServicesAndFeesGridDisplay != null)
            fuelReq.serviceOrder.serviceOrderItems = changes.fuelreqsServicesAndFeesGridDisplay.filter(f => f.serviceName != '');
    }

    private scrollToIndex(id: number): void {
        let elem = document.getElementById(id.toString());
        //let elem = this.rows.find(row => row.nativeElement.id === id.toString());

        elem?.scrollIntoView({ block: 'center', behavior: 'smooth' });
        //elem?.nativeElement.scrollIntoView({ block: 'center', behavior: 'smooth' });
    }

    private saveServiceOrderItems(serviceOrderItems: ServiceOrderItem[]) {
        serviceOrderItems.forEach((serviceOrderItem) => {
            if (serviceOrderItem.serviceName != "")

                if (serviceOrderItem.isCompleted) {
                    serviceOrderItem.completionDateTimeUtc = moment.utc().toDate();
                    serviceOrderItem.completedByUserId = this.sharedService.currentUser.oid;
                    serviceOrderItem.completedByName = this.sharedService.currentUser.firstName + ' ' + this.sharedService.currentUser.lastName;
                }

            this.serviceOrderService.updateServiceOrderItem(serviceOrderItem).subscribe((response: EntityResponseMessage<ServiceOrderItem>) => {

            });
        });
    }
}
