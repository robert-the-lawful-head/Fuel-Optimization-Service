import { CustomerHistoryComponent } from './customer-history/customer-history/customer-history.component';
import { A11yModule } from '@angular/cdk/a11y';
import { ClipboardModule } from '@angular/cdk/clipboard';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { PortalModule } from '@angular/cdk/portal';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { CdkStepperModule } from '@angular/cdk/stepper';
import { CdkTableModule } from '@angular/cdk/table';
import { CdkTreeModule } from '@angular/cdk/tree';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatBadgeModule } from '@angular/material/badge';
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
import { TextMaskModule } from 'angular2-text-mask';
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
import { DashboardFboComponent } from './dashboards/dashboard-fbo/dashboard-fbo.component';
import { DashboardHomeComponent } from './dashboards/dashboard-home/dashboard-home.component';
import { EmailTemplatesEditComponent } from './email-templates/email-templates-edit/email-templates-edit.component';
import { EmailTemplatesGridComponent } from './email-templates/email-templates-grid/email-templates-grid.component';
import { EmailTemplatesHomeComponent } from './email-templates/email-templates-home/email-templates-home.component';
import { FboPricesHomeComponent } from './fbo-prices/fbo-prices-home/fbo-prices-home.component';
import { FboPricesSelectDefaultTemplateComponent } from './fbo-prices/fbo-prices-select-default-template/fbo-prices-select-default-template.component';
import { FeeAndTaxSettingsDialogComponent } from './fbo-prices/fee-and-tax-settings-dialog/fee-and-tax-settings-dialog.component';
import { FbosContactsComponent } from './fbos/fbos-contacts/fbos-contacts.component';
import { FbosDialogNewFboComponent } from './fbos/fbos-dialog-new-fbo/fbos-dialog-new-fbo.component';
import { FbosEditComponent } from './fbos/fbos-edit/fbos-edit.component';
import { FbosGridComponent } from './fbos/fbos-grid/fbos-grid.component';
import { FbosGridNewFboDialogComponent } from './fbos/fbos-grid-new-fbo-dialog/fbos-grid-new-fbo-dialog.component';
import { FbosHomeComponent } from './fbos/fbos-home/fbos-home.component';
import { FlightWatchComponent } from './flight-watch/flight-watch/flight-watch.component';
import { FlightWatchAircraftInfoComponent } from './flight-watch/flight-watch-aircraft-info/flight-watch-aircraft-info.component';
import { FlightWatchMapComponent } from './flight-watch/flight-watch-map/flight-watch-map.component';
import { FlightWatchSettingsComponent } from './flight-watch/flight-watch-settings/flight-watch-settings.component';
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
        DashboardFboComponent,
        DashboardHomeComponent,
        EmailTemplatesDialogNewTemplateComponent,
        EmailTemplatesEditComponent,
        EmailTemplatesGridComponent,
        EmailTemplatesHomeComponent,
        FbosContactsComponent,
        FbosDialogNewFboComponent,
        FbosGridNewFboDialogComponent,
        FboPricesHomeComponent,
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
        FlightWatchSettingsComponent,
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
        FboGeofencingGridComponent

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
        TextMaskModule,
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
        Ej2MultiSelectModule
    ],
    providers: [
        ColumnMenuService,
        PageService,
        SortService,
        FilterService,
        GroupService,
        ToolbarService,
        DetailRowService
    ],
})
export class PagesModule { }
