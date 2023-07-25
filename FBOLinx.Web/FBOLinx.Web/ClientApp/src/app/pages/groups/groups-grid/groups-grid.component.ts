import {
    AfterViewInit,
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    ViewChild,
    ViewContainerRef,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import {
    Column,
    ColumnMenuService,
    DetailRowService,
    FilterSettingsModel,
    GridComponent,
    GridModel,
    RecordClickEventArgs,
    SelectionSettingsModel,
    SortEventArgs,
} from '@syncfusion/ej2-angular-grids';
import { first, last } from 'lodash';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { SharedService } from '../../../layouts/shared-service';
import { fboChangedEvent } from '../../../constants/sharedEvents';
import { FbosService } from '../../../services/fbos.service';
// Services
import { GroupsService } from '../../../services/groups.service';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';
import { ManageConfirmationComponent } from '../../../shared/components/manage-confirmation/manage-confirmation.component';
import { NotificationComponent } from '../../../shared/components/notification/notification.component';
import {
    ColumnType,
    TableSettingsComponent,
} from '../../../shared/components/table-settings/table-settings.component';
import { GroupGridState } from '../../../store/reducers/group';
import { FbosDialogNewFboComponent } from '../../fbos/fbos-dialog-new-fbo/fbos-dialog-new-fbo.component';
// Components
import { GroupsDialogNewGroupComponent } from '../groups-dialog-new-group/groups-dialog-new-group.component';
import { GroupsMergeDialogComponent } from '../groups-merge-dialog/groups-merge-dialog.component';
import { AssociationsDialogNewAssociationComponent } from '../../associations/associations-dialog-new-association/associations-dialog-new-association.component';
import { localStorageAccessConstant } from 'src/app/constants/LocalStorageAccessConstant';

const initialColumns: ColumnType[] = [
    {
        id: 'groupName',
        name: 'Group',
        sort: 'asc',
    },
    {
        id: 'expiredFboPricingCount',
        name: 'Pricing Expired',
    },
    {
        id: 'needAttentionCustomers',
        name: 'Need Attentions',
    },
    {
        id: 'lastLogin',
        name: 'Last Login Date',
    },
    {
        id: 'active',
        name: 'Active',
    },
    {
        id: 'users',
        name: 'Users',
    },
    {
        id: 'quotes30Days',
        name: 'Quotes (last 30 days)',
    },
    {
        id: 'orders30Days',
        name: 'Fuel Orders (last 30 days)',
    },
    {
        id: 'expiredFboAccountCount',
        name: 'Accounts Expired',
    },
];

@Component({
    providers: [DetailRowService, ColumnMenuService],
    selector: 'app-groups-grid',
    styleUrls: ['./groups-grid.component.scss'],
    templateUrl: './groups-grid.component.html',
})
export class GroupsGridComponent implements OnInit, AfterViewInit {
    @ViewChild('grid') public grid: GridComponent;
    @ViewChild('fboManageTemplate', { static: true })
    public fboManageTemplate: any;
    @ViewChild('needAttentionTemplate', { static: true })
    public needAttentionTemplate: any;
    @ViewChild('lastLoginTemplate', { static: true })
    public lastLoginTemplate: any;
    @ViewChild('pricingExpiredTemplate', { static: true })
    public pricingExpiredTemplate: any;
    @ViewChild('accountExpiredTemplate', { static: true })
    public accountExpiredTemplate: any;
    @ViewChild('accountTypeTemplate', { static: true })
    public accountTypeTemplate: any;
    @ViewChild('usersTemplate', { static: true }) public usersTemplate: any;

    // Input/Output Bindings
    @Input() groupsFbosData: any;
    @Input() groupGridState: GroupGridState;
    @Output() editGroupClicked = new EventEmitter<any>();
    @Output() editFboClicked = new EventEmitter<any>();
    @Output() deleteGroupClicked = new EventEmitter<any>();

    childGrid: GridModel;
    selectionOptions: SelectionSettingsModel = {
        checkboxMode: 'ResetOnRowClick',
    };

    // Members
    pageTitle = 'Groups';
    searchValue = '';
    groupAccountType: 'all' | 'active' | 'inactive' = 'active';
    fboActiveAccountType: 'all' | 'active' | 'inactive' = 'active';
    fboAccountType: 'all' | 'premium' | 'freemium' = 'premium';
    pageSettings: any = {
        pageSize: 25,
        pageSizes: [25, 50, 100, 'All'],
    };
    public filterSettings: FilterSettingsModel = { type: 'Menu' };

    groupDataSource: any[];
    fboDataSource: any[];

    selectedRows: any[] = [];

    tableLocalStorageKey = 'conductor-group-grid-settings';
    tableLocalStorageFilterKey = 'conductor-group-grid-filter';
    columns: ColumnType[] = [];

    constructor(
        private router: Router,
        private viewContainerRef: ViewContainerRef,
        private groupsService: GroupsService,
        private fbosService: FbosService,
        private sharedService: SharedService,
        private deleteGroupDialog: MatDialog,
        private deleteFboDialog: MatDialog,
        private newGroupDialog: MatDialog,
        private newFboDialog: MatDialog,
        private notification: MatDialog,
        private manageDialog: MatDialog,
        private mergeGroupsDialog: MatDialog,
        private tableSettingsDialog: MatDialog,
        private snackBar: MatSnackBar,
        private addAssociationDialog: MatDialog
    ) {}

    ngOnInit() {
        this.refreshGrid();
    }

    refreshGrid() {
        this.groupDataSource = this.groupsFbosData.groups;
        this.fboDataSource = this.groupsFbosData.fbos;

        this.sharedService.titleChange(this.pageTitle);
        const self = this;
        this.childGrid = {
            columns: [
                { field: 'icao', headerText: 'ICAO' },
                { field: 'fbo', headerText: 'FBO' },
                {
                    headerText: 'Pricing Expired',
                    template: this.pricingExpiredTemplate,
                },
                {
                    headerText: 'Need Attentions',
                    template: this.needAttentionTemplate,
                },
                {
                    headerText: 'Last Login Date',
                    template: this.lastLoginTemplate,
                },
                { headerText: 'Users', template: this.usersTemplate },
                { field: 'quotes30Days', headerText: 'Quotes (last 30 days)' },
                {
                    field: 'orders30Days',
                    headerText: 'Fuel Orders (last 30 days)',
                },
                {
                    headerText: 'Account Expired',
                    template: this.accountExpiredTemplate,
                },
                {
                    headerText: 'Account Type',
                    template: this.accountTypeTemplate,
                },
                { template: this.fboManageTemplate, width: 150 },
            ],
            dataSource: this.fboDataSource,
            load() {
                this.registeredTemplate = {}; // set registertemplate value as empty in load event
                (this as GridComponent).parentDetails.parentKeyFieldValue = (
                    (this as GridComponent).parentDetails.parentRowData as {
                        oid?: string;
                    }
                ).oid;
            },
            queryString: 'groupId',
            recordClick: (args: RecordClickEventArgs) => {
                self.manageFBO(args.rowData);
            },
        };

        if (localStorage.getItem(this.tableLocalStorageKey)) {
            const savedColumns = JSON.parse(
                localStorage.getItem(this.tableLocalStorageKey)
            ) as ColumnType[];
            if (savedColumns.length === initialColumns.length) {
                this.columns = savedColumns;
            } else {
                this.columns = initialColumns;
            }
        } else {
            this.columns = initialColumns;
        }

        if (this.groupGridState.filter) {
            this.applyFilter(this.groupGridState.filter);
        } else if (localStorage.getItem(this.tableLocalStorageFilterKey)) {
            this.applyFilter(
                localStorage.getItem(this.tableLocalStorageFilterKey)
            );
        } else {
            this.applyFilter('');
        }
    }

    ngAfterViewInit() {
        this.fboManageTemplate.elementRef.nativeElement._viewContainerRef =
            this.viewContainerRef;
        this.fboManageTemplate.elementRef.nativeElement.propName = 'template';

        this.needAttentionTemplate.elementRef.nativeElement._viewContainerRef =
            this.viewContainerRef;
        this.needAttentionTemplate.elementRef.nativeElement.propName =
            'template';

        this.lastLoginTemplate.elementRef.nativeElement._viewContainerRef =
            this.viewContainerRef;
        this.lastLoginTemplate.elementRef.nativeElement.propName = 'template';

        this.pricingExpiredTemplate.elementRef.nativeElement._viewContainerRef =
            this.viewContainerRef;
        this.pricingExpiredTemplate.elementRef.nativeElement.propName =
            'template';

        this.accountExpiredTemplate.elementRef.nativeElement._viewContainerRef =
            this.viewContainerRef;
        this.accountExpiredTemplate.elementRef.nativeElement.propName =
            'template';

        this.usersTemplate.elementRef.nativeElement._viewContainerRef =
            this.viewContainerRef;
        this.usersTemplate.elementRef.nativeElement.propName = 'template';

        this.accountTypeTemplate.elementRef.nativeElement._viewContainerRef =
            this.viewContainerRef;
        this.accountTypeTemplate.elementRef.nativeElement.propName =
            'template';

        setTimeout(() => {
            this.refreshColumns();
        }, 100);
    }

    get columnsString() {
        return this.columns
            .map(
                (column) =>
                    column.id + ' ' + (column.hidden ? 'hidden' : 'visible')
            )
            .join(', ');
    }

    deleteGroup(record) {
        const dialogRef = this.deleteGroupDialog.open(
            DeleteConfirmationComponent,
            {
                autoFocus: false,
                data: {
                    fullDescription:
                        'You are about to remove this group. This will remove the fbos and all the other data related to the group. Are you sure?',
                    item: record,
                },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.deleteGroupClicked.emit({
                isDeletingGroup: true
            });

            const deleteIndex = this.groupsFbosData.groups.findIndex(
                (group) => group.oid === record.oid
            );
            this.groupsService.remove(record).subscribe(() => {
                this.groupsFbosData.groups.splice(deleteIndex, 1);
                this.snackBar.open(record.groupName + ' is deleted', '', {
                    duration: 2000,
                    panelClass: ['blue-snackbar'],
                });
                this.refreshGrid();
                this.sharedService.emitChange('group deleted');
            });
        });
    }

    deleteFbo(record) {
        const dialogRef = this.deleteFboDialog.open(
            DeleteConfirmationComponent,
            {
                autoFocus: false,
                data: {
                    description: 'FBO',
                    item: record,
                },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.fbosService.remove(record).subscribe(() => {
                const fboIndex = this.groupsFbosData.fbos.findIndex(
                    (fbo) => fbo.oid === record.oid
                );
                this.groupsFbosData.fbos.splice(fboIndex, 1);
                this.snackBar.open(record.fbo + ' is deleted', '', {
                    duration: 2000,
                    panelClass: ['blue-snackbar'],
                });
                //this.grid.refresh();
                this.applyFilter('');
            });
        });
    }

    rowSelected(event: any) {
        if (!event.isInteracted) {
            this.manageGroup(event.data);
        } else {
            this.selectedRows = this.grid.getSelectedRecords();
        }
    }

    rowDeselected() {
        this.selectedRows = this.grid.getSelectedRecords();
    }

    addNewGroupOrFbo() {
        const dialogRef = this.newGroupDialog.open(
            GroupsDialogNewGroupComponent,
            {
                data: {
                    initialSetupPhase: true,
                    oid: 0,
                },
                width: '450px',
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            if (result.type === 'group') {
                this.editGroupClicked.emit({
                    group: result.data,
                    searchValue: this.searchValue,
                });
            } else {
                this.editFboClicked.emit({
                    fbo: result.data,
                    searchValue: this.searchValue,
                });
            }
        });
    }

    addNewFbo(group: any) {
        const dialogRef = this.newFboDialog.open(FbosDialogNewFboComponent, {
            data: {
                groupId: group.oid,
                initialSetupPhase: true,
                oid: 0,
            },
            width: '450px',
        });

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.editFboClicked.emit({
                    fbo: result,
                    searchValue: this.searchValue,
                });
            }
        });
    }

    manageGroup(group: any) {
        if (!group.active) {
            this.notification.open(NotificationComponent, {
                data: {
                    text: "You can't manage an inactive group!",
                    title: 'Inactive group',
                },
            });
        } else {
            const dialogRef = this.manageDialog.open(
                ManageConfirmationComponent,
                {
                    autoFocus: false,
                    data: {
                        description:
                            'This will temporarily switch your account to a group admin for this group.  Would you like to continue?',
                        group: true,
                        title: 'Manage Group?',
                    },
                    width: '450px',
                }
            );
            dialogRef.afterClosed().subscribe((result) => {
                if (!result) {
                    return;
                }

                localStorage.setItem(
                    this.tableLocalStorageFilterKey,
                    this.searchValue
                );

                this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.managerGroupId,this.sharedService.currentUser.groupId);

                this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.groupId,group.oid);

                this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.impersonatedrole,2);

                this.router.navigate(['/default-layout/fbos/']);
            });
        }
    }

    manageFBO(fbo: any) {
        if (!fbo.active) {
            this.notification.open(NotificationComponent, {
                data: {
                    text: "You can't manage an inactive fbo!",
                    title: 'Inactive fbo',
                },
            });
        } else {
            const dialogRef = this.manageDialog.open(
                ManageConfirmationComponent,
                {
                    autoFocus: false,
                    data: {
                        description:
                            'This will temporarily switch your account to a primary user for this FBO.  Would you like to continue?',
                        fboId: fbo.oid,
                        title: 'Manage FBO?',
                    },
                    width: '450px',
                }
            );

            dialogRef.afterClosed().subscribe((result) => {
                if (!result) {
                    return;
                }

                localStorage.setItem(
                    this.tableLocalStorageFilterKey,
                    this.searchValue
                );

                this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.managerGroupId,this.sharedService.currentUser.groupId);

                this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.managerGroupId,this.sharedService.currentUser.groupId);

                this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.groupId,fbo.groupId);

                this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.impersonatedrole,1);

                this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.conductorFbo,true);

                this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.fboId,fbo.oid);

                this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.icao,fbo.icao);

                this.sharedService.setCurrentUserPropertyValue(localStorageAccessConstant.accountType,fbo.accountType);

                this.sharedService.emitChange(fboChangedEvent);
                this.router.navigate(['/default-layout/dashboard-fbo-updated/']);
            });
        }
    }

    mergeGroups() {
        const dialogRef = this.mergeGroupsDialog.open(
            GroupsMergeDialogComponent,
            {
                data: {
                    groups: this.selectedRows,
                },
                width: '450px',
            }
        );
        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.grid.clearSelection();

            this.groupsFbosData.groups = this.groupsFbosData.groups.filter(
                (group) =>
                    !result.groups.find(
                        (bg) =>
                            bg.oid !== result.baseGroupId &&
                            bg.oid === group.oid
                    )
            );
            this.groupDataSource = this.groupsFbosData.groups;

            this.groupsFbosData.fbos = this.groupsFbosData.fbos.map((fbo) => {
                if (result.groups.find((bg) => bg.oid === fbo.groupId)) {
                    fbo.groupId = result.baseGroupId;
                }
                return fbo;
            });
            this.fboDataSource = this.groupsFbosData.fbos;
            this.childGrid.dataSource = this.fboDataSource;

            this.grid.refresh();
        });
    }

    addAssociation() {
        const dialogRef = this.addAssociationDialog.open(
            AssociationsDialogNewAssociationComponent,
            {
                width: '450px',
            }
        );
        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
        });
    }

    applyFilter(filterValue: string) {
        this.searchValue = filterValue;
        localStorage.setItem(this.tableLocalStorageFilterKey, filterValue);

        const filteredFbos = this.groupsFbosData.fbos.filter(
            (fbo) =>
                (this.fboActiveAccountType === 'all' ||
                (this.fboActiveAccountType === 'active' && !fbo.accountExpired) ||
                (this.fboActiveAccountType === 'inactive' && fbo.accountExpired))
                &&
                (
                    (this.fboAccountType === 'all' ||
                        (this.fboAccountType === 'premium' && !fbo.accountType) ||
                        (this.fboAccountType === 'freemium' && fbo.accountType))
                )
        );
        const filteredGroups = this.groupsFbosData.groups.filter(
            (group) =>
                this.groupAccountType === 'all' ||
                (this.groupAccountType === 'active' && group.active) ||
                (this.groupAccountType == 'inactive' && !group.active)
        );

        if (!filterValue) {
            this.groupDataSource = filteredGroups;
            this.fboDataSource = filteredFbos;
            this.childGrid.dataSource = this.fboDataSource;
            return;
        }

        const firstFilteredFbos = filteredFbos.filter(
            (fbo) =>
                this.ifStringContains(fbo.icao, filterValue) ||
                this.ifStringContains(fbo.fbo, filterValue) ||
                fbo.users?.find(
                    (user) =>
                        this.ifStringContains(
                            user.firstName + ' ' + user.lastName,
                            filterValue
                        ) || this.ifStringContains(user.username, filterValue)
                )
        );

        const firstFilteredGroups = filteredGroups.filter(
            (group) =>
                this.ifStringContains(group.groupName, filterValue) ||
                group.users?.find(
                    (user) =>
                        this.ifStringContains(
                            user.firstName + ' ' + user.lastName,
                            filterValue
                        ) || this.ifStringContains(user.username, filterValue)
                )
        );

        const secondFilteredGroups = filteredGroups.filter(
            (group) =>
                this.ifStringContains(group.groupName, filterValue) ||
                group.users?.find(
                    (user) =>
                        this.ifStringContains(
                            user.firstName + ' ' + user.lastName,
                            filterValue
                        ) || this.ifStringContains(user.username, filterValue)
                ) ||
                firstFilteredFbos.find((fbo) => fbo.groupId === group.oid)
        );

        const secondFilteredFbos = filteredFbos.filter(
            (fbo) =>
                firstFilteredGroups.find(
                    (group) => group.oid === fbo.groupId
                ) ||
                this.ifStringContains(fbo.icao, filterValue) ||
                this.ifStringContains(fbo.fbo, filterValue) ||
                fbo.users?.find(
                    (user) =>
                        this.ifStringContains(
                            user.firstName + ' ' + user.lastName,
                            filterValue
                        ) || this.ifStringContains(user.username, filterValue)
                )
        );

        this.groupDataSource = secondFilteredGroups;
        this.fboDataSource = secondFilteredFbos;
        this.childGrid.dataSource = this.fboDataSource;
    }

    groupFbos(groupIndex: number) {
        const group = (this.grid.dataSource as any[])[groupIndex];
        if (!group) {
            return [];
        }
        return this.groupsFbosData.fbos.filter(
            (fbo) => fbo.groupId === group.oid
        );
    }

    editGroup(group: any) {
        this.editGroupClicked.emit({
            group,
            searchValue: this.searchValue,
        });
    }

    editFBO(fbo: any) {
        this.editFboClicked.emit({
            fbo,
            searchValue: this.searchValue,
        });
    }

    openSettings() {
        const dialogRef = this.tableSettingsDialog.open(
            TableSettingsComponent,
            {
                data: [...this.columns],
            }
        );
        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.columns = [...result];

            this.refreshColumns();
            this.saveSettings();
        });
    }

    dateSortComparer(reference: string, comparer: string) {
        const a = reference ? new Date(reference) : new Date(-8640000000000000);
        const b = comparer ? new Date(comparer) : new Date(-8640000000000000);

        if (a < b) {
            return -1;
        }
        if (a > b) {
            return 1;
        }
        return 0;
    }

    actionHandler(args: SortEventArgs) {
        if (args.requestType === 'sorting') {
            this.columns = this.columns.map((column) =>
                column.id === args.columnName && !column.hidden
                    ? {
                          ...column,
                          sort: args.direction === 'Ascending' ? 'asc' : 'desc',
                      }
                    : {
                          hidden: column.hidden,
                          id: column.id,
                          name: column.name,
                      }
            );

            this.saveSettings();
        }
    }

    filterChanged() {
        this.applyFilter(this.searchValue);
    }
    isValidPricing(data: any) {
        return data.expiredFboPricingCount === 0 && data.activeFboCount > 0;
    }
    private saveSettings() {
        localStorage.setItem(
            this.tableLocalStorageKey,
            JSON.stringify(this.columns)
        );
    }

    private refreshColumns() {
        const invisibleColumns = this.columns
            .filter((c) => c.hidden)
            .map((c) => c.name);
        const visibleColumns = this.columns
            .filter((c) => !c.hidden)
            .map((c) => c.name);

        const sortedColumn = this.columns.filter((c) => c.sort)[0];

        this.reorderColumns();

        this.grid.showColumns(visibleColumns);
        this.grid.hideColumns(invisibleColumns);

        if (sortedColumn) {
            this.grid.sortColumn(
                sortedColumn.id,
                sortedColumn.sort === 'asc' ? 'Ascending' : 'Descending'
            );
        }
    }

    private reorderColumns() {
        const gridColumns = this.grid.columns as Column[];
        const newColumns: Column[] = [first(gridColumns)];

        for (const column of this.columns) {
            newColumns.push(gridColumns.find((c) => c.field === column.id));
        }

        newColumns.push(last(gridColumns));

        this.grid.columns = newColumns;
    }

    private ifStringContains(str1: string, str2: string) {
        return (!str1 ? '' : str1)
            .toLowerCase()
            .includes((!str2 ? '' : str2).toLowerCase());
    }
}
