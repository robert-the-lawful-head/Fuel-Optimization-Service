import { DragDropModule } from '@angular/cdk/drag-drop';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatLegacyAutocompleteModule as MatAutocompleteModule } from '@angular/material/legacy-autocomplete';
import { MatBadgeModule } from '@angular/material/badge';
import { MatLegacyButtonModule as MatButtonModule } from '@angular/material/legacy-button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatLegacyCardModule as MatCardModule } from '@angular/material/legacy-card';
import { MatLegacyCheckboxModule as MatCheckboxModule } from '@angular/material/legacy-checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatLegacyDialogModule as MatDialogModule } from '@angular/material/legacy-dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatLegacyFormFieldModule as MatFormFieldModule } from '@angular/material/legacy-form-field';
// Angular Material Modules
import { MatIconModule } from '@angular/material/icon';
import { MatLegacyInputModule as MatInputModule } from '@angular/material/legacy-input';
import { MatLegacyPaginatorModule as MatPaginatorModule } from '@angular/material/legacy-paginator';
import { MatLegacyProgressBarModule as MatProgressBarModule } from '@angular/material/legacy-progress-bar';
import { MatLegacyProgressSpinnerModule as MatProgressSpinnerModule } from '@angular/material/legacy-progress-spinner';
import { MatLegacySelectModule as MatSelectModule } from '@angular/material/legacy-select';
import { MatLegacySlideToggleModule as MatSlideToggleModule } from '@angular/material/legacy-slide-toggle';
import { MatLegacySnackBarModule as MatSnackBarModule } from '@angular/material/legacy-snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatStepperModule } from '@angular/material/stepper';
import { MatLegacyTableModule as MatTableModule } from '@angular/material/legacy-table';
import { MatLegacyTabsModule as MatTabsModule } from '@angular/material/legacy-tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatLegacyTooltipModule as MatTooltipModule } from '@angular/material/legacy-tooltip';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { PopoverModule } from 'ngx-smart-popover';
import { NgxUiLoaderModule } from 'ngx-ui-loader';
import {
    NgxMatDatetimePickerModule,
    NgxMatNativeDateModule,
    NgxMatTimepickerModule
} from '@angular-material-components/datetime-picker';

import { environment } from '../environments/environment';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { JwtInterceptor } from './helpers';
import { DefaultLayoutComponent } from './layouts/default/default.component';
import { LandingSiteLayoutComponent } from './layouts/landing-site/landing-site.component';
import { OutsideTheGateLayoutComponent } from './layouts/outside-the-gate/outside-the-gate.component';
import { SharedService } from './layouts/shared-service';
import { NiComponentsModule } from './ni-components/ni-components.module';
import { PagesModule } from './pages/pages.module';
// Services
import { AcukwikairportsService } from './services/acukwikairports.service';
import { AircraftpricesService } from './services/aircraftprices.service';
import { AircraftsService } from './services/aircrafts.service';
import { AirportWatchService } from './services/airportwatch.service';
import { AppService } from './services/app.service';
import { ContactinfobyfboService } from './services/contactinfobyfbo.service';
import { ContactinfobygroupsService } from './services/contactinfobygroups.service';
import { ContactsService } from './services/contacts.service';
import { CustomcustomertypesService } from './services/customcustomertypes.service';
import { CustomerCompanyTypesService } from './services/customer-company-types.service';
import { CustomeraircraftsService } from './services/customeraircrafts.service';
import { CustomercontactsService } from './services/customercontacts.service';
import { CustomerinfobygroupService } from './services/customerinfobygroup.service';
import { CustomermarginsService } from './services/customermargins.service';
import { CustomersService } from './services/customers.service';
import { CustomersviewedbyfboService } from './services/customersviewedbyfbo.service';
import { DistributionService } from './services/distribution.service';
import { EmailcontentService } from './services/emailcontent.service';
import { FboairportsService } from './services/fboairports.service';
import { FbocontactsService } from './services/fbocontacts.service';
import { FbofeeandtaxomitsbycustomerService } from './services/fbofeeandtaxomitsbycustomer.service';
import { FbofeeandtaxomitsbypricingtemplateService } from './services/fbofeeandtaxomitsbypricingtemplate.service';
import { FbofeesandtaxesService } from './services/fbofeesandtaxes.service';
import { FbomissedquoteslogService } from './services/fbomissedquoteslog.service';
import { FbopreferencesService } from './services/fbopreferences.service';
import { FbopricesService } from './services/fboprices.service';
import { FbosService } from './services/fbos.service';
import { FuelreqsService } from './services/fuelreqs.service';
import { GroupsService } from './services/groups.service';
import { LandingsiteService } from './services/landingsite.service';
import { Parametri } from './services/paremeters.service';
import { PricetiersService } from './services/pricetiers.service';
import { PricingtemplatesService } from './services/pricingtemplates.service';
import { RampfeesService } from './services/rampfees.service';
import { RampfeesettingsService } from './services/rampfeesettings.service';
import { TemporaryAddOnMarginService } from './services/temporaryaddonmargin.service';
import { UserService } from './services/user.service';
import { LoginModalComponent } from './shared/components/login-modal/login-modal.component';
import { RequestDemoModalComponent } from './shared/components/request-demo-modal/request-demo-modal.component';
import { RequestDemoSuccessComponent } from './shared/components/request-demo-success/request-demo-success.component';
import { metaReducers, reducers } from './store/reducers';
import { UIModule } from './ui/ui.module';
import { TagsService } from './services/tags.service';
import { AssociationsService } from './services/associations.service';
import { AirportFboGeofenceClustersService } from './services/airportfbogeofenceclusters.service';
import { AirportFboGeofenceClusterCoordinatesService } from './services/airportfbogeofenceclustercoordinates.service';
import { DateTimeService } from './services/datetime.service';
import { SwimService } from './services/swim.service';
import { FlightWatchService } from './services/flightwatch.service';
import { FlightWatchHelper } from './pages/flight-watch/FlightWatchHelper.service';
import { PublicViewComponent } from './layouts/public-view/public-view.component';
import { FileHelper } from './helpers/files/file.helper';
import { PricingTemplateCalcService } from './pages/pricing-templates/pricingTemplateCalc.service';
import { DocumentService } from './services/documents.service';
import { ServiceOrderService } from './services/serviceorder.service';
import { ServicesAndFeesService } from './services/servicesandfees.service';
import { ServiceTypeService } from './services/servicetypes.service';
import { SnackBarService } from './services/utils/snackBar.service';
import { FavoritesService } from './services/favorites.service';
import { FormValidationHelperService } from './helpers/forms/formValidationHelper.service';
import { FlightWatchMapSharedService } from './pages/flight-watch/services/flight-watch-map-shared.service';
import { ManageFboGroupsService } from './services/managefbo.service';
import { StringHelperService } from './helpers/strings/stringHelper.service';
import {JetNetService} from './services/jetnet.service';
import { HttpErrorInterceptor } from './interceptors/http-error.interceptor';
import { AuthenticationService } from './services/security/authentication.service';
import { NgxMaskModule } from 'ngx-mask';
@NgModule({
    bootstrap: [AppComponent],
    declarations: [
        AppComponent,
        DefaultLayoutComponent,
        PublicViewComponent,
        LandingSiteLayoutComponent,
        OutsideTheGateLayoutComponent,
        LoginModalComponent,
        RequestDemoModalComponent,
        RequestDemoSuccessComponent,
    ],
    imports: [
        BrowserAnimationsModule,
        BrowserModule,
        FormsModule,
        DragDropModule,
        ReactiveFormsModule,
        MatBadgeModule,
        MatIconModule,
        MatAutocompleteModule,
        MatButtonModule,
        MatButtonToggleModule,
        MatCardModule,
        MatCheckboxModule,
        MatDatepickerModule,
        MatDialogModule,
        MatDividerModule,
        MatExpansionModule,
        MatFormFieldModule,
        MatInputModule,
        MatPaginatorModule,
        MatProgressBarModule,
        MatProgressSpinnerModule,
        MatSelectModule,
        MatSlideToggleModule,
        MatSnackBarModule,
        MatSortModule,
        MatStepperModule,
        MatTableModule,
        MatTabsModule,
        MatToolbarModule,
        MatTooltipModule,
        NgxUiLoaderModule,
        NgxMatDatetimePickerModule,
        NgxMatTimepickerModule,
        NgxMatNativeDateModule,
        NgxMaskModule.forRoot({
            validation: true,
            dropSpecialCharacters: false
          }),
        EffectsModule.forRoot([]),
        StoreModule.forRoot(reducers, {
            metaReducers,
        }),
        StoreDevtoolsModule.instrument({
            logOnly: environment.production,
            maxAge: 25,
        }),
        AppRoutingModule,
        UIModule,
        NiComponentsModule,
        PagesModule,
        PopoverModule,
    ],
    providers: [
        {
            multi: true,
            provide: HTTP_INTERCEPTORS,
            useClass: JwtInterceptor,
        },
        {
            multi: true,
            provide: HTTP_INTERCEPTORS,
            useClass: HttpErrorInterceptor,
        },
        AcukwikairportsService,
        AircraftsService,
        AircraftpricesService,
        AuthenticationService,
        ContactinfobyfboService,
        ContactinfobygroupsService,
        ContactsService,
        CustomcustomertypesService,
        CustomeraircraftsService,
        CustomerCompanyTypesService,
        CustomercontactsService,
        CustomerinfobygroupService,
        CustomermarginsService,
        CustomersService,
        CustomersviewedbyfboService,
        DistributionService,
        EmailcontentService,
        FboairportsService,
        FbocontactsService,
        FbofeesandtaxesService,
        FbofeeandtaxomitsbycustomerService,
        FbofeeandtaxomitsbypricingtemplateService,
        FbomissedquoteslogService,
        FbopreferencesService,
        FbopricesService,
        FbosService,
        FuelreqsService,
        GroupsService,
        LandingsiteService,
        Parametri,
        PricetiersService,
        PricingtemplatesService,
        RampfeesService,
        RampfeesettingsService,
        SharedService,
        TemporaryAddOnMarginService,
        UserService,
        AppService,
        AirportWatchService,
        TagsService,
        AssociationsService,
        AirportFboGeofenceClustersService,
        AirportFboGeofenceClusterCoordinatesService,
        DateTimeService,
        SwimService,
        FlightWatchService,
        FlightWatchHelper,
        FileHelper,
        PricingTemplateCalcService,
        DocumentService,
        ServiceOrderService,
        ServicesAndFeesService,
        ServiceTypeService,
        SnackBarService,
        FavoritesService,
        FormValidationHelperService,
        FlightWatchMapSharedService,
        ManageFboGroupsService,
        StringHelperService,
        JetNetService
    ],
})
export class AppModule { }
