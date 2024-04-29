import { ThrowStmt } from '@angular/compiler';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CustomersListType } from 'src/app/models';
import { Aircraftwatch } from 'src/app/models/flight-watch';
import { CustomerinfobygroupService } from 'src/app/services/customerinfobygroup.service';
import { AircraftAssignModalComponent, NewCustomerAircraftDialogData } from 'src/app/shared/components/aircraft-assign-modal/aircraft-assign-modal.component';
import { FlightWatchMapSharedService } from '../services/flight-watch-map-shared.service';
import { SharedService } from 'src/app/layouts/shared-service';

@Component({
  selector: 'app-aircraft-popup-container',
  templateUrl: './aircraft-popup-container.component.html',
  styleUrls: ['./aircraft-popup-container.component.scss'],
})
export class AircraftPopupContainerComponent {
  @Input() flightData: Aircraftwatch;
  @Input() isLoading: boolean;
  @Input() fboId: any;
  @Input() groupId: any;

  public aircraftWatch: Aircraftwatch = {
      customerInfoByGroupId : 0,
      tailNumber: '',
      atcFlightNumber: '',
      aircraftTypeCode: '',
      isAircraftOnGround: false,
      flightDepartment: '',
      aircraftMakeModel: '',
      lastQuote: '',
      currentPricing: '',
      aircraftICAO: '',
      faaRegisteredOwner: '',
      origin: '',
      destination: ''
  };
  public hasAircraft = false;
  public customers: CustomersListType[] = []
  public selectedFboId: number;
  public selectedGroupId: number;

  constructor(
    private newCustomerAircraftDialog: MatDialog,
    private customerInfoByGroupService: CustomerinfobygroupService,
    private router: Router,
    private flightWatchMapSharedService: FlightWatchMapSharedService,
    private sharedService: SharedService
  ) { }
  ngOnChanges(changes) {
    if(changes.flightData?.currentValue) this.aircraftWatch = changes.flightData.currentValue;
    if(changes.isLoading?.currentValue) this.isLoading = changes.isLoading.currentValue;
    if (changes.flightData?.currentValue?.flightDepartment) this.hasAircraft = true;
    else this.hasAircraft = false;
  }
  ngOnInit(){
    if(this.fboId && this.groupId)
    this.getCustomersList(this.groupId,this.fboId);
  }

  get isCustomerManagerButtonDisabled(){
    return !this.hasAircraft || this.sharedService.isCsr;
  }

  addAircraft() {
    const dialogRef = this.newCustomerAircraftDialog.open<
        AircraftAssignModalComponent,
        Partial<NewCustomerAircraftDialogData>
    >(AircraftAssignModalComponent, {
        data: {
            customers: this.customers,
            tailNumber: this.flightData.tailNumber
        },
        panelClass: 'aircraft-assign-modal',
        width: '450px',
    });
    dialogRef
    .afterClosed()
    .subscribe((result: any) => {
        if (result) {
            this.hasAircraft = true;
            this.aircraftWatch.aircraftMakeModel = result.aircraftType;
            this.aircraftWatch.flightDepartment = result.company;
            this.aircraftWatch.tailNumber = this.flightData.tailNumber;
            this.aircraftWatch.customerInfoByGroupId = result.customerInfoByGroupID;
            this.flightWatchMapSharedService.updateCustomerAicraftData(this.aircraftWatch);
        }
    });
  }
  getCustomersList(groupId,fboId) {
      this.customerInfoByGroupService
          .getCustomersListByGroupAndFbo(
              groupId,
              fboId
          )
          .subscribe((customers: any[]) => {
              this.customers = customers;
          });
  }
  goToCustomerManager(customerInfoByGroupId: number):void{
    this.router.navigate(['./default-layout','customers',customerInfoByGroupId]);
  }
}
