import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import * as _ from 'lodash';
import { CustomersListType } from 'src/app/models';
import { Aircraftwatch, FlightWatch } from 'src/app/models/flight-watch';
import { AirportWatchService } from 'src/app/services/airportwatch.service';
import { CustomerinfobygroupService } from 'src/app/services/customerinfobygroup.service';
import { AircraftAssignModalComponent, NewCustomerAircraftDialogData } from 'src/app/shared/components/aircraft-assign-modal/aircraft-assign-modal.component';

@Component({
  selector: 'app-flight-watch-aircraft-info-dialog',
  templateUrl: './flight-watch-aircraft-info-dialog.component.html',
  styleUrls: ['./flight-watch-aircraft-info-dialog.component.scss']
})
export class FlightWatchAircraftInfoDialogComponent implements OnInit {
  public aircraftWatch: Aircraftwatch = {
      tailNumber: '',
      atcFlightNumber: '',
      aircraftTypeCode: '',
      isAircraftOnGround: false,
      company: '',
      aircraftMakeModel: '',
      lastQuote: '',
      currentPricing: ''
  };
  public isLoading: boolean = true;
  public hasAircraft:  boolean =  true;
  private customers: CustomersListType[] = []

  constructor(public dialogRef: MatDialogRef<FlightWatchAircraftInfoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private newCustomerAircraftDialog: MatDialog,
    private airportWatchService : AirportWatchService,
    private customerInfoByGroupService: CustomerinfobygroupService
  ) { }

  ngOnInit() {
    this.prePopulateAircraftData(this.data.flightWatch);
    this.getCustomersList();
    if(!this.data.flightWatch.tailNumber){
        this.isLoading = false;
        return;
    }
    this.getAircraftWatchData();
  }
  addAircraft() {
    const dialogRef = this.newCustomerAircraftDialog.open<
        AircraftAssignModalComponent,
        Partial<NewCustomerAircraftDialogData>
    >(AircraftAssignModalComponent, {
        data: {
            customers: this.customers,
            tailNumber: this.data.tailNumber
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
  prePopulateAircraftData(flightWatch : FlightWatch){
      this.aircraftWatch.tailNumber = flightWatch.tailNumber;
      this.aircraftWatch.aircraftTypeCode = flightWatch.aircraftTypeCode;
      this.aircraftWatch.isAircraftOnGround = flightWatch.isAircraftOnGround;
      this.aircraftWatch.atcFlightNumber = flightWatch.atcFlightNumber;
  }
  getAircraftWatchData(){
    this.airportWatchService.getAircraftLiveData(this.data.groupId,this.data.fboId,this.data.flightWatch.tailNumber).subscribe((data: Aircraftwatch) => {
          this.aircraftWatch = data;
          if(data.company != null){
            this.hasAircraft = false;
          }
          this.isLoading = false;
      }, (error: any) => {
          console.log(error);
          this.isLoading = false;
      });
  }
  getCustomersList() {
        this.customerInfoByGroupService
            .getCustomersListByGroupAndFbo(
                this.data.groupId,
                this.data.fboId
            )
            .subscribe((customers: any[]) => {
                this.customers = customers;
            });
    }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
