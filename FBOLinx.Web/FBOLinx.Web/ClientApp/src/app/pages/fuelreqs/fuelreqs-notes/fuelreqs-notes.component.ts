import {
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    SimpleChanges,
} from '@angular/core';
import { combineLatest, EMPTY, of } from 'rxjs';

import { SharedService } from '../../../layouts/shared-service';
import { CustomerinfobygroupService } from '../../../services/customerinfobygroup.service';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
import { ServiceOrderService } from '../../../services/serviceorder.service';
import { CustomerInfoByGroupNote } from '../../../models/customer-info-by-group-note';
import { CustomerAircraftNote } from '../../../models/customer-aircraft-note';
import { ServiceOrder } from '../../../models/service-order';
import { Router } from '@angular/router';
import { Customer } from '../../../models';
import { CustomerInfoByGroup } from '../../../models/customer-info-by-group';
import { CustomerAircraft } from '../../../models/customer-aircraft';
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { OrderNote } from '../../../models/order-note';
import * as moment from 'moment';
import { DeleteConfirmationComponent } from '../../../shared/components/delete-confirmation/delete-confirmation.component';
import { MatDialog } from '@angular/material/dialog';


@Component({
    //host: {
    //    '[class.addition-navbar]': 'true',
    //    '[class.open]': 'open',
    //},
    selector: 'app-fuelreqs-notes',
    styleUrls: ['./fuelreqs-notes.component.scss'],
    templateUrl: './fuelreqs-notes.component.html',
})
export class FuelreqsNotesComponent implements OnInit {
    @Input() public serviceOrderId: number | null = null;
    @Input() public fuelOrderId: number | null = null;
    @Input() public fuelerlinxTransactionId: number | null = null;
    @Input() public tailNumber: string;
    @Input() public customerId: number;
    @Input() public customer: string;
    @Output() openByDefault: EventEmitter<any> = new EventEmitter();
    public customerNotes: string;
    public customerAircraftNotes: string;
    public serviceOrderNotes: any[];
    public additionalNotes: any[];
    customerInfoByGroupId: number;
    customerAircraft: CustomerAircraft;
    isLoading: boolean = true;
    newAdditionalNote: string;

    constructor(
        private customerInfoByGroupService: CustomerinfobygroupService,
        private customerAircraftsService: CustomeraircraftsService,
        private sharedService: SharedService,
        private serviceOrderService: ServiceOrderService,
        private fuelReqsService: FuelreqsService,
        private router: Router,
        private dialog: MatDialog,
    ) {
        
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.serviceOrderId || changes.fuelOrderId || changes.fuelerlinxTransactionId) {
            this.loadNotes();
        }
    }

    ngOnInit() {
    }

    goToCustomer(): void {
        this.router.navigate(['/default-layout/customers/' + this.customerInfoByGroupId]);
    }

    goToCustomerAircraft(): void {
        this.router.navigate(['/default-layout/customers/' + this.customerInfoByGroupId], { queryParams: { tab: 1, search: this.tailNumber } });
    }

    addAdditionalNoteClicked(): void {
        var newAdditionalNote: OrderNote = { oid: 0, note: this.newAdditionalNote, associatedFuelOrderId: this.fuelOrderId == null ? 0 : this.fuelOrderId, associatedServiceOrderId: this.serviceOrderId == null ? 0 : this.serviceOrderId, associatedFuelerLinxTransactionId: this.fuelerlinxTransactionId == null ? 0 : this.fuelerlinxTransactionId, dateAdded: moment().utc().toDate(), addedByUserId: this.sharedService.currentUser.oid, addedByName: this.sharedService.currentUser.firstName + " " + this.sharedService.currentUser.lastName, timeZone: '' };
        this.fuelReqsService.addAdditionalNotes(newAdditionalNote).subscribe((response: any) => {

        });

        this.additionalNotes.push(newAdditionalNote);
        this.newAdditionalNote = "";
    }

    editAdditionalNoteClicked(additionalNote: OrderNote): void {
        additionalNote.isEdit = true;
    }

    saveEditAdditionalNoteClicked(additionalNote: OrderNote): void {
        delete additionalNote.isEdit;
        this.fuelReqsService.editAdditionalNote(additionalNote).subscribe((response: any) => {
            additionalNote.isEdit = false;
        });
    }

    deleteAdditionalNoteClicked(additionalNote: OrderNote): void {
        const dialogRef = this.dialog.open(
            DeleteConfirmationComponent,
            {
                autoFocus: false,
                data: { description: 'note', item: additionalNote, includeThis: true },
            }
        );

        dialogRef.afterClosed().subscribe((additionalNote) => {
            if (!additionalNote) {
                return;
            }
            this.fuelReqsService.deleteAdditionalNote(additionalNote.item).subscribe((response: any) => {
                this.additionalNotes.splice(this.additionalNotes.indexOf(additionalNote.item), 1);
            });
        });
    }

    private async loadNotes() {
        const results = await combineLatest([
            //0
            this.customerInfoByGroupService.getCustomerInfoByGroupNoteByCustomerIdGroupId(this.customerId, this.sharedService.currentUser.groupId),
            //1
            this.customerAircraftsService.getCustomerAircraftNotesByTailNumberGroupIdCustomerId(
                this.tailNumber,
                this.sharedService.currentUser.groupId,
                this.customerId
            ),
            //2
            this.serviceOrderService.getServiceOrderById(this.serviceOrderId),
            //3
            this.fuelReqsService.getAdditionalOrderNotes(this.fuelOrderId, this.serviceOrderId, this.fuelerlinxTransactionId),
            //4
            this.customerInfoByGroupService.getCustomerInfoByGroupAndCustomerId(this.sharedService.currentUser.groupId, this.customerId)
        ]).toPromise();

        //0
        var customerNotes = results[0] as CustomerInfoByGroupNote;
        //1
        var customerAircraftNotes = results[1] as CustomerAircraftNote[];
        //2
        var serviceOrderResults = results[2] as any;
        this.additionalNotes = results[3] as any[];
        //4
        var customer = results[4] as CustomerInfoByGroup;

        if (customerNotes != null && customerNotes.notes != "")
            this.customerNotes = customerNotes.notes;
        else
            this.customerNotes = "";

        if (customerAircraftNotes.length > 0)
            this.customerAircraftNotes = customerAircraftNotes[0].notes;
        else
            this.customerAircraftNotes = "";

        var serviceOrder = serviceOrderResults.result as ServiceOrder;
        var serviceOrderNotes: any[] = [];
        if (serviceOrder != null) {
            serviceOrder.serviceOrderItems.forEach((serviceOrderItem) => {
                if (serviceOrderItem.serviceNote != null && serviceOrderItem.serviceNote != "")
                    serviceOrderNotes.push(serviceOrderItem.serviceName + ": " + serviceOrderItem.serviceNote);
            });
        }
        this.serviceOrderNotes = serviceOrderNotes;

        this.customerInfoByGroupId = customer.oid;

        this.isLoading = false;

        if (this.customerNotes || this.customerAircraftNotes || this.serviceOrderNotes.length > 0 || this.additionalNotes.length > 0)
            this.openByDefault.emit(true);
        else
            this.openByDefault.emit(false);
    }
}
