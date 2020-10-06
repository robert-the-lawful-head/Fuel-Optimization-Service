import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

// Services
import { FbosService } from '../../../services/fbos.service';
import { FboairportsService } from '../../../services/fboairports.service';
import { FbocontactsService } from '../../../services/fbocontacts.service';
import { ContactsService } from '../../../services/contacts.service';
import { GroupsService } from '../../../services/groups.service';
import { SharedService } from '../../../layouts/shared-service';

@Component({
    selector: 'app-fbos-edit',
    templateUrl: './fbos-edit.component.html',
    styleUrls: ['./fbos-edit.component.scss'],
})
export class FbosEditComponent implements OnInit {
  @Output() saveClicked = new EventEmitter < any > ();
  @Output() cancelClicked = new EventEmitter < any > ();
  @Input() fboInfo: any;
  @Input() fboAirportInfo: any;
  @Input() groupInfo: any;
  @Input() embed: boolean;

  // Members
  breadcrumb: any[];
  pageTitle = 'Edit FBO';
  currentContact: any;
  contactsData: any;
  groups: Array < any > ;

  // Private Members

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fboService: FbosService,
    private fboAirportsService: FboairportsService,
    private fboContactsService: FbocontactsService,
    private contactsService: ContactsService,
    private groupsService: GroupsService,
    private sharedService: SharedService
  ) {
    this.sharedService.titleChange(this.pageTitle);
  }

  ngOnInit() {
    if (!this.embed) {
      if (this.sharedService.currentUser.role === 3 && !this.sharedService.currentUser.impersonatedRole) {
        this.breadcrumb = [{
            title: 'Main',
            link: '/default-layout',
          },
          {
            title: 'Groups',
            link: '/default-layout/groups',
          },
          {
            title: 'Edit FBO',
            link: '',
          },
        ];
      } else {
        this.breadcrumb = [{
            title: 'Main',
            link: '/default-layout',
          },
          {
            title: 'FBOs',
            link: '/default-layout/fbos',
          },
          {
            title: 'Edit FBO',
            link: '',
          },
        ];
      }
    }

    if (this.fboInfo) {
      this.loadAdditionalFboInfo();
    } else {
      const id = this.route.snapshot.paramMap.get('id');
      this.fboService.get({
        oid: id
      }).subscribe((data: any) => {
        this.fboInfo = data;
        this.loadAdditionalFboInfo();
      });
      this.fboAirportsService
        .getForFbo({
          oid: id
        })
        .subscribe((data: any) => (this.fboAirportInfo = data));
    }
  }

  saveEdit() {
    if (sessionStorage.getItem('isNewFbo')) {
      sessionStorage.removeItem('isNewFbo');
    }
    this.fboAirportInfo.fboId = this.fboInfo.oid;
    this.fboService.update(this.fboInfo).subscribe(() => {
      this.fboAirportsService
        .update(this.fboAirportInfo)
        .subscribe(() => {
          this.saveClicked.emit(this.fboInfo);
          if (this.sharedService.currentUser.role === 3 && !this.sharedService.currentUser.impersonatedRole) {
            this.router.navigate(['/default-layout/groups/']);
          } else {
            this.router.navigate(['/default-layout/fbos/']);
          }
        });
    });
  }

  cancelEdit() {
    if (sessionStorage.getItem('isNewFbo')) {
      this.fboService.remove(this.fboInfo).subscribe(() => {
        sessionStorage.removeItem('isNewFbo');
        this.saveClicked.emit(this.fboInfo);
      });
    } else {
      if (this.sharedService.currentUser.role === 3 && !this.sharedService.currentUser.impersonatedRole) {
        this.router.navigate(['/default-layout/groups/']);
      } else {
        this.router.navigate(['/default-layout/fbos/']);
      }
    }
  }

  contactDeleted(record) {
    this.fboContactsService.remove(record).subscribe(() => {
      this.fboContactsService
        .getForFbo(this.fboInfo)
        .subscribe((data: any) => (this.contactsData = data));
    });
  }

  newContactClicked() {
    this.currentContact = {
      oid: 0,
    };
  }

  editContactClicked(record) {
    this.contactsService
      .get({
        oid: record.contactId
      })
      .subscribe((data: any) => (this.currentContact = data));
  }

  saveEditContactClicked() {
    this.contactsService.update(this.currentContact).subscribe(() => {
      this.currentContact = null;
    });
  }

  cancelEditContactClicked() {
    this.currentContact = null;
  }

  // Private Methods
  private loadAdditionalFboInfo() {
    this.fboContactsService
      .getForFbo(this.fboInfo)
      .subscribe((data: any) => (this.contactsData = data));
    this.groupsService
      .getAllGroups()
      .subscribe((data: any) => (this.groups = data));
  }
}
