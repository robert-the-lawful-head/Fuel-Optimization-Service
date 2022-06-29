import { Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CustomersListType } from 'src/app/models';
import { Aircraftwatch } from 'src/app/models/flight-watch';
import { CustomerinfobygroupService } from 'src/app/services/customerinfobygroup.service';
import { AircraftAssignModalComponent, NewCustomerAircraftDialogData } from 'src/app/shared/components/aircraft-assign-modal/aircraft-assign-modal.component';

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
      customerInfoBygGroupId : 0,
      tailNumber: '',
      atcFlightNumber: '',
      aircraftTypeCode: '',
      isAircraftOnGround: false,
      company: '',
      aircraftMakeModel: '',
      lastQuote: '',
      currentPricing: ''
  };
  public hasAircraft = false;
  private customers: CustomersListType[] = []

  constructor(
    private newCustomerAircraftDialog: MatDialog,
    private customerInfoByGroupService: CustomerinfobygroupService,
    private router: Router
  ) { }
  ngOnChanges(changes) {
    console.log("🚀 ~ file: aircraft-popup-container.component.ts ~ line 47 ~ AircraftPopupContainerComponent ~ ngOnChanges ~ changes", changes)
    console.log("🚀 ~ file: aircraft-popup-container.component.ts ~ line 45 ~ AircraftPopupContainerComponent ~ ngOnChanges ~ changes.flightData.previousValue?.oid", changes.flightData)

    console.log("🚀 ~ file: aircraft-popup-container.component.ts ~ line 46 ~ AircraftPopupContainerComponent ~ ngOnChanges ~ changes.flightData.currentValue.company", changes.flightData?.currentValue?.company)

    if(changes.flightData?.currentValue) this.aircraftWatch = changes.flightData.currentValue;  
    if(changes.isLoading?.currentValue) this.isLoading = changes.isLoading.currentValue; 
    if(changes.hasAircraft?.currentValue) this.hasAircraft = changes.hasAircraft.currentValue;    
    if(changes.aircraftData?.currentValue?.company) this.hasAircraft = true;
    else this.hasAircraft = false;   
  }
  ngOnInit() {
    this.getCustomersList();
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
            this.aircraftWatch.aircraftTypeCode = result.aircraftType
            this.aircraftWatch.company = result.company
        }
    });
  }
  getCustomersList() {
      this.customerInfoByGroupService
          .getCustomersListByGroupAndFbo(
              this.groupId,
              this.fboId
          )
          .subscribe((customers: any[]) => {
              this.customers = customers;
          });
  }
  goToCustomerManager(customerInfoBygGroupId: number):void{
    console.log(customerInfoBygGroupId);
    this.router.navigate(['default-layout','customers',customerInfoBygGroupId])
  } 
}
