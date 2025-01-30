import { Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DefaultUrlSerializer, Router } from '@angular/router';
import { CustomersListType } from 'src/app/models';
import { Aircraftwatch } from 'src/app/models/flight-watch';
import { CustomerinfobygroupService } from 'src/app/services/customerinfobygroup.service';
import { AircraftAssignModalComponent, NewCustomerAircraftDialogData } from 'src/app/shared/components/aircraft-assign-modal/aircraft-assign-modal.component';
import { FlightWatchMapSharedService } from '../services/flight-watch-map-shared.service';
import { SharedService } from 'src/app/layouts/shared-service';
import { JetNetInformationComponent } from '../../../shared/components/jetnet-information/jetnet-information.component';
import { JetNetService } from '../../../services/jetnet.service';
import { JetNet } from '../../../models/jetnet-information';
import { NullOrEmptyToDefault } from 'src/app/shared/pipes/null/NullOrEmptyToDefault.pipe';
import { defaultStringsEnum } from 'src/app/enums/strings.enums';

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
      destination: '',
      isJetNetEnabled: false
  };
  public hasAircraft = false;
  public customers: CustomersListType[] = []
  public selectedFboId: number;
    public selectedGroupId: number;
    private newChanges: any;
    public hasJetNetInformation: boolean = false;
    public isJetNetOpened: boolean = false;

    constructor(
        private newCustomerAircraftDialog: MatDialog,
        private customerInfoByGroupService: CustomerinfobygroupService,
        private router: Router,
        private flightWatchMapSharedService: FlightWatchMapSharedService,
        private sharedService: SharedService,
        private jetNetInformationDialog: MatDialog,
        private jetNetService: JetNetService,
        private nullOrEmptyToDefault: NullOrEmptyToDefault
    ) { }
    ngOnChanges(changes) {
        if (changes.flightData?.currentValue != undefined && changes.flightData?.currentValue.atcFlightNumber != this.newChanges) {
            this.isLoading = true;
            this.newChanges = changes.flightData.currentValue.atcFlightNumber;

            if (changes.flightData?.currentValue) {
                this.aircraftWatch = changes.flightData.currentValue;

                if (changes.flightData?.currentValue?.flightDepartment) {
                    this.hasAircraft = true;
                    this.isLoading = false;
                }
                else
                    this.getJetNetCustomerName();
            }
        }
  }
  ngOnInit(){
    if(this.fboId && this.groupId)
    this.getCustomersList(this.groupId,this.fboId);
  }

    get isJetNetIntegrationEnabled() {
        return this.sharedService.currentUser.isJetNetIntegrationEnabled;
    }
    get isCustomerManagerButtonDisabled() {
        return !this.hasAircraft || this.sharedService.isCsr || this.aircraftWatch?.customerInfoByGroupId == null || this.aircraftWatch?.customerInfoByGroupId == 0;
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

    get companyDisplayText() {
        const defaultstring = this.nullOrEmptyToDefault.transform(this.aircraftWatch?.flightDepartment,defaultStringsEnum.unknown);
        return !this.isJetNetIntegrationEnabled
            ? defaultstring
            : (!this.hasJetNetInformation)
                ? this.aircraftWatch?.flightDepartment ?? 'Info N/A on JETNET' 
                : defaultstring
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
    goToCustomerManager(customerInfoByGroupId: number): void {
        this.router.navigate([
            './default-layout',
            'customers',
            customerInfoByGroupId,
        ]);
    }

    openJetNetInformation() {
        this.isJetNetOpened = true;
        const dialogRef = this.jetNetInformationDialog.open(
            JetNetInformationComponent,
            {
            width: '1100px',
                data: this.aircraftWatch.tailNumber,
            }
        );
        dialogRef.afterClosed().subscribe((result: any) => {
                this.aircraftWatch.customerInfoByGroupId = result;
                this.isJetNetOpened = false;
            });
    }

    getJetNetCustomerName() {
        if ((this.aircraftWatch?.customerInfoByGroupId == null || this.aircraftWatch?.customerInfoByGroupId == 0) && this.isJetNetIntegrationEnabled && this.aircraftWatch?.tailNumber?.startsWith("N")) {
            try {
                this.jetNetService.getJetNetInformationByTailNumber(this.aircraftWatch?.tailNumber).subscribe((response: JetNet) => {
                    if (response.aircraftresult != null) {
                        this.hasJetNetInformation = true;
                        this.aircraftWatch.flightDepartment = response.aircraftresult.companyrelationships[0].companyname.toUpperCase();
                        this.hasAircraft = true;
                    }
                    else
                        this.hasJetNetInformation = false;
                    this.isLoading = false;
                });
            }
            catch (e) {
                this.hasAircraft = false;
                this.hasJetNetInformation = false;
                this.isLoading = false;
            }
        }
        else {
            this.hasAircraft = false;
            this.isLoading = false;
            this.hasJetNetInformation = false;
        }
    }
}
