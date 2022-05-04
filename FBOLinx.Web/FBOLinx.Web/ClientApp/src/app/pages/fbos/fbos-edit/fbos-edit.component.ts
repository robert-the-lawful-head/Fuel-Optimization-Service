import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSelectChange } from '@angular/material/select';

import { SharedService } from '../../../layouts/shared-service';
import { ContactsService } from '../../../services/contacts.service';
import { FboairportsService } from '../../../services/fboairports.service';
import { FbocontactsService } from '../../../services/fbocontacts.service';
// Services
import { FbosService } from '../../../services/fbos.service';
import { GroupsService } from '../../../services/groups.service';
import { AirportWatchService } from '../../../services/airportwatch.service';
import {
    CloseConfirmationComponent,
    CloseConfirmationData,
} from '../../../shared/components/close-confirmation/close-confirmation.component';

@Component({
    selector: 'app-fbos-edit',
    styleUrls: ['./fbos-edit.component.scss'],
    templateUrl: './fbos-edit.component.html',
})
export class FbosEditComponent implements OnInit {
    @Input() fboInfo: any;
    @Input() fboAirportInfo: any;
    @Input() groupInfo: any;
    @Input() embed: boolean;
    @Output() saveClicked = new EventEmitter<any>();
    @Output() cancelClicked = new EventEmitter<any>();

    // Members
    breadcrumb: any[];
    pageTitle = 'Edit FBO';
    currentContact: any;
    contactsData: any;
    groups: Array<any>;
    availableAntennas: any[];

    // Private Members

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private fboService: FbosService,
        private fboAirportsService: FboairportsService,
        private fboContactsService: FbocontactsService,
        private contactsService: ContactsService,
        private groupsService: GroupsService,
        private sharedService: SharedService,
        private confirmDialog: MatDialog,
        private airportWatchService: AirportWatchService
    ) {
        this.sharedService.titleChange(this.pageTitle);
    }

    get canChangeActive() {
        return (
            !this.embed &&
            this.sharedService.currentUser.role === 3 &&
            !this.sharedService.currentUser.impersonatedRole
        );
    }

    ngOnInit() {
        if (!this.embed) {
            if (
                this.sharedService.currentUser.role === 3 &&
                !this.sharedService.currentUser.impersonatedRole
            ) {
                this.breadcrumb = [
                    {
                        link: '/default-layout',
                        title: 'Main',
                    },
                    {
                        link: '/default-layout/groups',
                        title: 'Groups',
                    },
                    {
                        link: '',
                        title: 'Edit FBO',
                    },
                ];
            } else {
                this.breadcrumb = [
                    {
                        link: '/default-layout',
                        title: 'Main',
                    },
                    {
                        link: '/default-layout/fbos',
                        title: 'FBOs',
                    },
                    {
                        link: '',
                        title: 'Edit FBO',
                    },
                ];
            }
        }

        if (this.fboInfo) {
            this.loadAdditionalFboInfo();
        } else {
            const id = this.route.snapshot.paramMap.get('id');
            this.fboService
                .get({
                    oid: id,
                })
                .subscribe((data: any) => {
                    this.fboInfo = data;
                    this.loadAdditionalFboInfo();
                });
            this.fboAirportsService
                .getForFbo({
                    oid: id,
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
                    this.navigateToParent();
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
            this.cancelClicked.emit();
            this.navigateToParent();
        }
    }

    navigateToParent() {
        if (
            this.sharedService.currentUser.role === 3 &&
            !this.sharedService.currentUser.impersonatedRole
        ) {
            if (this.groupInfo) {
                this.router.navigate([
                    '/default-layout/groups/' + this.groupInfo.oid,
                ]);
            } else {
                this.router.navigate(['/default-layout/groups/']);
            }
        } else {
            this.router.navigate(['/default-layout/fbos/']);
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
                oid: record.contactId,
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

    onActiveToggle(event) {
        if (event.checked && this.fboInfo.expirationDate) {
            this.confirmDialog
                .open(CloseConfirmationComponent, {
                    data: {
                        cancel: 'Cancel',
                        customText:
                            'Account Expiry date is set. If you activate this account, it will be removed.',
                        customTitle: 'Account Expired!',
                        ok: 'Set Active',
                    } as CloseConfirmationData,
                })
                .afterClosed()
                .subscribe((confirmed) => {
                    if (!confirmed) {
                        this.fboInfo.active = false;
                        return;
                    }

                    this.fboInfo.expirationDate = null;
                });
        }
    }

    get isConductor() {
        return this.sharedService.currentUser.role === 3;
    }

    // Private Methods
    private loadAdditionalFboInfo() {
        this.fboContactsService
            .getForFbo(this.fboInfo)
            .subscribe((data: any) => (this.contactsData = data));
        this.groupsService
            .getAllGroups()
            .subscribe((data: any) => (this.groups = data));
        this.airportWatchService
            .getUnassignedAntennaBoxes(this.fboInfo.antennaName)
            .subscribe((data: any) => (this.availableAntennas = data));
    }
}
