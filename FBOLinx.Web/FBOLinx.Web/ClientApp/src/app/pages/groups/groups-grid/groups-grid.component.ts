import {
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    ViewChild,
    ViewContainerRef,
} from '@angular/core';
import { MatLegacyDialog as MatDialog } from '@angular/material/legacy-dialog';
import { MatLegacySnackBar as MatSnackBar } from '@angular/material/legacy-snack-bar';
import { Router } from '@angular/router';
import { Dictionary, groupBy } from 'lodash';

import { SharedService } from '../../../layouts/shared-service';
import {
    accountTypeChangedEvent,
} from '../../../constants/sharedEvents';
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
import { ManageFboGroupsService } from 'src/app/services/managefbo.service';
import { localStorageAccessConstant } from 'src/app/constants/LocalStorageAccessConstant';
import { accountStatusType, AccountType, userAccountType, UserRole } from 'src/app/enums/user-role';
import { GroupFboViewModel } from 'src/app/models/groups';
import {
    animate,
    state,
    style,
    transition,
    trigger,
} from '@angular/animations';
import { MatSort } from '@angular/material/sort';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource } from '@angular/material/table';
import { GridBase } from 'src/app/services/tables/GridBase';
import { MatPaginator } from '@angular/material/paginator';
import { GroupFbosGridComponent } from '../group-fbos-grid/group-fbos-grid.component';

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
    }
];

@Component({
    selector: 'app-groups-grid',
    styleUrls: ['./groups-grid.component.scss'],
    templateUrl: './groups-grid.component.html',
    animations: [
        trigger('detailExpand', [
            state('collapsed', style({ height: '0px', minHeight: '0' })),
            state('expanded', style({ height: '*' })),
            transition(
                'expanded <=> collapsed',
                animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')
            ),
        ]),
    ],
})
export class GroupsGridComponent extends GridBase implements OnInit {
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(GroupFbosGridComponent) groupFbosGridComponent: GroupFbosGridComponent;


    // Input/Output Bindings
    @Input() groupsFbosData: GroupFboViewModel;
    @Input() groupGridState: GroupGridState;
    @Output() editGroupClicked = new EventEmitter<any>();
    @Output() deleteGroupClicked = new EventEmitter<any>();

    // Members
    pageTitle = 'Groups';
    searchValue = '';

    groupAccountType = accountStatusType;
    fboActiveAccountType = accountStatusType;
    fboAccountType =  userAccountType;

    groupDataSource: any[];
    fboDataSourceDictionary: Dictionary<any>;

    tableLocalStorageKey = 'conductor-group-grid-settings';
    tableLocalStorageFilterKey = 'conductor-group-grid-filter';
    columns: ColumnType[] = [];

    expandedElement: any = null;
    columnsToDisplay: string[] = [];
    dataColumns: string[] = [];

    customColumContent = [
        'expiredFboPricingCount',
        'needAttentionCustomers',
        'active',
        'lastLogin',
        'users',
        'expiredFboAccountCount',
    ];

    dataSource = new MatTableDataSource<any>();
    selection = new SelectionModel<any>(true, []);

    isCustomContent(columnId) {
        return this.customColumContent.includes(columnId);
    }
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
        private addAssociationDialog: MatDialog,
        private manageFboGroupsService: ManageFboGroupsService
    ) {
        super();
    }

    ngOnInit() {
        this.refreshGrid();
    }

    refreshGrid() {
        this.groupDataSource = this.groupsFbosData.groups;
        this.fboDataSourceDictionary = groupBy(
            this.groupsFbosData.fbos,
            (gf) => {
                return gf.groupId;
            }
        );

        this.columns = this.getClientSavedColumns(
            this.tableLocalStorageKey,
            initialColumns
        );

        if (this.groupGridState.filter) {
            this.applyFilter(this.groupGridState.filter);
        } else if (localStorage.getItem(this.tableLocalStorageFilterKey)) {
            this.applyFilter(
                localStorage.getItem(this.tableLocalStorageFilterKey)
            );
        } else {
            this.applyFilter('');
        }

        this.setTableColumns();

        this.setVirtualScrollVariables(
            this.paginator,
            this.sort,
            this.groupDataSource
        );
    }
    //todo: ot might be moved to base  grid
    private setTableColumns() {
        this.dataColumns = this.getFilteredTableColumns(this.columns);
        this.columnsToDisplay = this.getFilteredTableColumns(this.columns);
        this.columnsToDisplay.push('actions');
        this.columnsToDisplay.unshift('expandIcon', 'select');
    }
    getColumnDisplayString(columnId){
        let column  = this.columns.filter(obj => {
            return obj.id == columnId
          });
        return column[0]?.name ?? '';
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
                    includeThis: true,
                },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.deleteGroupClicked.emit({
                isDeletingGroup: true,
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
                debugger;
                const fboIndexaux = this.fboDataSourceDictionary[
                    record.groupId
                ].findIndex((fbo) => fbo.oid === record.oid);
                this.fboDataSourceDictionary[record.groupId].splice(
                    fboIndexaux,
                    1
                );

                let dicCpy = Object.assign(
                    {},
                    this.fboDataSourceDictionary[record.groupId]
                );
                this.fboDataSourceDictionary[record.groupId] = Object.assign(
                    {},
                    dicCpy
                );

                this.snackBar.open(record.fbo + ' is deleted', '', {
                    duration: 2000,
                    panelClass: ['blue-snackbar'],
                });
            });
        });
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
                this.groupFbosGridComponent.editFBO(result);
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
                this.groupFbosGridComponent.editFBO(result);
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

                this.sharedService.setCurrentUserPropertyValue(
                    localStorageAccessConstant.managerGroupId,
                    this.sharedService.currentUser.groupId.toString()
                );

                this.sharedService.setCurrentUserPropertyValue(
                    localStorageAccessConstant.groupId,
                    group.oid
                );

                this.sharedService.setCurrentUserPropertyValue(
                    localStorageAccessConstant.impersonatedrole,
                    UserRole.GroupAdmin
                );

                this.sharedService.emitChange(accountTypeChangedEvent);

                this.router.navigate(['/default-layout/fbos/']);
            });
        }
    }

    mergeGroups() {
        const dialogRef = this.mergeGroupsDialog.open(
            GroupsMergeDialogComponent,
            {
                data: {
                    groups: this.selection.selected,
                },
                width: '450px',
            }
        );
        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.selection.clear();

            this.groupsFbosData.groups = this.groupsFbosData.groups.filter(
                (group) =>
                    !result.groups.find(
                        (bg) =>
                            bg.oid !== result.baseGroupId &&
                            bg.oid === group.oid
                    )
            );
            this.groupDataSource = this.groupsFbosData.groups;

            this.dataSource.data = this.groupDataSource;
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

        const {filteredFbos, filteredGroups} = this.filterAccountTypeAndStatus();

        if (!filterValue) {
            this.groupDataSource = filteredGroups;
            this.dataSource.data = this.groupDataSource;
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

        this.groupDataSource = secondFilteredGroups;

        this.dataSource.data = this.groupDataSource;
    }
    private filterAccountTypeAndStatus() {
        let filteredFbos = this.groupsFbosData.fbos.filter(
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
        let filteredGroups = this.groupsFbosData.groups.filter(
            (group) =>
                (this.fboAccountType == "freemium" || this.fboAccountType == "premium") ? ((
                    (this.fboAccountType == "freemium" && !group.hasPremiumFbos) || (this.fboAccountType == "premium" && group.hasPremiumFbos))
                    && group.fboCount > 0 && (this.fboActiveAccountType == "all" || ((this.fboActiveAccountType == "active" && group.hasActiveFbos) || this.fboActiveAccountType == "inactive" && !group.hasActiveFbos))) :
                (this.groupAccountType === 'all' ||
                  (this.groupAccountType === 'active' && group.active) ||
                (this.groupAccountType == 'inactive' && !group.active))
        );
        return {filteredFbos, filteredGroups};
    }
    editGroup(group: any) {
        this.editGroupClicked.emit({
            group,
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

            this.setTableColumns();

            this.refreshSort(this.sort, this.columns);

            this.saveSettings(this.tableLocalStorageKey, this.columns);
        });
    }

    filterChanged() {
        this.applyFilter(this.searchValue);
    }
    isValidPricing(data: any) {
        return data.expiredFboPricingCount === 0 && data.activeFboCount > 0;
    }
    getGroupFbos(groupId: number) {
        return this.fboDataSourceDictionary[groupId];
    }

    isNetworkFbo(groupId: number) {
        return this.manageFboGroupsService.isNetworkFbo(
            this.groupsFbosData,
            groupId
        );
    }

    //todo: move to utils service
    private ifStringContains(str1: string, str2: string) {
        return (!str1 ? '' : str1)
            .toLowerCase()
            .includes((!str2 ? '' : str2).toLowerCase());
    }

    /** Whether the number of selected elements matches the total number of rows. */
    isAllSelected() {
        const numSelected = this.selection.selected.length;
        const numRows = this.dataSource.data.length;
        return numSelected === numRows;
    }

    /** Selects all rows if they are not all selected; otherwise clear selection. */
    masterToggle() {
        if (this.isAllSelected()) {
            this.selection.clear();
            return;
        }

        this.selection.select(...this.dataSource.data);
    }
    /** The label for the checkbox on the passed row */
    checkboxLabel(row?: any): string {
        if (!row) {
            return `${this.isAllSelected() ? 'deselect' : 'select'} all`;
        }
        return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${
            row.position + 1
        }`;
    }
}
