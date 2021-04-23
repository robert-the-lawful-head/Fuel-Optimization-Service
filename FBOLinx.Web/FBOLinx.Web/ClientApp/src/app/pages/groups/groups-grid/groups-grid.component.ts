import {
    AfterViewInit,
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    ViewChild,
    ViewContainerRef
} from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
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

// Services
import { GroupsService } from '../../../services/groups.service';
import { FbosService } from '../../../services/fbos.service';
import { SharedService } from '../../../layouts/shared-service';

// Components
import { GroupsDialogNewGroupComponent } from '../groups-dialog-new-group/groups-dialog-new-group.component';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';
import { NotificationComponent } from '../../../shared/components/notification/notification.component';
import { ManageConfirmationComponent } from '../../../shared/components/manage-confirmation/manage-confirmation.component';
import { fboChangedEvent } from '../../../models/sharedEvents';
import { FbosDialogNewFboComponent } from '../../fbos/fbos-dialog-new-fbo/fbos-dialog-new-fbo.component';
import { GroupsMergeDialogComponent } from '../groups-merge-dialog/groups-merge-dialog.component';
import { GroupGridState } from '../../../store/reducers/group';
import { ColumnType, TableSettingsComponent } from '../../../shared/components/table-settings/table-settings.component';
import { first, last } from 'lodash';

const initialColumns: ColumnType[] = [
    {
        id: 'groupName',
        name: 'Group',
        sort: 'asc',
    },
    {
        id: 'expiredFboCount',
        name: 'Expired',
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
        name: 'Users'
    }
];


@Component({
    selector: 'app-groups-grid',
    templateUrl: './groups-grid.component.html',
    styleUrls: [ './groups-grid.component.scss' ],
    providers: [ DetailRowService, ColumnMenuService ],
})
export class GroupsGridComponent implements OnInit, AfterViewInit {
    @ViewChild('grid') public grid: GridComponent;
    @ViewChild('fboManageTemplate', { static: true }) public fboManageTemplate: any;
    @ViewChild('needAttentionTemplate', { static: true }) public needAttentionTemplate: any;
    @ViewChild('lastLoginTemplate', { static: true }) public lastLoginTemplate: any;
    @ViewChild('pricingExpiredTemplate', { static: true }) public pricingExpiredTemplate: any;
    @ViewChild('usersTemplate', { static: true }) public usersTemplate: any;

    // Input/Output Bindings
    @Input() groupsFbosData: any;
    @Input() groupGridState: GroupGridState;
    @Output() editGroupClicked = new EventEmitter<any>();
    @Output() editFboClicked = new EventEmitter<any>();

    childGrid: GridModel;
    selectionOptions: SelectionSettingsModel = {
        checkboxMode: 'ResetOnRowClick'
    };

    // Members
    pageTitle = 'Groups';
    searchValue = '';
    pageSettings: any = {
        pageSizes: [ 25, 50, 100, 'All' ],
        pageSize: 25,
    };
    public filterSettings: FilterSettingsModel = { type: 'Menu' };

    groupDataSource: any[];
    fboDataSource: any[];

    selectedRows: any[] = [];

    tableLocalStorageKey = 'conductor-group-grid-settings';
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
    ) {
    }

    ngOnInit() {
        this.groupDataSource = this.groupsFbosData.groups;
        this.fboDataSource = this.groupsFbosData.fbos;

        this.sharedService.titleChange(this.pageTitle);
        const self = this;
        this.childGrid = {
            dataSource: this.fboDataSource,
            queryString: 'groupId',
            columns: [
                { field: 'icao', headerText: 'ICAO', width: 100 },
                { field: 'fbo', headerText: 'FBO' },
                { template: this.pricingExpiredTemplate },
                { template: this.needAttentionTemplate },
                { template: this.lastLoginTemplate, headerText: 'Last Login Date', width: 200 },
                { field: 'active', headerText: 'Active', width: 100 },
                { template: this.usersTemplate, headerText: 'Users' },
                { template: this.fboManageTemplate, width: 150 },
            ],
            load() {
                this.registeredTemplate = {};   // set registertemplate value as empty in load event
                (this as GridComponent).parentDetails.parentKeyFieldValue
                    = ((this as GridComponent).parentDetails.parentRowData as { oid?: string }).oid;
            },
            recordClick: (args: RecordClickEventArgs) => {
                self.manageFBO(args.rowData);
            },
        };
        if (this.groupGridState.filter) {
            this.applyFilter(this.groupGridState.filter);
        }

        // if (localStorage.getItem(this.tableLocalStorageKey)) {
        //     this.columns = JSON.parse(localStorage.getItem(this.tableLocalStorageKey));
        // } else {
            this.columns = initialColumns;
        // }
    }

    ngAfterViewInit() {
        this.fboManageTemplate.elementRef.nativeElement._viewContainerRef = this.viewContainerRef;
        this.fboManageTemplate.elementRef.nativeElement.propName = 'template';

        this.needAttentionTemplate.elementRef.nativeElement._viewContainerRef = this.viewContainerRef;
        this.needAttentionTemplate.elementRef.nativeElement.propName = 'template';

        this.lastLoginTemplate.elementRef.nativeElement._viewContainerRef = this.viewContainerRef;
        this.lastLoginTemplate.elementRef.nativeElement.propName = 'template';

        this.pricingExpiredTemplate.elementRef.nativeElement._viewContainerRef = this.viewContainerRef;
        this.pricingExpiredTemplate.elementRef.nativeElement.propName = 'template';

        this.usersTemplate.elementRef.nativeElement._viewContainerRef = this.viewContainerRef;
        this.usersTemplate.elementRef.nativeElement.propName = 'template';

        setTimeout(() => {
            this.refreshColumns();
        }, 100);
    }

    get columnsString() {
        return this.columns.map(column => column.id + ' ' + (column.hidden ? 'hidden' : 'visible')).join(', ');
    }

    deleteGroup(record) {
        const dialogRef = this.deleteGroupDialog.open(
            DeleteConfirmationComponent, {
                data: {
                    item: record,
                    fullDescription: 'You are about to remove this group. This will remove the fbos and all the other data related to the group. Are you sure?'
                },
                autoFocus: false,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            const deleteIndex = this.groupsFbosData.groups.findIndex(group => group.oid === record.oid);
            this.groupsService.remove(record).subscribe(() => {
                this.groupsFbosData.groups.splice(deleteIndex, 1);
                this.snackBar.open(record.groupName + ' is deleted', '', {
                    duration: 2000,
                    panelClass: [ 'blue-snackbar' ],
                });
                this.grid.refresh();
            });
        });
    }

    deleteFbo(record) {
        const dialogRef = this.deleteFboDialog.open(
            DeleteConfirmationComponent, {
                data: {
                    item: record,
                    description: 'FBO'
                },
                autoFocus: false,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.fbosService.remove(record).subscribe(() => {
                const fboIndex = this.groupsFbosData.fbos.findIndex(fbo => fbo.oid === record.oid);
                this.groupsFbosData.fbos.splice(fboIndex, 1);
                this.snackBar.open(record.fbo + ' is deleted', '', {
                    duration: 2000,
                    panelClass: [ 'blue-snackbar' ],
                });
                this.grid.refresh();
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
            GroupsDialogNewGroupComponent, {
                width: '450px',
                data: {
                    oid: 0,
                    initialSetupPhase: true
                },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            if (result.type === 'group') {
                this.editGroupClicked.emit(result.data);
            } else {
                this.editFboClicked.emit(result.data);
            }

        });
    }

    addNewFbo(group: any) {
        const dialogRef = this.newFboDialog.open(FbosDialogNewFboComponent, {
            width: '450px',
            data: {
                oid: 0,
                initialSetupPhase: true,
                groupId: group.oid
            },
        });

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.editFboClicked.emit(result);
            }
        });
    }

    manageGroup(group: any) {
        if (!group.active) {
            this.notification.open(
                NotificationComponent, {
                    data: {
                        title: 'Inactive group',
                        text: 'You can\'t manage an inactive group!',
                    },
                }
            );
        } else {
            const dialogRef = this.manageDialog.open(
                ManageConfirmationComponent, {
                    width: '450px',
                    data: {
                        title: 'Manage Group?',
                        description: 'This will temporarily switch your account to a group admin for this group.  Would you like to continue?',
                        group: true
                    },
                    autoFocus: false,
                }
            );
            dialogRef.afterClosed().subscribe((result) => {
                if (!result) {
                    return;
                }

                this.sharedService.currentUser.managerGroupId = this.sharedService.currentUser.groupId;
                localStorage.setItem('managerGroupId', this.sharedService.currentUser.groupId.toString());
                this.sharedService.currentUser.groupId = group.oid;
                localStorage.setItem('groupId', group.oid);
                localStorage.setItem('impersonatedrole', '2');
                this.sharedService.currentUser.impersonatedRole = 2;
                this.router.navigate([ '/default-layout/fbos/' ]);
            });
        }
    }

    manageFBO(fbo: any) {
        if (!fbo.active) {
            this.notification.open(
                NotificationComponent, {
                    data: {
                        title: 'Inactive fbo',
                        text: 'You can\'t manage an inactive fbo!',
                    },
                }
            );
        } else {
            const dialogRef = this.manageDialog.open(
                ManageConfirmationComponent, {
                    width: '450px',
                    data: {
                        title: 'Manage FBO?',
                        description: 'This will temporarily switch your account to a primary user for this FBO.  Would you like to continue?',
                        fboId: fbo.oid,
                    },
                    autoFocus: false,
                }
            );

            dialogRef.afterClosed().subscribe((result) => {
                if (!result) {
                    return;
                }

                localStorage.setItem('managerGroupId', this.sharedService.currentUser.groupId.toString());
                this.sharedService.currentUser.managerGroupId = this.sharedService.currentUser.groupId;

                localStorage.setItem('groupId', fbo.groupId.toString());
                this.sharedService.currentUser.groupId = fbo.groupId;

                localStorage.setItem('impersonatedrole', '1');
                this.sharedService.currentUser.impersonatedRole = 1;

                localStorage.setItem('conductorFbo', 'true');
                this.sharedService.currentUser.conductorFbo = true;

                localStorage.setItem('fboId', fbo.oid.toString());
                this.sharedService.currentUser.fboId = fbo.oid;

                this.sharedService.emitChange(fboChangedEvent);
                this.router.navigate([ '/default-layout/dashboard-fbo/' ]);
            });
        }
    }

    mergeGroups() {
        const dialogRef = this.mergeGroupsDialog.open(
            GroupsMergeDialogComponent, {
                width: '450px',
                data: {
                    groups: this.selectedRows
                },
            }
        );
        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.grid.clearSelection();

            this.groupsFbosData.groups = this.groupsFbosData.groups.filter(group =>
                !result.groups.find(bg => bg.oid !== result.baseGroupId && bg.oid === group.oid)
            );
            this.groupDataSource = this.groupsFbosData.groups;

            this.groupsFbosData.fbos = this.groupsFbosData.fbos.map(fbo => {
                if (result.groups.find(bg => bg.oid === fbo.groupId)) {
                    fbo.groupId = result.baseGroupId;
                }
                return fbo;
            });
            this.fboDataSource = this.groupsFbosData.fbos;
            this.childGrid.dataSource = this.fboDataSource;

            this.grid.refresh();
        });
    }

    applyFilter(filterValue: string) {
        this.searchValue = filterValue;

        if (!filterValue || !filterValue.length) {
            this.groupDataSource = this.groupsFbosData.groups;
            this.fboDataSource = this.groupsFbosData.fbos;
            this.childGrid.dataSource = this.fboDataSource;
            return;
        }

        const firstFilteredFbos = this.groupsFbosData.fbos.filter(fbo =>
            this.ifStringContains(fbo.icao, filterValue) ||
            this.ifStringContains(fbo.fbo, filterValue) ||
            fbo.users?.find(user =>
                this.ifStringContains(user.firstName + ' ' + user.lastName, filterValue) ||
                this.ifStringContains(user.username, filterValue)
            )
        );

        const firstFilteredGroups = this.groupsFbosData.groups.filter(group =>
            this.ifStringContains(group.groupName, filterValue) ||
            group.users?.find(user =>
                this.ifStringContains(user.firstName + ' ' + user.lastName, filterValue) ||
                this.ifStringContains(user.username, filterValue)
            )
        );

        const secondFilteredGroups = this.groupsFbosData.groups.filter(group =>
            this.ifStringContains(group.groupName, filterValue) ||
            group.users?.find(user =>
                this.ifStringContains(user.firstName + ' ' + user.lastName, filterValue) ||
                this.ifStringContains(user.username, filterValue)
            ) || firstFilteredFbos.find(fbo => fbo.groupId === group.oid)
        );

        const secondFilteredFbos = this.groupsFbosData.fbos.filter(fbo =>
            firstFilteredGroups.find(group => group.oid === fbo.groupId) ||
            this.ifStringContains(fbo.icao, filterValue) ||
            this.ifStringContains(fbo.fbo, filterValue) ||
            fbo.users?.find(user =>
                this.ifStringContains(user.firstName + ' ' + user.lastName, filterValue) ||
                this.ifStringContains(user.username, filterValue)
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
        return this.groupsFbosData.fbos.filter(fbo => fbo.groupId === group.oid);
    }

    fbosPricingExpired(group: any) {
        console.log(group);
    }

    ifStringContains(str1: string, str2: string) {
        return (!str1 ? '' : str1).toLowerCase().includes((!str2 ? '' : str2).toLowerCase());
    }

    editGroup(group: any) {
        this.editGroupClicked.emit({
            group,
            searchValue: this.searchValue
        });
    }

    editFBO(fbo: any) {
        this.editFboClicked.emit({
            fbo,
            searchValue: this.searchValue
        });
    }

    openSettings() {
        const dialogRef = this.tableSettingsDialog.open(TableSettingsComponent, {
            data: [...this.columns]
        });
        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.columns = [ ...result ];

            this.refreshColumns();
            this.saveSettings();
        });
    }

    saveSettings() {
        localStorage.setItem(this.tableLocalStorageKey, JSON.stringify(this.columns));
    }

    refreshColumns() {
        const invisibleColumns = this.columns.filter(c => c.hidden).map(c => c.id);
        const visibleColumns = this.columns.filter(c => !c.hidden).map(c => c.id);

        const sortedColumn = this.columns.filter(c => c.sort)[0];

        this.grid.showColumns(visibleColumns);
        this.grid.hideColumns(invisibleColumns);

        this.grid.reorderColumns(visibleColumns.slice(1), visibleColumns[0]);

        this.reorderColumns();

        if (sortedColumn) {
            this.grid.sortColumn(sortedColumn.id, sortedColumn.sort === 'asc' ? 'Ascending' : 'Descending');
        }
    }

    reorderColumns() {
        const gridColumns = this.grid.columns as Column[];
        const newColumns: Column[] = [first(gridColumns)];

        for (const column of this.columns) {
            newColumns.push(gridColumns.find(c => c.field === column.id));
        }

        newColumns.push(last(gridColumns));

        this.grid.columns = newColumns;
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
            this.columns = this.columns.map(column => column.id === args.columnName ? ({
                ...column,
                sort: args.direction === 'Ascending' ? 'asc' : 'desc'
            }) : ({
                id: column.id,
                name: column.name
            }));

            this.saveSettings();
        }
    }
}
