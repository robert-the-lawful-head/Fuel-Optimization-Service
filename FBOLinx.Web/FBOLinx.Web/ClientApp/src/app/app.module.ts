import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
//import { CommonModule } from '@angular/common';


//Bootstrap Assist Template Modules
import { routes, AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UIModule } from './ui/ui.module';
import { NiComponentsModule } from './ni-components/ni-components.module';
import { PagesModule } from './pages/pages.module';

//NgBootstrap
import { NgbCarouselModule } from '@ng-bootstrap/ng-bootstrap';

import { DefaultLayoutComponent } from './layouts/default/default.component';
import { BoxedLayoutComponent } from './layouts/boxed/boxed.component';
import { DefaultCLayoutComponent } from './layouts/default-c/default-c.component';
import { BoxedCLayoutComponent } from './layouts/boxed-c/boxed-c.component';
import { ExtraLayoutComponent } from './layouts/extra/extra.component';
import { LandingSiteLayoutComponent } from './layouts/landing-site/landing-site.component';

//3rd Party Modules
import { NgxCurrencyModule } from "ngx-currency";
import { RichTextEditorAllModule } from '@syncfusion/ej2-angular-richtexteditor';

//Angular Material Modules
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material';
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
import { TextMaskModule } from 'angular2-text-mask';

//Services
import { AcukwikairportsService } from './services/acukwikairports.service';
import { AircraftsService } from './services/aircrafts.service';
import { AircraftpricesService } from './services/aircraftprices.service';
import { AuthenticationService } from './services/authentication.service';
import { ContactinfobygroupsService } from './services/contactinfobygroups.service';
import { ContactsService } from './services/contacts.service';
import { CustomcustomertypesService } from './services/customcustomertypes.service';
import { CustomeraircraftsService } from './services/customeraircrafts.service';
import {CustomerCompanyTypesService} from './services/customer-company-types.service';
import { CustomercontactsService } from './services/customercontacts.service';
import { CustomerinfobygroupService } from './services/customerinfobygroup.service';
import { CustomermarginsService } from './services/customermargins.service';
import { CustomersService } from './services/customers.service';
import { CustomersviewedbyfboService } from './services/customersviewedbyfbo.service';
import { DistributionService } from './services/distribution.service';
import { EmailcontentService } from './services/emailcontent.service';
import { FboairportsService } from './services/fboairports.service';
import { FbocontactsService } from './services/fbocontacts.service';
import { FbofeesService } from './services/fbofees.service';
import { FbopreferencesService } from './services/fbopreferences.service';
import { FbopricesService } from './services/fboprices.service';
import { FbosService } from './services/fbos.service';
import { FuelreqsService } from './services/fuelreqs.service';
import { GroupsService } from './services/groups.service';
import { LandingsiteService } from './services/landingsite.service';
import { PricetiersService } from './services/pricetiers.service';
import { PricingtemplatesService } from './services/pricingtemplates.service';
import { RampfeesService } from './services/rampfees.service';
import { RampfeesettingsService } from './services/rampfeesettings.service';
import { UserService } from './services/user.service';

//Helpers
import { JwtInterceptor, ErrorInterceptor } from './helpers';

//Pipes
import { DecimalPipe } from '@angular/common';
import { DatePipe } from '@angular/common';
//import {AppPipesModule} from './app-pipes.module'


@NgModule({
    declarations: [
        AppComponent,
        DefaultLayoutComponent,
        BoxedLayoutComponent,
        DefaultCLayoutComponent,
        BoxedCLayoutComponent,
        ExtraLayoutComponent,
        LandingSiteLayoutComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        HttpClientModule,
        FormsModule,
        ReactiveFormsModule,
        BrowserAnimationsModule,
        NgxCurrencyModule,
        RichTextEditorAllModule,

        //Start bootstrap template additions
        AppRoutingModule,
        UIModule,
        NiComponentsModule,
        PagesModule,
        //End bootstrap template additions

        //NgBoostrap Modules
        NgbCarouselModule,
        
        MatAutocompleteModule,
        MatButtonModule,
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
        TextMaskModule,
        RouterModule.forRoot(routes, { useHash: true })
//        ,
        //CommonModule,
        //AppPipesModule
    ],
    exports: [],
    entryComponents: [],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
        AcukwikairportsService, AircraftsService, AircraftpricesService, AuthenticationService, ContactinfobygroupsService, ContactsService, CustomcustomertypesService, CustomeraircraftsService, CustomerCompanyTypesService, CustomercontactsService, CustomerinfobygroupService, CustomermarginsService, CustomersService, CustomersviewedbyfboService, DistributionService, EmailcontentService, FboairportsService, FbocontactsService, FbofeesService, FbopreferencesService, FbopricesService, FbosService, FuelreqsService, GroupsService, LandingsiteService, PricetiersService, PricingtemplatesService, RampfeesService, RampfeesettingsService, UserService],
    bootstrap: [AppComponent]
})
export class AppModule { }
