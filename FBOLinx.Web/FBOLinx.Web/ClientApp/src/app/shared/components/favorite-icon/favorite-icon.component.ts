import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SharedService } from 'src/app/layouts/shared-service';
import { FavoritesService } from 'src/app/services/favorites.service';
import { SnackBarService } from 'src/app/services/utils/snackBar.service';
import { AircraftAssignModalComponent, NewCustomerAircraftDialogData } from '../aircraft-assign-modal/aircraft-assign-modal.component';
import { MatDialog } from '@angular/material/dialog';
import { Aircraftwatch } from 'src/app/models/flight-watch';
import { FlightWatchMapSharedService } from 'src/app/pages/flight-watch/services/flight-watch-map-shared.service';
export enum CallbackComponent {
    aircraft,
    Company,
    customCallback
}
@Component({
  selector: 'app-favorite-icon',
  templateUrl: './favorite-icon.component.html',
  styleUrls: ['./favorite-icon.component.scss']
})
export class FavoriteIconComponent implements OnInit {
    @Input() favoriteData: any;
    @Input() callbackComponent: CallbackComponent = CallbackComponent.customCallback;
    @Input() isSaving: boolean = false;
    @Input() hasPadding: boolean = true;
    @Input() customers: [] = [];
    @Output() favoriteClick = new EventEmitter<any>();

    isFavorite = false;

    constructor(private favoritesService: FavoritesService,
        private sharedService: SharedService,
        private snackbarService: SnackBarService,
        private newCustomerAircraftDialog: MatDialog,
        private flightWatchMapSharedService: FlightWatchMapSharedService
        ) { }

    ngOnInit() {
        this.isFavorite = this.favoriteData.isFavorite;
    }

    setIsFavoriteProperty(aircraft: any): any {
        aircraft.isFavorite = aircraft.favoriteAircraft != null;
        return aircraft;
    }
    async toogleFavorite(): Promise<void>{
        if(this.isSaving) return;

        this.isFavorite = !this.isFavorite;
        this.favoriteData.isFavorite = this.isFavorite;

        switch(this.callbackComponent) {
            case CallbackComponent.aircraft: {
               await this.aircraftFavoriteClickAction(this.favoriteData);
               break;
            }
            case CallbackComponent.Company: {
               this.companyFavoriteClickAction(this.favoriteData);
               break;
            }
            default: {
                this.favoriteClick.emit(this.favoriteData);
                break;
            }
         }

    }
    private hasCustomer(): boolean {
        return this.callbackComponent == CallbackComponent.aircraft && this.favoriteData.isCustomerManagerAircraft;
    }
    private async aircraftFavoriteClickAction(favoriteData: any): Promise<void> {
        if(!this.hasCustomer()){
            await this.openAddCustomerToAicraftDialog();
        }
        else
            this.toogleAicraftFavorite(favoriteData);
    }
    private toogleAicraftFavorite(favoriteData: any): void {
        if(favoriteData.isFavorite)
        this.favoritesService.saveAircraftFavorite(this.sharedService.currentUser.fboId, favoriteData.customerAircraftId, this.sharedService.currentUser.groupId)
        .subscribe(
            (data: any) => {
               favoriteData.favoriteAircraft = data;
            },
            (error: any) => {
                console.log(error);
                this.snackbarService.showErrorSnackBar("Error adding aircraft to favorites");
                favoriteData.favoriteAircraft = null;
                favoriteData.isFavorite = false;
                this.isFavorite = false;
            }
        );
        else
        this.favoritesService.deleteAircraftFavorite(favoriteData.favoriteAircraft.oid, this.sharedService.currentUser.groupId).subscribe(
            (data: any) => {
               favoriteData.favoriteAircraft = null;
            },
            (error: any) => {
                console.log(error);
                this.snackbarService.showErrorSnackBar("Error removing aircraft from favorites");
            }
        );
    }
    private  async openAddCustomerToAicraftDialog(): Promise<void>{
        let dialogRef = this.newCustomerAircraftDialog.open<
            AircraftAssignModalComponent,
            Partial<NewCustomerAircraftDialogData>
        >(AircraftAssignModalComponent, {
            data: {
                customers: this.customers,
                tailNumber: this.favoriteData.tailNumber
            },
            panelClass: 'aircraft-assign-modal',
            width: '450px',
        });
        let result = await dialogRef
        .afterClosed().toPromise();
        if (result) {
            let aircraftWatch: Aircraftwatch = {
                customerInfoByGroupId : result.customerInfoByGroupID,
                tailNumber: this.favoriteData.tailNumber,
                atcFlightNumber: this.favoriteData.atcFlightNumber,
                aircraftTypeCode: this.favoriteData.aircraftTypeCode,
                isAircraftOnGround: false,
                flightDepartment: result.company,
                aircraftMakeModel: result.aircraftType,
                lastQuote: this.favoriteData.lastQuote,
                currentPricing: this.favoriteData.currentPricing,
                aircraftICAO: this.favoriteData.aircraftICAO,
                faaRegisteredOwner: this.favoriteData.faaRegisteredOwner,
                origin: this.favoriteData.origin,
                destination: this.favoriteData.destination,
                isJetNetEnabled: false
            };
            this.favoriteData.isFavorite = this.isFavorite;
            this.favoriteData.isCustomerManagerAircraft = true;
            this.favoriteData.customerInfoByGroupId = result.customerInfoByGroupID;
            this.favoriteData.customerAircraftId = result.customerAircraftId;
            this.flightWatchMapSharedService.updateCustomerAicraftData(aircraftWatch);
            this.toogleAicraftFavorite(this.favoriteData);
        }else{
            this.isFavorite = !this.isFavorite;
            this.favoriteData.isFavorite = this.isFavorite;
        }
        dialogRef = null;
    }
    private companyFavoriteClickAction(favoriteData: any): void {
        if(favoriteData.isFavorite)
            this.favoritesService.saveCompanyFavorite(this.sharedService.currentUser.fboId, favoriteData.customerInfoByGroupId)
            .subscribe(
                (data: any) => {
                   favoriteData.favoriteCompany = data;
                   this.favoriteClick.emit(this.favoriteData);
                },
                (error: any) => {
                    console.log(error);
                    this.snackbarService.showErrorSnackBar("Error adding company to favorites");
                    favoriteData.isFavorite = false;
                    this.isFavorite = false;
                }
            );
        else
            this.favoritesService.deleteCompanyFavorite(favoriteData.favoriteCompany.oid)
            .subscribe(
                (data: any) => {
                   favoriteData.favoriteCompany = null;
                   this.favoriteClick.emit(this.favoriteData);
                },
                (error: any) => {
                    console.log(error)
                    this.snackbarService.showErrorSnackBar("Error removing company from favorites");
                    favoriteData.isFavorite = true;
                    this.isFavorite = true;
                }
            );
    }
}
