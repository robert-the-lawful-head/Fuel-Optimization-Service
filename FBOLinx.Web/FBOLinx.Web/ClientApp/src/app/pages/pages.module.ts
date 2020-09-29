import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { NiComponentsModule } from '../ni-components/ni-components.module';

import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatNativeDateModule, MatRippleModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSliderModule } from '@angular/material/slider';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { TextMaskModule } from 'angular2-text-mask';
import { NgbPopoverModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxUiLoaderModule } from 'ngx-ui-loader';
import { RichTextEditorAllModule } from '@syncfusion/ej2-angular-richtexteditor';
import { GridModule, PageService, SortService, FilterService, GroupService, ToolbarService } from '@syncfusion/ej2-angular-grids';

import { AuthtokenComponent } from './auth/authtoken/authtoken.component';
import { LoginComponent } from './auth/login/login.component';
import { AircraftsGridComponent } from './aircrafts/aircrafts-grid/aircrafts-grid.component';
import { ContactsEditComponent } from './contacts/contacts-edit/contacts-edit.component';
import { ContactsDialogNewContactComponent } from './contacts/contacts-edit-modal/contacts-edit-modal.component';
import { ContactsDialogConfirmContactDeleteComponent } from './contacts/contact-confirm-delete-modal/contact-confirm-delete-modal.component';
import { DialogConfirmAircraftDeleteComponent } from './customer-aircrafts/customer-aircrafts-confirm-delete-modal/customer-aircrafts-confirm-delete-modal.component';
import { ContactsGridComponent } from './contacts/contacts-grid/contacts-grid.component';
import { ContactsHomeComponent } from './contacts/contacts-home/contacts-home.component';
import { SystemcontactsGridComponent } from './contacts/systemcontacts-grid/systemcontacts-grid.component';
import { CustomersEditComponent } from './customers/customers-edit/customers-edit.component';
import { CustomerAircraftsDialogNewAircraftComponent } from './customer-aircrafts/customer-aircrafts-dialog-new-aircraft/customer-aircrafts-dialog-new-aircraft.component';
import { CustomerAircraftsEditComponent } from './customer-aircrafts/customer-aircrafts-edit/customer-aircrafts-edit.component';
import { CustomerAircraftsGridComponent } from './customer-aircrafts/customer-aircrafts-grid/customer-aircrafts-grid.component';
import { CustomerAircraftSelectModelComponent } from './customer-aircrafts/customer-aircrafts-select-model-dialog/customer-aircrafts-select-model-dialog.component';
import { CustomerCompanyTypeDialogComponent } from './customers/customer-company-type-dialog/customer-company-type-dialog.component';
import { CustomersDialogNewCustomerComponent } from './customers/customers-dialog-new-customer/customers-dialog-new-customer.component';
import { CustomersGridComponent } from './customers/customers-grid/customers-grid.component';
import { CustomersHomeComponent } from './customers/customers-home/customers-home.component';
import { DashboardFboComponent } from './dashboards/dashboard-fbo/dashboard-fbo.component';
import { DashboardHomeComponent } from './dashboards/dashboard-home/dashboard-home.component';
import { FbosContactsComponent } from './fbos/fbos-contacts/fbos-contacts.component';
import { FbosDialogNewFboComponent } from './fbos/fbos-dialog-new-fbo/fbos-dialog-new-fbo.component';
import { FbosGridNewFboDialogComponent } from './fbos/fbos-grid-new-fbo-dialog/fbos-grid-new-fbo-dialog.component';
import { FboPricesHomeComponent } from './fbo-prices/fbo-prices-home/fbo-prices-home.component';
import { FbosHomeComponent } from './fbos/fbos-home/fbos-home.component';
import { FbosGridComponent } from './fbos/fbos-grid/fbos-grid.component';
import { FbosEditComponent } from './fbos/fbos-edit/fbos-edit.component';
import { FuelreqsGridComponent } from './fuelreqs/fuelreqs-grid/fuelreqs-grid.component';
import { FuelreqsHomeComponent } from './fuelreqs/fuelreqs-home/fuelreqs-home.component';
import { GroupsDialogNewGroupComponent } from './groups/groups-dialog-new-group/groups-dialog-new-group.component';
import { GroupsEditComponent } from './groups/groups-edit/groups-edit.component';
import { GroupsGridComponent } from './groups/groups-grid/groups-grid.component';
import { GroupsHomeComponent } from './groups/groups-home/groups-home.component';
import { PricingTemplatesDialogNewTemplateComponent } from './pricing-templates/pricing-templates-dialog-new-template/pricing-templates-dialog-new-template.component';
import { PricingTemplatesDialogCopyTemplateComponent } from './pricing-templates/pricing-template-dialog-copy-template/pricing-template-dialog-copy-template.component';
import { PricingTemplatesDialogDeleteWarningComponent } from './pricing-templates/pricing-template-dialog-delete-warning-template/pricing-template-dialog-delete-warning.component';
import { FboPricesSelectDefaultTemplateComponent } from './fbo-prices/fbo-prices-select-default-template/fbo-prices-select-default-template.component';
import { PricingTemplatesEditComponent } from './pricing-templates/pricing-templates-edit/pricing-templates-edit.component';
import { PricingTemplatesGridComponent } from './pricing-templates/pricing-templates-grid/pricing-templates-grid.component';
import { PricingTemplatesHomeComponent } from './pricing-templates/pricing-templates-home/pricing-templates-home.component';
import { RampFeesCategoryComponent } from './ramp-fees/ramp-fees-category/ramp-fees-category.component';
import { RampFeesDialogNewFeeComponent } from './ramp-fees/ramp-fees-dialog-new-fee/ramp-fees-dialog-new-fee.component';
import { RampFeesImportInformationComponent } from './ramp-fees/ramp-fees-import-information-dialog/ramp-fees-import-information-dialog.component';
import { RampFeesHomeComponent } from './ramp-fees/ramp-fees-home/ramp-fees-home.component';
import { AnalyticsHomeComponent } from './analytics/analytics-home/analytics-home.component';
import { UsersDialogNewUserComponent } from './users/users-dialog-new-user/users-dialog-new-user.component';
import { UsersEditComponent } from './users/users-edit/users-edit.component';
import { UsersGridComponent } from './users/users-grid/users-grid.component';
import { UsersHomeComponent } from './users/users-home/users-home.component';
import { SystemcontactsNewContactModalComponent } from './contacts/systemcontacts-new-contact-modal/systemcontacts-new-contact-modal.component';

// Shared
import { FboPricesPanelComponent } from '../shared/components/fbo-prices-panel/fbo-prices-panel.component';
import { AccountProfileComponent } from '../shared/components/account-profile/account-profile.component';
import { AirportAutocompleteComponent } from '../shared/components/airport-autocomplete/airport-autocomplete.component';
import { DeleteConfirmationComponent } from '../shared/components/delete-confirmation/delete-confirmation.component';
import { CloseConfirmationComponent } from '../shared/components/close-confirmation/close-confirmation.component';
import { DistributionWizardMainComponent } from '../shared/components/distribution-wizard/distribution-wizard-main/distribution-wizard-main.component';
import { DistributionWizardReviewComponent } from '../shared/components/distribution-wizard/distribution-wizard-review/distribution-wizard-review.component';
import { EmailContentEditComponent } from '../shared/components/email-content-edit/email-content-edit.component';
import { EmailContentSelectionComponent } from '../shared/components/email-content-selection/email-content-selection.component';
import { ForgotPasswordDialogComponent } from '../shared/components/forgot-password/forgot-password-dialog/forgot-password-dialog.component';
import { ManageConfirmationComponent } from '../shared/components/manage-confirmation/manage-confirmation.component';
import { NotificationComponent } from '../shared/components/notification/notification.component';
import { PricingExpiredNotificationComponent } from '../shared/components/pricing-expired-notification/pricing-expired-notification.component';
import { PricingExpiredNotificationGroupComponent } from '../shared/components/pricing-expired-notification-group/pricing-expired-notification-group.component';
import { TemporaryAddOnMarginComponent } from '../shared/components/temporary-add-on-margin/temporary-add-on-margin.component';
import { StatisticsOrdersByLocationComponent } from '../shared/components/statistics-orders-by-location/statistics-orders-by-location.component';
import { StatisticsTotalAircraftComponent } from '../shared/components/statistics-total-aircraft/statistics-total-aircraft.component';
import { StatisticsTotalCustomersComponent } from '../shared/components/statistics-total-customers/statistics-total-customers.component';
import { StatisticsTotalOrdersComponent } from '../shared/components/statistics-total-orders/statistics-total-orders.component';
import { AnalyticsOrdersQuoteChartComponent } from '../shared/components/analytics-orders-quote-chart/analytics-orders-quote-chart.component';
import { AnalyticsOrdersOverTimeChartComponent } from '../shared/components/analytics-orders-over-time-chart/analytics-orders-over-time-chart.component';
import { AnalyticsVolumesNearbyAirportChartComponent } from '../shared/components/analytics-volumes-nearby-airport-chart/analytics-volumes-nearby-airport-chart.component';
import { AnalyticsCustomerBreakdownChartComponent } from '../shared/components/analytics-customer-breakdown-chart/analytics-customer-breakdown-chart.component';
import { AnalyticsCompaniesQuotesDealTableComponent } from '../shared/components/analytics-companies-quotes-deal-table/analytics-companies-quotes-deal-table.component';
import { AnalyticsFuelVendorSourceChartComponent } from '../shared/components/analytics-fuel-vendor-source-chart/analytics-fuel-vendor-source-chart.component';
import { AnalyticsMarketShareFboAirportChartComponent } from '../shared/components/analytics-market-share-fbo-airport-chart/analytics-market-share-fbo-airport-chart.component';
import { FuelReqsExportModalComponent } from '../shared/components/fuelreqs-export/fuelreqs-export.component';
import { CustomerMatchDialogComponent } from './customers/customer-match-dialog/customer-match-dialog.component';
import { TableColumnFilterComponent } from '../shared/components/table-column-filter/table-column-filter.component';
import { TableGlobalSearchComponent } from '../shared/components/table-global-search/table-global-search.component';
// Pipes
import { AppPipesModule } from '../app-pipes.module';

import { ClickStopPropagationDirective } from '../shared/directives/click-stop-propagation.directive';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NiComponentsModule,
    MatAutocompleteModule,
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
    MatStepperModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
    NgxChartsModule,
    TextMaskModule,
    RichTextEditorAllModule,
    AppPipesModule,
    NgbPopoverModule,
    NgxUiLoaderModule,
    GridModule,
    RouterModule
  ],
  declarations: [
    AuthtokenComponent,
    LoginComponent,
    ContactsEditComponent,
    ContactsDialogNewContactComponent,
    ContactsDialogConfirmContactDeleteComponent,
    DialogConfirmAircraftDeleteComponent,
    ContactsGridComponent,
    ContactsHomeComponent,
    SystemcontactsGridComponent,
    CustomerAircraftsDialogNewAircraftComponent,
    CustomerAircraftSelectModelComponent,
    CustomerAircraftsEditComponent,
    CustomerAircraftsGridComponent,
    CustomerCompanyTypeDialogComponent,
    CustomersDialogNewCustomerComponent,
    CustomersEditComponent,
    CustomersGridComponent,
    CustomersHomeComponent,
    DashboardFboComponent,
    DashboardHomeComponent,
    FbosContactsComponent,
    FbosDialogNewFboComponent,
    FbosGridNewFboDialogComponent,
    FboPricesHomeComponent,
    FbosHomeComponent,
    FbosGridComponent,
    FbosEditComponent,
    FuelreqsGridComponent,
    FuelreqsHomeComponent,
    GroupsDialogNewGroupComponent,
    GroupsEditComponent,
    GroupsGridComponent,
    GroupsHomeComponent,
    PricingTemplatesDialogNewTemplateComponent,
    PricingTemplatesDialogCopyTemplateComponent,
    PricingTemplatesDialogDeleteWarningComponent,
    FboPricesSelectDefaultTemplateComponent,
    PricingTemplatesEditComponent,
    PricingTemplatesGridComponent,
    PricingTemplatesHomeComponent,
    RampFeesCategoryComponent,
    RampFeesDialogNewFeeComponent,
    RampFeesImportInformationComponent,
    RampFeesHomeComponent,
    AnalyticsHomeComponent,
    UsersDialogNewUserComponent,
    UsersEditComponent,
    UsersGridComponent,
    UsersHomeComponent,
    AirportAutocompleteComponent,
    AnalyticsOrdersQuoteChartComponent,
    AnalyticsOrdersOverTimeChartComponent,
    AnalyticsVolumesNearbyAirportChartComponent,
    AnalyticsCustomerBreakdownChartComponent,
    AnalyticsCompaniesQuotesDealTableComponent,
    AnalyticsFuelVendorSourceChartComponent,
    AnalyticsMarketShareFboAirportChartComponent,
    DeleteConfirmationComponent,
    CloseConfirmationComponent,
    DistributionWizardMainComponent,
    DistributionWizardReviewComponent,
    FboPricesPanelComponent,
    AccountProfileComponent,
    EmailContentEditComponent,
    EmailContentSelectionComponent,
    ForgotPasswordDialogComponent,
    ManageConfirmationComponent,
    NotificationComponent,
    PricingExpiredNotificationComponent,
    PricingExpiredNotificationGroupComponent,
    TemporaryAddOnMarginComponent,
    StatisticsOrdersByLocationComponent,
    StatisticsTotalAircraftComponent,
    StatisticsTotalCustomersComponent,
    StatisticsTotalOrdersComponent,
    AircraftsGridComponent,
    FuelReqsExportModalComponent,
    ClickStopPropagationDirective,
    CustomerMatchDialogComponent,
    SystemcontactsNewContactModalComponent,
    TableColumnFilterComponent,
    TableGlobalSearchComponent
  ],
  exports: [
    ClickStopPropagationDirective,
    FboPricesPanelComponent,
  ],
  providers: [
    PageService,
    SortService,
    FilterService,
    GroupService,
    ToolbarService,
  ],
})
export class PagesModule {}
