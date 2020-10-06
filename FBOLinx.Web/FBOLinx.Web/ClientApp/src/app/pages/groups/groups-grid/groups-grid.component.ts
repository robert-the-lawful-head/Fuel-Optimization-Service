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
import { NotificationComponent } from '../../../shared/components/notification/notification.component';
import { ManageConfirmationComponent } from '../../../shared/components/manage-confirmation/manage-confirmation.component';
import { fboChangedEvent } from '../../../models/sharedEvents';
import { FbosDialogNewFboComponent } from '../../fbos/fbos-dialog-new-fbo/fbos-dialog-new-fbo.component';

@Component({
  selector: 'app-groups-grid',
  templateUrl: './groups-grid.component.html',
  styleUrls: ['./groups-grid.component.scss'],
  providers: [DetailRowService],
})
export class GroupsGridComponent implements OnInit, AfterViewInit {
  @ViewChild('grid') public grid: GridComponent;
  @ViewChild('fboManageTemplate', { static: true }) public fboManageTemplate: any;
  @ViewChild('needAttentionTemplate', { static: true }) public needAttentionTemplate: any;
  @ViewChild('lastLoginTemplate', { static: true }) public lastLoginTemplate: any;
  @ViewChild('pricingExpiredTemplate', { static: true }) public pricingExpiredTemplate: any;

  // Input/Output Bindings
  @Input() groupsFbosData: any;
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
    private manageDialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

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

    this.needAttentionTemplate.elementRef.nativeElement._viewContainerRef = this.viewContainerRef;
    this.needAttentionTemplate.elementRef.nativeElement.propName = 'template';

    this.lastLoginTemplate.elementRef.nativeElement._viewContainerRef = this.viewContainerRef;
    this.lastLoginTemplate.elementRef.nativeElement.propName = 'template';

    this.pricingExpiredTemplate.elementRef.nativeElement._viewContainerRef = this.viewContainerRef;
    this.pricingExpiredTemplate.elementRef.nativeElement.propName = 'template';
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
          panelClass: ['blue-snackbar'],
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
          panelClass: ['blue-snackbar'],
        });
        this.grid.refresh();
      });
    });
  }

  rowSelected(event) {
    this.editGroupClicked.emit(event.data);
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
            description: 'This will temporarily switch your account to a group admin for this group.  Would you like to continue?'
          },
          autoFocus: false,
        }
      );
      dialogRef.afterClosed().subscribe((result) => {
        if (!result) {
          return;
        }

        this.sharedService.currentUser.managerGroupId = this.sharedService.currentUser.groupId;
        sessionStorage.setItem('managerGroupId', this.sharedService.currentUser.managerGroupId.toString());
        this.sharedService.currentUser.groupId = group.oid;
        sessionStorage.setItem('groupId', this.sharedService.currentUser.groupId.toString());
        this.sharedService.currentUser.impersonatedRole = 2;
        sessionStorage.setItem('impersonatedrole', '2');
        this.router.navigate(['/default-layout/fbos/']);
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
            description: 'This will temporarily switch your account to a primary user for this FBO.  Would you like to continue?'
          },
          autoFocus: false,
        }
      );

      dialogRef.afterClosed().subscribe((result) => {
        if (!result) {
          return;
        }

        this.sharedService.currentUser.managerGroupId = this.sharedService.currentUser.groupId;
        sessionStorage.setItem('managerGroupId', this.sharedService.currentUser.managerGroupId.toString());
        this.sharedService.currentUser.groupId = fbo.groupId;
        sessionStorage.setItem('groupId', this.sharedService.currentUser.groupId.toString());
        this.sharedService.currentUser.impersonatedRole = 1;
        sessionStorage.setItem('impersonatedrole', '1');
        this.sharedService.currentUser.conductorFbo = true;
        sessionStorage.setItem('conductorFbo', 'true');
        this.sharedService.currentUser.fboId = fbo.oid;
        this.sharedService.emitChange(fboChangedEvent);
        this.router.navigate(['/default-layout/dashboard-fbo/']);
      });
    }
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

  allChildFbosPricingLive(groupIndex: number) {
    const group = (this.grid.dataSource as any[])[groupIndex];
    if (!group) {
      return true;
    }
    const fbos: any[] = this.groupsFbosData.fbos.filter(fbo => fbo.groupId === group.oid);
    return fbos.every(fbo => !fbo.pricingExpired);
  }

  fbosPricingExpired(groupIndex: number) {
    const group = (this.grid.dataSource as any[])[groupIndex];
    if (!group) {
      return '';
    }
    const fbos: any[] = this.groupsFbosData.fbos.filter(fbo => fbo.groupId === group.oid);
    const all = fbos.length;
    const expired = fbos.filter(fbo => fbo.pricingExpired).length;
    if (!expired) {
      return 'All Pricing Live';
    }
    return `${expired} of ${all} Expired`;
  }

  ifStringContains(str1: string, str2: string) {
    return str1.toLowerCase().includes(str2.toLowerCase());
  }
}
