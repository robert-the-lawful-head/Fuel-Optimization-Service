import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

// Angular Material Modules
import { MatIconModule } from '@angular/material/icon';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';

import { PopoverModule } from 'ngx-smart-popover';

import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { TextMaskModule } from 'angular2-text-mask';
import { NgxUiLoaderModule } from 'ngx-ui-loader';

import { AppRoutingModule } from './app-routing.module';
import { UIModule } from './ui/ui.module';
import { NiComponentsModule } from './ni-components/ni-components.module';
import { PagesModule } from './pages/pages.module';

import { AppComponent } from './app.component';
import { DefaultLayoutComponent } from './layouts/default/default.component';
import { LandingSiteLayoutComponent } from './layouts/landing-site/landing-site.component';

import { LoginModalComponent } from './shared/components/login-modal/login-modal.component';
import { RequestDemoModalComponent } from './shared/components/request-demo-modal/request-demo-modal.component';
import { RequestDemoSuccessComponent } from './shared/components/request-demo-success/request-demo-success.component';

// Services
import { AcukwikairportsService } from './services/acukwikairports.service';
import { AircraftsService } from './services/aircrafts.service';
import { AircraftpricesService } from './services/aircraftprices.service';
import { AuthenticationService } from './services/authentication.service';
import { ContactinfobygroupsService } from './services/contactinfobygroups.service';
import { ContactsService } from './services/contacts.service';
import { CustomcustomertypesService } from './services/customcustomertypes.service';
import { CustomeraircraftsService } from './services/customeraircrafts.service';
import { CustomerCompanyTypesService } from './services/customer-company-types.service';
import { CustomercontactsService } from './services/customercontacts.service';
import { CustomerinfobygroupService } from './services/customerinfobygroup.service';
import { CustomermarginsService } from './services/customermargins.service';
import { CustomersService } from './services/customers.service';
import { CustomersviewedbyfboService } from './services/customersviewedbyfbo.service';
import { DistributionService } from './services/distribution.service';
import { EmailcontentService } from './services/emailcontent.service';
import { FboairportsService } from './services/fboairports.service';
import { FbocontactsService } from './services/fbocontacts.service';
import { FbofeesandtaxesService } from './services/fbofeesandtaxes.service';
import { FbofeeandtaxomitsbycustomerService } from './services/fbofeeandtaxomitsbycustomer.service';
import { FbopreferencesService } from './services/fbopreferences.service';
import { FbopricesService } from './services/fboprices.service';
import { FbosService } from './services/fbos.service';
import { FuelreqsService } from './services/fuelreqs.service';
import { GroupsService } from './services/groups.service';
import { Parametri } from './services/paremeters.service';
import { LandingsiteService } from './services/landingsite.service';
import { PricetiersService } from './services/pricetiers.service';
import { PricingtemplatesService } from './services/pricingtemplates.service';
import { RampfeesService } from './services/rampfees.service';
import { RampfeesettingsService } from './services/rampfeesettings.service';
import { TemporaryAddOnMarginService } from './services/temporaryaddonmargin.service';
import { UserService } from './services/user.service';
import { AppService } from './services/app.service';

import { environment } from '../environments/environment';
import { metaReducers, reducers } from './store/reducers';
import { JwtInterceptor, ErrorInterceptor } from './helpers';

@NgModule({
  declarations: [
    AppComponent,
    DefaultLayoutComponent,
    LandingSiteLayoutComponent,
    LoginModalComponent,
    RequestDemoModalComponent,
    RequestDemoSuccessComponent,
  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
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
    TextMaskModule,
    EffectsModule.forRoot([]),
    StoreModule.forRoot(reducers, {
      metaReducers,
    }),
    StoreDevtoolsModule.instrument({ maxAge: 25, logOnly: environment.production }),
    AppRoutingModule,
    UIModule,
    NiComponentsModule,
    PagesModule,
    PopoverModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true,
    },
    AcukwikairportsService,
    AircraftsService,
    AircraftpricesService,
    AuthenticationService,
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
    TemporaryAddOnMarginService,
    UserService,
    AppService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
