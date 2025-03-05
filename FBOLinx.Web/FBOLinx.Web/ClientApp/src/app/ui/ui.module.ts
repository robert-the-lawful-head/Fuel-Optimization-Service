import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { MatLegacyAutocompleteModule as MatAutocompleteModule } from '@angular/material/legacy-autocomplete';
import { MatBadgeModule } from '@angular/material/badge';
import { MatLegacyButtonModule as MatButtonModule } from '@angular/material/legacy-button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatLegacyCardModule as MatCardModule } from '@angular/material/legacy-card';
import { MatLegacyCheckboxModule as MatCheckboxModule } from '@angular/material/legacy-checkbox';
import { MatLegacyChipsModule as MatChipsModule } from '@angular/material/legacy-chips';
import { MatNativeDateModule, MatRippleModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatLegacyDialogModule as MatDialogModule } from '@angular/material/legacy-dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatLegacyInputModule as MatInputModule } from '@angular/material/legacy-input';
import { MatLegacyListModule as MatListModule } from '@angular/material/legacy-list';
import { MatLegacyMenuModule as MatMenuModule } from '@angular/material/legacy-menu';
import { MatLegacyPaginatorModule as MatPaginatorModule } from '@angular/material/legacy-paginator';
import { MatLegacyProgressBarModule as MatProgressBarModule } from '@angular/material/legacy-progress-bar';
import { MatLegacyProgressSpinnerModule as MatProgressSpinnerModule } from '@angular/material/legacy-progress-spinner';
import { MatLegacyRadioModule as MatRadioModule } from '@angular/material/legacy-radio';
import { MatLegacySelectModule as MatSelectModule } from '@angular/material/legacy-select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatLegacySlideToggleModule as MatSlideToggleModule } from '@angular/material/legacy-slide-toggle';
import { MatLegacySliderModule as MatSliderModule } from '@angular/material/legacy-slider';
import { MatLegacySnackBarModule as MatSnackBarModule } from '@angular/material/legacy-snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatLegacyTableModule as MatTableModule } from '@angular/material/legacy-table';
import { MatLegacyTabsModule as MatTabsModule } from '@angular/material/legacy-tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatLegacyTooltipModule as MatTooltipModule } from '@angular/material/legacy-tooltip';
import { RouterModule } from '@angular/router';
import { NgbPopoverModule } from '@ng-bootstrap/ng-bootstrap';
import {
    NgxMatDatetimePickerModule,
    NgxMatNativeDateModule,
    NgxMatTimepickerModule
} from '@angular-material-components/datetime-picker';

import { NiComponentsModule } from '../ni-components/ni-components.module';
import { AdditionNavbarComponent } from './components/addition-navbar/addition-navbar.component';
import { FooterComponent } from './components/footer/footer.component';
import { HorizontalNavbarComponent } from './components/horizontal-navbar/horizontal-navbar.component';
import { LogoComponent } from './components/logo/logo.component';
import { MenuComponent } from './components/menu/menu.component';
import { VerticalNavbarComponent } from './components/vertical-navbar/vertical-navbar.component';
import { AppPipesModule } from "../app-pipes.module";
import { DemoRequestStaticDialogComponent } from '../pages/demo-request-static-dialog/demo-request-static-dialog.component';
import { IncomingFavoriteAircraftInfoComponent } from './components/incoming-favorite-aircraft-info/incoming-favorite-aircraft-info.component';
import {JetNetSearchComponent } from "../shared/components/jetnet-search/jetnet-search.component";

@NgModule({
    declarations: [
        HorizontalNavbarComponent,
        VerticalNavbarComponent,
        AdditionNavbarComponent,
        LogoComponent,
        MenuComponent,
        FooterComponent,
        DemoRequestStaticDialogComponent,
        IncomingFavoriteAircraftInfoComponent,
        JetNetSearchComponent
    ],
    exports: [
        HorizontalNavbarComponent,
        VerticalNavbarComponent,
        AdditionNavbarComponent,
        LogoComponent,
        MenuComponent,
        FooterComponent,
        DemoRequestStaticDialogComponent,
        IncomingFavoriteAircraftInfoComponent,
        JetNetSearchComponent
    ],
    imports: [
        HttpClientModule,
        CommonModule,
        RouterModule,
        NiComponentsModule,
        MatAutocompleteModule,
        MatBadgeModule,
        MatButtonModule,
        MatButtonToggleModule,
        MatCardModule,
        MatCheckboxModule,
        MatChipsModule,
        MatDatepickerModule,
        MatDialogModule,
        MatExpansionModule,
        MatGridListModule,
        MatIconModule,
        MatInputModule,
        MatListModule,
        MatMenuModule,
        MatNativeDateModule,
        MatPaginatorModule,
        MatProgressBarModule,
        MatProgressSpinnerModule,
        MatRadioModule,
        MatRippleModule,
        MatSelectModule,
        MatSidenavModule,
        MatSliderModule,
        MatSlideToggleModule,
        MatSnackBarModule,
        MatSortModule,
        MatTableModule,
        MatTabsModule,
        MatToolbarModule,
        MatTooltipModule,
        NgbPopoverModule,
        NgxMatDatetimePickerModule,
        NgxMatTimepickerModule,
        NgxMatNativeDateModule,
        AppPipesModule
    ]
})
export class UIModule {}
