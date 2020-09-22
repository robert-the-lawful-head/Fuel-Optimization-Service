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
    selector: 'app-groups-grid-new',
    templateUrl: './groups-grid-new.component.html',
    styleUrls: ['./groups-grid-new.component.scss'],
    providers: [DetailRowService],
})
export class GroupsGridNewComponent implements OnInit, AfterViewInit {
    @ViewChild('grid') public grid: GridComponent;
    @ViewChild('fboAddNewTemplate', { static: true }) public fboAddNewTemplate: any;
    @ViewChild('fboManageTemplate', { static: true }) public fboManageTemplate: any;
    @ViewChild('fboDeleteTemplate', { static: true }) public fboDeleteTemplate: any;

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
        this.sharedService.titleChange(this.pageTitle);
        const self = this;
        this.childGrid = {
            dataSource: this.fbosData,
            queryString: 'groupId',
            columns: [
                { field: 'oid', headerText: 'Oid', isPrimaryKey: true, visible: false },
                { field: 'icao', headerText: 'ICAO' },
                { field: 'fbo', headerText: 'FBO' },
                { field: 'active', headerText: 'Status', width: 100 },
                { template: this.fboManageTemplate, width: 150 },
                { template: this.fboDeleteTemplate, headerTemplate: this.fboAddNewTemplate, width: 150 },
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
        this.fboAddNewTemplate.elementRef.nativeElement._viewContainerRef = this.viewContainerRef;
        this.fboAddNewTemplate.elementRef.nativeElement.propName = 'headerTemplate';
        this.fboManageTemplate.elementRef.nativeElement._viewContainerRef = this.viewContainerRef;
        this.fboManageTemplate.elementRef.nativeElement.propName = 'template';
        this.fboDeleteTemplate.elementRef.nativeElement._viewContainerRef = this.viewContainerRef;
        this.fboDeleteTemplate.elementRef.nativeElement.propName = 'template';
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

    public manageFBO(fbo) {
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
        // this.groupsDataSource.filter = filterValue.trim().toLowerCase();
    }
}
