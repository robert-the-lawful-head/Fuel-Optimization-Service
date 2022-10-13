import { ThrowStmt } from '@angular/compiler';
import { Component, EventEmitter, Input, Output } from '@angular/core';
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
  @Output() refreshAircraftProperties = new EventEmitter<Aircraftwatch>();

  public aircraftWatch: Aircraftwatch = {
      customerInfoBygGroupId : 0,
      tailNumber: '',
      atcFlightNumber: '',
      aircraftTypeCode: '',
      isAircraftOnGround: false,
      flightDepartment: '',
      aircraftMakeModel: '',
      lastQuote: '',
      currentPricing: '',
      aircraftICAO: ''
  };
  public hasAircraft = false;
  public customers: CustomersListType[] = []
  public selectedFboId: number;
  public selectedGroupId: number;

  constructor(
    private newCustomerAircraftDialog: MatDialog,
    private customerInfoByGroupService: CustomerinfobygroupService,
    private router: Router
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
          console.log(result)
            this.aircraftWatch.aircraftMakeModel = result.aircraftType;
            this.aircraftWatch.flightDepartment = result.company;
            this.refreshAircraftProperties.emit(this.aircraftWatch);
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
  goToCustomerManager(customerInfoBygGroupId: number):void{
    console.log(customerInfoBygGroupId);
    this.router.navigate(['default-layout','customers',customerInfoBygGroupId])
  }
}
