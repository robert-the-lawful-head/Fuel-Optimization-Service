import {
    Component,
    EventEmitter,
    Input,
    Output,
    OnInit,
    ViewChild,
    ViewContainerRef,
    AfterViewInit
} from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { DetailRowService, GridComponent, GridModel, RecordClickEventArgs } from '@syncfusion/ej2-angular-grids';

// Services
import { GroupsService } from '../../../services/groups.service';
import { FbosService } from '../../../services/fbos.service';
import { SharedService } from '../../../layouts/shared-service';

// Components
import { GroupsDialogNewGroupComponent } from '../groups-dialog-new-group/groups-dialog-new-group.component';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';
import { FbosGridNewFboDialogComponent } from '../../fbos/fbos-grid-new-fbo-dialog/fbos-grid-new-fbo-dialog.component';
import { NotificationComponent } from '../../../shared/components/notification/notification.component';
import { ManageConfirmationComponent } from '../../../shared/components/manage-confirmation/manage-confirmation.component';
import { fboChangedEvent } from '../../../models/sharedEvents';

@Component({
    selector: 'app-groups-grid',
    templateUrl: './groups-grid.component.html',
    styleUrls: ['./groups-grid.component.scss'],
    providers: [DetailRowService],
})
export class GroupsGridComponent implements OnInit, AfterViewInit {
    @ViewChild('grid') public grid: GridComponent;
    @ViewChild('fboManageTemplate', { static: true }) public fboManageTemplate: any;

    // Input/Output Bindings
    @Input() groupsData: Array<any>;
    @Input() fbosData: Array<any>;
    @Output() editGroupClicked = new EventEmitter<any>();
    @Output() editFboClicked = new EventEmitter<any>();

    childGrid: GridModel;

    // Members
    pageTitle = 'Groups';
    searchValue = '';
    pageSettings: object = {
        pageSizes: [25, 50, 100, 'All'],
        pageSize: 25,
    };

    groupDataSource: any[];
    fboDataSource: any[];

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
        private manageFboDialog: MatDialog,
        private snackBar: MatSnackBar
    ) {}

    ngOnInit() {
        this.groupDataSource = this.groupsData;
        this.fboDataSource = this.fbosData;

        this.sharedService.titleChange(this.pageTitle);
        const self = this;
        this.childGrid = {
            dataSource: this.fboDataSource,
            queryString: 'groupId',
            columns: [
                { field: 'icao', headerText: 'ICAO', width: 100 },
                { field: 'fbo', headerText: 'FBO' },
                { headerText: 'Needs Attention' },
                { headerText: 'Last Login Date' },
                { field: 'active', headerText: 'Active', width: 100 },
                { template: this.fboManageTemplate, width: 150 },
            ],
            load() {
                this.registeredTemplate = {};   // set registertemplate value as empty in load event
                (this as GridComponent).parentDetails.parentKeyFieldValue
                    = ((this as GridComponent).parentDetails.parentRowData as { oid?: string}).oid;
            },
            recordClick(args: RecordClickEventArgs) {
                self.editFboClicked.emit(args.rowData);
            },
        };
    }

    ngAfterViewInit() {
        this.fboManageTemplate.elementRef.nativeElement._viewContainerRef = this.viewContainerRef;
        this.fboManageTemplate.elementRef.nativeElement.propName = 'template';
    }

    deleteGroup(record) {
        const dialogRef = this.deleteGroupDialog.open(
            DeleteConfirmationComponent,
            {
                data: { item: record, fullDescription: 'You are about to remove this group. This will remove the fbos and all the other data related to the group. Are you sure?' },
                autoFocus: false,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            const deleteIndex = this.groupsData.findIndex(group => group.oid === record.oid);
            this.groupsService.remove(record).subscribe(() => {
                this.groupsData.splice(deleteIndex, 1);
                this.snackBar.open(record.groupName + ' is deleted', '', {
                    duration: 2000,
                    panelClass: ['blue-snackbar'],
                });
                this.grid.refresh();
            });
        });
    }

    deleteFbo(record) {
        const dialogRef = this.deleteFboDialog.open(
            DeleteConfirmationComponent,
            {
                data: { item: record, description: 'FBO' },
                autoFocus: false,
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }

            this.fbosService.remove(record).subscribe(() => {
                const fboIndex = this.fbosData.findIndex(fbo => fbo.oid === record.oid);
                this.fbosData.splice(fboIndex, 1);
                this.snackBar.open(record.fbo + ' is deleted', '', {
                    duration: 2000,
                    panelClass: ['blue-snackbar'],
                });
                this.grid.refresh();
            });
        });
    }

    rowSelected(event) {
        this.editGroupClicked.emit(event.data);
    }

    newGroup() {
        const dialogRef = this.newGroupDialog.open(
            GroupsDialogNewGroupComponent,
            {
                width: '450px',
                data: { oid: 0, initialSetupPhase: true },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            result.active = true;
            this.groupsService.add(result).subscribe((data: any) => {
                this.editGroupClicked.emit(data);
            });
        });
    }

    newFbo(event) {
        const parentRow = this.getParentUntil(event.currentTarget, 'e-detailrow');
        const groupRow = parentRow.previousSibling;
        const rowIndex = groupRow.getAttribute('aria-rowindex');
        const dataIndex = this.grid.pageSettings.pageSize * (this.grid.pageSettings.currentPage - 1) + parseInt(rowIndex, 10);
        const rowData = this.grid.dataSource[dataIndex];

        const dialogRef = this.newFboDialog.open(FbosGridNewFboDialogComponent, {
            width: '450px',
            data: { oid: 0, initialSetupPhase: true },
        });

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                result.groupId = rowData.oid;

                this.fbosService.add(result).subscribe((newFbo: any) => {
                    this.snackBar.open(newFbo.fbo + ' is created', '', {
                        duration: 3000,
                        panelClass: ['blue-snackbar'],
                    });
                    this.editFboClicked.emit(newFbo);
                });
            }
        });
    }

    manageFBO(fbo) {
        if (!fbo.active) {
            this.notification.open(
                NotificationComponent,
                {
                    data: {
                        title: 'Inactive fbo',
                        text: 'You can\'t manage an inactive fbo!',
                    },
                }
            );
        } else {
            const dialogRef = this.manageFboDialog.open(
                ManageConfirmationComponent,
                {
                    width: '450px',
                    data: fbo,
                    autoFocus: false,
                }
            );

            dialogRef.afterClosed().subscribe((result) => {
                if (!result) {
                    return;
                }

                this.sharedService.currentUser.impersonatedRole = 1;
                sessionStorage.setItem('impersonatedrole', '1');
                this.sharedService.currentUser.fboId = result.oid;
                sessionStorage.setItem('fboId', this.sharedService.currentUser.fboId.toString());
                this.sharedService.emitChange(fboChangedEvent);
                this.router.navigate(['/default-layout/dashboard-fbo/']);
            });
        }
    }

    getParentUntil(element, className) {
        if (element.classList.contains(className)) {
            return element;
        }
        return this.getParentUntil(element.parentElement, className);
    }

    applyFilter(filterValue: string) {
        this.searchValue = filterValue;

        if (!filterValue || !filterValue.length) {
            return this.groupsData;
        }

        const firstFilteredFbos = this.fbosData.filter(fbo =>
            this.ifStringContains(fbo.icao, filterValue) ||
            this.ifStringContains(fbo.fbo, filterValue)
        );

        const firstFilteredGroups = this.groupsData.filter(group =>
            this.ifStringContains(group.groupName, filterValue)
        );

        const secondFilteredGroups = this.groupsData.filter(group =>
            this.ifStringContains(group.groupName, filterValue) || firstFilteredFbos.find(fbo => fbo.groupId === group.oid)
        );

        const secondFilteredFbos = this.fbosData.filter(fbo =>
            firstFilteredGroups.find(group => group.oid === fbo.groupId) ||
            this.ifStringContains(fbo.icao, filterValue) ||
            this.ifStringContains(fbo.fbo, filterValue)
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
        return this.fbosData.filter(fbo => fbo.groupId === group.oid);
    }

    ifStringContains(str1: string, str2: string) {
        return str1.toLowerCase().includes(str2.toLowerCase());
    }
}
