import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SharedService } from 'src/app/layouts/shared-service';
import { FavoritesService } from 'src/app/services/favorites.service';
import { SnackBarService } from 'src/app/services/utils/snackBar.service';

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
    @Output() favoriteClick = new EventEmitter<any>();

    isFavorite = false;

    constructor(private favoritesService: FavoritesService,
        private sharedService: SharedService,
        private snackbarService: SnackBarService) { }

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
               this.aircraftFavoriteClickAction(this.favoriteData);
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
    private aircraftFavoriteClickAction(favoriteData: any): void {
        if(favoriteData.isFavorite)
        this.favoritesService.saveAircraftFavorite(this.sharedService.currentUser.fboId, favoriteData.customerAircraftId)
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
        this.favoritesService.deleteAircraftFavorite(favoriteData.favoriteAircraft.oid).subscribe(
            (data: any) => {
               favoriteData.favoriteAircraft = null;
            },
            (error: any) => {
                console.log(error);
                this.snackbarService.showErrorSnackBar("Error removing aircraft from favorites");
                favoriteData.isFavorite = true;
                this.isFavorite = true;
            }
        );
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
