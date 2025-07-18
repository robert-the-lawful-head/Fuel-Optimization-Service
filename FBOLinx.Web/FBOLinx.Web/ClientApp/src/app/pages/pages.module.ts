import { CustomerHistoryComponent } from './customer-history/customer-history/customer-history.component';
import { A11yModule } from '@angular/cdk/a11y';
import { ClipboardModule } from '@angular/cdk/clipboard';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { PortalModule } from '@angular/cdk/portal';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { CdkStepperModule } from '@angular/cdk/stepper';
import { CdkTableModule } from '@angular/cdk/table';
import { CdkTreeModule } from '@angular/cdk/tree';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
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
import { MatStepperModule } from '@angular/material/stepper';
import { MatLegacyTableModule as MatTableModule } from '@angular/material/legacy-table';
import { MatLegacyTabsModule as MatTabsModule } from '@angular/material/legacy-tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatLegacyTooltipModule as MatTooltipModule } from '@angular/material/legacy-tooltip';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { NgbPopoverModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MultiSelectModule as Ej2MultiSelectModule } from '@syncfusion/ej2-angular-dropdowns';
import {
    ColumnMenuService,
    DetailRowService,
    FilterService,
    GridModule,
    GroupService,
    PageService,
    SortService,
    ToolbarService,
} from '@syncfusion/ej2-angular-grids';
import { RichTextEditorAllModule } from '@syncfusion/ej2-angular-richtexteditor';
import { ResizableModule } from 'angular-resizable-element';
import {
    NgxMatDatetimePickerModule,
    NgxMatNativeDateModule,
    NgxMatTimepickerModule
} from '@angular-material-components/datetime-picker';

// Popover
import { PopoverModule } from 'ngx-smart-popover';
import { NgxUiLoaderModule } from 'ngx-ui-loader';
import { DropdownModule } from 'primeng/dropdown';
import { ListboxModule } from 'primeng/listbox';
import { MultiSelectModule } from 'primeng/multiselect';

// Pipes
import { AppPipesModule } from '../app-pipes.module';
import { NiComponentsModule } from '../ni-components/ni-components.module';
import { AccountProfileComponent } from '../shared/components/account-profile/account-profile.component';
import { AircraftAssignModalComponent } from '../shared/components/aircraft-assign-modal/aircraft-assign-modal.component';
import { AirportAutocompleteComponent } from '../shared/components/airport-autocomplete/airport-autocomplete.component';
import { AutocompleteSearchComponent } from '../shared/components/autocomplete-search/autocomplete-search.component';
import { CloseConfirmationComponent } from '../shared/components/close-confirmation/close-confirmation.component';
import { CopyConfirmationComponent } from '../shared/components/copy-confirmation/copy-confirmation.component';
import { CsvExportModalComponent } from '../shared/components/csv-export-modal/csv-export-modal.component';
import { DeleteConfirmationComponent } from '../shared/components/delete-confirmation/delete-confirmation.component';
import { DistributeEmailsConfirmationComponent } from '../shared/components/distribute-emails-confirmation/distribute-emails-confirmation.component';
import { DistributionWizardMainComponent } from '../shared/components/distribution-wizard/distribution-wizard-main/distribution-wizard-main.component';
import { DistributionWizardReviewComponent } from '../shared/components/distribution-wizard/distribution-wizard-review/distribution-wizard-review.component';
import { EmailContentEditComponent } from '../shared/components/email-content-edit/email-content-edit.component';
import { EmailContentSelectionComponent } from '../shared/components/email-content-selection/email-content-selection.component';
import { EmailTemplatesDialogNewTemplateComponent } from '../shared/components/email-templates-dialog-new-template/email-templates-dialog-new-template.component';
// Shared
import { FboPricesPanelComponent } from '../shared/components/fbo-prices-panel/fbo-prices-panel.component';
import { FeeAndTaxBreakdownComponent } from '../shared/components/fee-and-tax-breakdown/fee-and-tax-breakdown.component';
import { ForgotPasswordDialogComponent } from '../shared/components/forgot-password/forgot-password-dialog/forgot-password-dialog.component';
import { FuelReqsExportModalComponent } from '../shared/components/fuelreqs-export/fuelreqs-export.component';
import { ManageConfirmationComponent } from '../shared/components/manage-confirmation/manage-confirmation.component';
import { NotificationComponent } from '../shared/components/notification/notification.component';
import { PriceBreakdownComponent } from '../shared/components/price-breakdown/price-breakdown.component';
import { PriceCheckerComponent } from '../shared/components/price-checker/price-checker.component';
import { PricingExpiredNotificationComponent } from '../shared/components/pricing-expired-notification/pricing-expired-notification.component';
import { PricingExpiredNotificationGroupComponent } from '../shared/components/pricing-expired-notification-group/pricing-expired-notification-group.component';
import { ProceedConfirmationComponent } from '../shared/components/proceed-confirmation/proceed-confirmation.component';
import { SaveConfirmationComponent } from '../shared/components/save-confirmation/save-confirmation.component';
import { StatisticsOrdersByLocationComponent } from '../shared/components/statistics-orders-by-location/statistics-orders-by-location.component';
import { StatisticsTotalAircraftComponent } from '../shared/components/statistics-total-aircraft/statistics-total-aircraft.component';
import { StatisticsTotalCustomersComponent } from '../shared/components/statistics-total-customers/statistics-total-customers.component';
import { StatisticsTotalOrdersComponent } from '../shared/components/statistics-total-orders/statistics-total-orders.component';
import { TableColumnFilterComponent } from '../shared/components/table-column-filter/table-column-filter.component';
import { TableGlobalSearchComponent } from '../shared/components/table-global-search/table-global-search.component';
import { TableSettingsComponent } from '../shared/components/table-settings/table-settings.component';
import { ClickStopPropagationDirective } from '../shared/directives/click-stop-propagation.directive';
import { AircraftsGridComponent } from './aircrafts/aircrafts-grid/aircrafts-grid.component';
import { AnalyticsAirportArrivalsDepaturesComponent } from './analytics/analytics-airport-arrivals-depatures/analytics-airport-arrivals-depatures.component';
import { AnalyticsCompaniesQuotesDealTableComponent } from './analytics/analytics-companies-quotes-deal-table/analytics-companies-quotes-deal-table.component';
import { AnalyticsCustomerBreakdownChartComponent } from './analytics/analytics-customer-breakdown-chart/analytics-customer-breakdown-chart.component';
import { AnalyticsFuelVendorSourceChartComponent } from './analytics/analytics-fuel-vendor-source-chart/analytics-fuel-vendor-source-chart.component';
import { AnalyticsHomeComponent } from './analytics/analytics-home/analytics-home.component';
import { AnalyticsMarketShareFboAirportChartComponent } from './analytics/analytics-market-share-fbo-airport-chart/analytics-market-share-fbo-airport-chart.component';
import { AnalyticsOrdersOverTimeChartComponent } from './analytics/analytics-orders-over-time-chart/analytics-orders-over-time-chart.component';
import { AnalyticsOrdersQuoteChartComponent } from './analytics/analytics-orders-quote-chart/analytics-orders-quote-chart.component';
import { AnalyticsVolumesNearbyAirportChartComponent } from './analytics/analytics-volumes-nearby-airport-chart/analytics-volumes-nearby-airport-chart.component';
import { AuthtokenComponent } from './auth/authtoken/authtoken.component';
import { LoginComponent } from './auth/login/login.component';
import { ResetPasswordComponent } from './auth/reset-password/reset-password.component';
import { ContactsDialogConfirmContactDeleteComponent } from './contacts/contact-confirm-delete-modal/contact-confirm-delete-modal.component';
import { ContactsEditComponent } from './contacts/contacts-edit/contacts-edit.component';
import { ContactsDialogNewContactComponent } from './contacts/contacts-edit-modal/contacts-edit-modal.component';
import { ContactsGridComponent } from './contacts/contacts-grid/contacts-grid.component';
import { ContactsHomeComponent } from './contacts/contacts-home/contacts-home.component';
import { SystemcontactsGridComponent } from './contacts/systemcontacts-grid/systemcontacts-grid.component';
import { SystemcontactsNewContactModalComponent } from './contacts/systemcontacts-new-contact-modal/systemcontacts-new-contact-modal.component';
import { DialogConfirmAircraftDeleteComponent } from './customer-aircrafts/customer-aircrafts-confirm-delete-modal/customer-aircrafts-confirm-delete-modal.component';
import { CustomerAircraftsDialogNewAircraftComponent } from './customer-aircrafts/customer-aircrafts-dialog-new-aircraft/customer-aircrafts-dialog-new-aircraft.component';
import { CustomerAircraftsEditComponent } from './customer-aircrafts/customer-aircrafts-edit/customer-aircrafts-edit.component';
import { CustomerAircraftsGridComponent } from './customer-aircrafts/customer-aircrafts-grid/customer-aircrafts-grid.component';
import { CustomerAircraftSelectModelComponent } from './customer-aircrafts/customer-aircrafts-select-model-dialog/customer-aircrafts-select-model-dialog.component';
import { CustomerCompanyTypeDialogComponent } from './customers/customer-company-type-dialog/customer-company-type-dialog.component';
import { CustomerMatchDialogComponent } from './customers/customer-match-dialog/customer-match-dialog.component';
import { CustomersDialogNewCustomerComponent } from './customers/customers-dialog-new-customer/customers-dialog-new-customer.component';
import { CustomersEditComponent } from './customers/customers-edit/customers-edit.component';
import { CustomersGridComponent } from './customers/customers-grid/customers-grid.component';
import { CustomersHomeComponent } from './customers/customers-home/customers-home.component';
import { CustomersEditDialogComponent } from './customers/customers-edit-dialog/customers-edit-dialog.component';
import { DashboardFboComponent } from './dashboards/dashboard-fbo/dashboard-fbo.component';
import { DashboardFboUpdatedComponent } from './dashboards/dashboard-fbo-updated/dashboard-fbo-updated.component';
import { DashboardHomeComponent } from './dashboards/dashboard-home/dashboard-home.component';
import { EmailTemplatesEditComponent } from './email-templates/email-templates-edit/email-templates-edit.component';
import { EmailTemplatesGridComponent } from './email-templates/email-templates-grid/email-templates-grid.component';
import { EmailTemplatesHomeComponent } from './email-templates/email-templates-home/email-templates-home.component';
import { FboPricesHomeComponent } from './fbo-prices/fbo-prices-home/fbo-prices-home.component';
import { FboPricesUpdateGeneratorComponent } from './fbo-prices/fbo-prices-update-generator/fbo-prices-update-generator.component';
import { FboPricesUpdateGeneratorGridComponent } from './fbo-prices/fbo-prices-update-generator-grid/fbo-prices-update-generator-grid.component';
import { FboPricesSelectDefaultTemplateComponent } from './fbo-prices/fbo-prices-select-default-template/fbo-prices-select-default-template.component';
import { FeeAndTaxSettingsDialogComponent } from './fbo-prices/fee-and-tax-settings-dialog/fee-and-tax-settings-dialog.component';
import { FbosContactsComponent } from './fbos/fbos-contacts/fbos-contacts.component';
import { FbosDialogNewFboComponent } from './fbos/fbos-dialog-new-fbo/fbos-dialog-new-fbo.component';
import { FbosEditComponent } from './fbos/fbos-edit/fbos-edit.component';
import { FbosGridComponent } from './fbos/fbos-grid/fbos-grid.component';
import { FbosGridNewFboDialogComponent } from './fbos/fbos-grid-new-fbo-dialog/fbos-grid-new-fbo-dialog.component';
import { FbosHomeComponent } from './fbos/fbos-home/fbos-home.component';
import { FlightWatchComponent } from './flight-watch/flight-watch.component';
import { FlightWatchAircraftInfoComponent } from './flight-watch/aircraft-popup-container/flight-watch-aircraft-info/flight-watch-aircraft-info.component';
import { FlightWatchMapComponent } from './flight-watch/flight-watch-map/flight-watch-map.component';
import { FlightWatchAicraftGridComponent } from './flight-watch/flight-watch-aicraft-grid/flight-watch-aicraft-grid.component';
import { FuelreqsGridComponent } from './fuelreqs/fuelreqs-grid/fuelreqs-grid.component';
import { FuelreqsHomeComponent } from './fuelreqs/fuelreqs-home/fuelreqs-home.component';
import { GroupAnalyticsCustomerStatisticsComponent } from './group-analytics/group-analytics-customer-statistics/group-analytics-customer-statistics.component';
import { GroupAnalyticsEmailPricingDialogComponent } from './group-analytics/group-analytics-email-pricing-dialog/group-analytics-email-pricing-dialog.component';
import { GroupAnalyticsEmailTemplateDialogComponent } from './group-analytics/group-analytics-email-template-dialog/group-analytics-email-template-dialog.component';
import { GroupAnalyticsFuelVendorSourcesComponent } from './group-analytics/group-analytics-fuel-vendor-sources/group-analytics-fuel-vendor-sources.component';
import { GroupAnalyticsGenerateDialogComponent } from './group-analytics/group-analytics-generate-dialog/group-analytics-generate-dialog.component';
import { GroupAnalyticsHomeComponent } from './group-analytics/group-analytics-home/group-analytics-home.component';
import { GroupAnalyticsMarketShareComponent } from './group-analytics/group-analytics-market-share/group-analytics-market-share.component';
import { GroupCustomersGridComponent } from './group-customers/group-customers-grid/group-customers-grid.component';
import { GroupCustomersHomeComponent } from './group-customers/group-customers-home/group-customers-home.component';
import { GroupsDialogNewGroupComponent } from './groups/groups-dialog-new-group/groups-dialog-new-group.component';
import { GroupsEditComponent } from './groups/groups-edit/groups-edit.component';
import { GroupsGridComponent } from './groups/groups-grid/groups-grid.component';
import { GroupsHomeComponent } from './groups/groups-home/groups-home.component';
import { GroupsMergeDialogComponent } from './groups/groups-merge-dialog/groups-merge-dialog.component';
import { PricingTemplatesDialogCopyTemplateComponent } from './pricing-templates/pricing-template-dialog-copy-template/pricing-template-dialog-copy-template.component';
import { PricingTemplatesDialogDeleteWarningComponent } from './pricing-templates/pricing-template-dialog-delete-warning-template/pricing-template-dialog-delete-warning.component';
import { PricingTemplatesDialogNewTemplateComponent } from './pricing-templates/pricing-templates-dialog-new-template/pricing-templates-dialog-new-template.component';
import { PricingTemplatesEditComponent } from './pricing-templates/pricing-templates-edit/pricing-templates-edit.component';
import { PricingTemplatesGridComponent } from './pricing-templates/pricing-templates-grid/pricing-templates-grid.component';
import { PricingTemplatesHomeComponent } from './pricing-templates/pricing-templates-home/pricing-templates-home.component';
import { RampFeesCategoryComponent } from './ramp-fees/ramp-fees-category/ramp-fees-category.component';
import { RampFeesDialogNewFeeComponent } from './ramp-fees/ramp-fees-dialog-new-fee/ramp-fees-dialog-new-fee.component';
import { RampFeesHomeComponent } from './ramp-fees/ramp-fees-home/ramp-fees-home.component';
import { RampFeesImportInformationComponent } from './ramp-fees/ramp-fees-import-information-dialog/ramp-fees-import-information-dialog.component';
import { UsersDialogNewUserComponent } from './users/users-dialog-new-user/users-dialog-new-user.component';
import { UsersEditComponent } from './users/users-edit/users-edit.component';
import { UsersGridComponent } from './users/users-grid/users-grid.component';
import { UsersHomeComponent } from './users/users-home/users-home.component';
import { CustomerTagDialogComponent } from './customers/customer-tag-dialog/customer-tag-dialog.component';
import { CustomerInfoByGroupHistoryComponent } from './customer-history/customer-info-by-group-history/customer-info-by-group-history.component';
import { AssociationsDialogNewAssociationComponent } from './associations/associations-dialog-new-association/associations-dialog-new-association.component';
import { CustomerContactHistoryComponent } from './customer-history/customer-contact-history/customer-contact-history.component';
import { CustomerAircraftHistoryComponent } from './customer-history/customer-aircraft-history/customer-aircraft-history.component';
import { CutomerItpMraginHistoryComponent } from './customer-history/cutomer-itp-mragin-history/cutomer-itp-mragin-history.component';
import { CustomerHistoryDetailsComponent } from './customer-history/customer-history-details/customer-history-details.component';
import { CustomersAnalyticsComponent } from './customers/customers-analytics/customers-analytics.component';
import { FboGeofencingHomeComponent } from './fbo-geofencing/fbo-geofencing-home/fbo-geofencing-home.component';
import { FboGeofencingGridComponent } from './fbo-geofencing/fbo-geofencing-grid/fbo-geofencing-grid.component';
import { FboGeofencingMapComponent } from './fbo-geofencing/fbo-geofencing-map/fbo-geofencing-map.component';
import { FboGeofencingDialogNewClusterComponent } from
    './fbo-geofencing/fbo-geofencing-dialog-new-cluster/fbo-geofencing-dialog-new-cluster.component';
import { FboGeofencingDialogNewAirportComponent } from './fbo-geofencing/fbo-geofencing-dialog-new-airport/fbo-geofencing-dialog-new-airport.component';
import { AntennaStatusHomeComponent } from './antenna-status/antenna-status-home/antenna-status-home.component';
import { AntennaStatusGridComponent } from './antenna-status/antenna-status-grid/antenna-status-grid.component';
import { FbosMissedOrdersGridComponent } from './fbos-missed-orders/fbos-missed-orders-grid/fbos-missed-orders-grid.component';
import { FbosMissedQuotesGridComponent } from './fbos-missed-quotes/fbos-missed-quotes-grid/fbos-missed-quotes-grid.component';
import { AircraftPopupContainerComponent } from './flight-watch/aircraft-popup-container/aircraft-popup-container.component';
import { FlightWatchAircraftDataTableComponent } from './flight-watch/flight-watch-aicraft-grid/flight-watch-aircraft-data-table/flight-watch-aircraft-data-table.component';
import { MissedOrdersGridComponent } from './missed-orders/missedorders-grid/missedorders-grid.component';
import { FlightWatchMapWrapperComponent } from './flight-watch/flight-watch-map-wrapper/flight-watch-map-wrapper.component';
import { PriceCheckerDialogComponent } from './fbo-prices/price-checker-dialog/price-checker-dialog.component';
import { AircraftLegendComponent } from './flight-watch/aircraft-legend/aircraft-legend.component';
import { FlightWatchFiltersComponent } from './flight-watch/flight-watch-filters/flight-watch-filters.component';
import { LobbyViewComponent } from './lobby-view/lobby-view.component';
import { AboutFbolinxComponent } from './about-fbolinx/about-fbolinx.component';
import { AgreementsAndDocumentsModalComponent } from '../shared/components/Agreements-and-documents-modal/Agreements-and-documents-modal.component';
import { ServiceOrdersListComponent } from './service-orders/service-orders-list/service-orders-list.component';
import { GroupAnalyticsIntraNetworkVisitsReportComponent } from './group-analytics/group-analytics-intra-network-visits-report/group-analytics-intra-network-visits-report.component';
import { ServiceOrdersHomeComponent } from './service-orders/service-orders-home/service-orders-home.component';
import { ServiceOrdersItemListComponent } from './service-orders/service-orders-item-list/service-orders-item-list.component';
import { ServiceOrdersDialogNewComponent } from './service-orders/service-orders-dialog-new/service-orders-dialog-new.component';
import { ServiceOrdersDialogOrderItemsComponent } from './service-orders/service-orders-dialog-order-items/service-orders-dialog-order-items.component';
import { FeeAndTaxBreakdownDialogWrapperComponent } from '../shared/components/price-breakdown/fee-and-tax-breakdown-dialog-wrapper/fee-and-tax-breakdown-dialog-wrapper.component';
import { ServicesAndFeesComponent } from './services-and-fees/services-and-fees.component';
import { ServicesAndFeesHomeComponent } from './services-and-fees-home/services-and-fees-home.component';
import { FavoriteIconComponent } from '../shared/components/favorite-icon/favorite-icon.component';
import { AicraftExpandedDetailComponent } from './flight-watch/flight-watch-aicraft-grid/flight-watch-aircraft-data-table/aicraft-expanded-detail/aicraft-expanded-detail.component';
import { AnalyticsActivityReportsComponent } from './analytics/analytics-activity-reports/analytics-activity-reports.component';
import { AnalyticsReportPopupComponent } from './analytics/analytics-report-popup/analytics-report-popup.component';
import { PresetDateFilterComponent } from '../shared/components/preset-date-filter/preset-date-filter.component';
import { ReportFiltersComponent } from './analytics/analytics-report-popup/report-filters/report-filters.component';
import { CustomerCaptureRateComponent } from './analytics/customer-capture-rate/customer-capture-rate.component';
import { ItemInputComponent } from './services-and-fees/item-input/item-input.component';
import { FuelreqsGridServicesComponent } from './fuelreqs/fuelreqs-grid-services/fuelreqs-grid-services.component';
import { FuelreqsNotesComponent } from './fuelreqs/fuelreqs-notes/fuelreqs-notes.component';
import { DecimalPrecisionPipe } from '../shared/pipes/decimal/decimal-precision.pipe';
import { JetNetInformationComponent } from '../shared/components/jetnet-information/jetnet-information.component';
import {CustomerActionStatusComponent} from '../shared/components/customer-action-status/customer-action-status.component';
import { CurrencyPresicionPipe } from '../shared/pipes/decimal/currencyPresicion.pipe';
import { GroupAnalyticsGenerateDialogGridComponent } from './group-analytics/group-analytics-generate-dialog-grid/group-analytics-generate-dialog-grid.component';
import { MultiselectAutocompleteComponent } from '../shared/components/multiselect-autocomplete/multiselect-autocomplete.component';
import { NgxMaskModule } from 'ngx-mask';
import { GroupFbosGridComponent } from './groups/group-fbos-grid/group-fbos-grid.component';
import { CustomerEmailGridComponent } from './group-analytics/group-analytics-email-pricing-dialog/customer-email-grid/customer-email-grid.component';

@NgModule({
    declarations: [
        AuthtokenComponent,
        LoginComponent,
        ResetPasswordComponent,
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
        CustomersEditDialogComponent,
        DashboardFboComponent,
        DashboardFboUpdatedComponent,
        DashboardHomeComponent,
        EmailTemplatesDialogNewTemplateComponent,
        EmailTemplatesEditComponent,
        EmailTemplatesGridComponent,
        EmailTemplatesHomeComponent,
        FbosContactsComponent,
        FbosDialogNewFboComponent,
        FbosGridNewFboDialogComponent,
        FboPricesHomeComponent,
        FboPricesUpdateGeneratorComponent,
        FboPricesUpdateGeneratorGridComponent,
        FbosHomeComponent,
        FbosGridComponent,
        FbosEditComponent,
        FeeAndTaxSettingsDialogComponent,
        FuelreqsGridComponent,
        FuelreqsHomeComponent,
        GroupsDialogNewGroupComponent,
        GroupsMergeDialogComponent,
        GroupsEditComponent,
        GroupsGridComponent,
        GroupFbosGridComponent,
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
        AnalyticsAirportArrivalsDepaturesComponent,
        CopyConfirmationComponent,
        DeleteConfirmationComponent,
        ProceedConfirmationComponent,
        DistributeEmailsConfirmationComponent,
        CloseConfirmationComponent,
        SaveConfirmationComponent,
        DistributionWizardMainComponent,
        DistributionWizardReviewComponent,
        FboPricesPanelComponent,
        AccountProfileComponent,
        EmailContentEditComponent,
        EmailContentSelectionComponent,
        ForgotPasswordDialogComponent,
        ManageConfirmationComponent,
        NotificationComponent,
        PriceBreakdownComponent,
        PriceCheckerComponent,
        PricingExpiredNotificationComponent,
        PricingExpiredNotificationGroupComponent,
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
        TableGlobalSearchComponent,
        GroupAnalyticsHomeComponent,
        GroupAnalyticsGenerateDialogComponent,
        GroupAnalyticsEmailPricingDialogComponent,
        GroupAnalyticsCustomerStatisticsComponent,
        GroupAnalyticsFuelVendorSourcesComponent,
        GroupAnalyticsMarketShareComponent,
        GroupAnalyticsEmailTemplateDialogComponent,
        GroupCustomersHomeComponent,
        GroupCustomersGridComponent,
        FeeAndTaxBreakdownComponent,
        TableSettingsComponent,
        AircraftAssignModalComponent,
        FlightWatchComponent,
        FlightWatchMapComponent,
        FlightWatchAircraftInfoComponent,
        FlightWatchAicraftGridComponent,
        CsvExportModalComponent,
        AutocompleteSearchComponent,
        CustomerTagDialogComponent,
        CustomerInfoByGroupHistoryComponent,
        AssociationsDialogNewAssociationComponent,
        CustomerContactHistoryComponent,
        CustomerAircraftHistoryComponent,
        CutomerItpMraginHistoryComponent,
        CustomersAnalyticsComponent,
        CustomerHistoryComponent,
        CustomerHistoryDetailsComponent,
        FboGeofencingHomeComponent,
        FboGeofencingGridComponent,
        FboGeofencingMapComponent,
        FboGeofencingDialogNewClusterComponent,
        FboGeofencingDialogNewAirportComponent,
        AntennaStatusHomeComponent,
        AntennaStatusGridComponent,
        FbosMissedOrdersGridComponent,
        FbosMissedQuotesGridComponent,
        AircraftPopupContainerComponent,
        FlightWatchAircraftDataTableComponent,
        AicraftExpandedDetailComponent,
        MissedOrdersGridComponent,
        FlightWatchMapWrapperComponent,
        PriceCheckerDialogComponent,
        AircraftLegendComponent,
        FlightWatchFiltersComponent,
        LobbyViewComponent,
        AboutFbolinxComponent,
        AgreementsAndDocumentsModalComponent,
        GroupAnalyticsIntraNetworkVisitsReportComponent,
        ServiceOrdersListComponent,
        ServiceOrdersHomeComponent,
        ServiceOrdersItemListComponent,
        ServiceOrdersDialogNewComponent,
        ServiceOrdersDialogOrderItemsComponent,
        FeeAndTaxBreakdownDialogWrapperComponent,
        FavoriteIconComponent,
        AnalyticsActivityReportsComponent,
        AnalyticsReportPopupComponent,
        PresetDateFilterComponent,
        ReportFiltersComponent,
        CustomerCaptureRateComponent,
        ServicesAndFeesComponent,
        FavoriteIconComponent,
        ServicesAndFeesHomeComponent,
        RampFeesCategoryComponent,
        ItemInputComponent,
        FuelreqsGridServicesComponent,
        FuelreqsNotesComponent,
        JetNetInformationComponent,
        CustomerActionStatusComponent,
        DecimalPrecisionPipe,
        CurrencyPresicionPipe,
        GroupAnalyticsGenerateDialogGridComponent,
        MultiselectAutocompleteComponent,
        CustomerEmailGridComponent
    ],
    exports: [ClickStopPropagationDirective, FboPricesPanelComponent],
    imports: [
        CommonModule,
        BrowserAnimationsModule,
        FormsModule,
        DragDropModule,
        A11yModule,
        ClipboardModule,
        PortalModule,
        ScrollingModule,
        CdkStepperModule,
        CdkTableModule,
        CdkTreeModule,
        ReactiveFormsModule,
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
        MatStepperModule,
        MatTableModule,
        MatTabsModule,
        MatToolbarModule,
        MatTooltipModule,
        NgxChartsModule,
        NgxMaskModule.forRoot({
            validation: true,
            dropSpecialCharacters: false
            }),
        RichTextEditorAllModule,
        AppPipesModule,
        NgbPopoverModule,
        NgxUiLoaderModule,
        GridModule,
        RouterModule,
        PopoverModule,
        ResizableModule,
        MultiSelectModule,
        ListboxModule,
        DropdownModule,
        Ej2MultiSelectModule,
        NgxMatDatetimePickerModule,
        NgxMatTimepickerModule,
        NgxMatNativeDateModule
    ],
    providers: [
        ColumnMenuService,
        PageService,
        SortService,
        FilterService,
        GroupService,
        ToolbarService,
        DetailRowService,
        DatePipe,
        CurrencyPipe,
        DecimalPrecisionPipe,
        CurrencyPresicionPipe
    ],
})
export class PagesModule { }
