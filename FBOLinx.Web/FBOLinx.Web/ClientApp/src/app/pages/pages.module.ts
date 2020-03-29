import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { NiComponentsModule } from "../ni-components/ni-components.module";

import { MatAutocompleteModule } from "@angular/material/autocomplete";
import { MatButtonModule } from "@angular/material/button";
import { MatButtonToggleModule } from "@angular/material/button-toggle";
import { MatCardModule } from "@angular/material/card";
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatChipsModule } from "@angular/material/chips";
import { MatNativeDateModule, MatRippleModule } from "@angular/material/core";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatDialogModule } from "@angular/material/dialog";
import { MatExpansionModule } from "@angular/material/expansion";
import { MatGridListModule } from "@angular/material/grid-list";
import { MatIconModule } from "@angular/material/icon";
import { MatInputModule } from "@angular/material/input";
import { MatListModule } from "@angular/material/list";
import { MatMenuModule } from "@angular/material/menu";
import { MatPaginatorModule } from "@angular/material/paginator";
import { MatProgressBarModule } from "@angular/material/progress-bar";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatRadioModule } from "@angular/material/radio";
import { MatSelectModule } from "@angular/material/select";
import { MatSidenavModule } from "@angular/material/sidenav";
import { MatSlideToggleModule } from "@angular/material/slide-toggle";
import { MatSliderModule } from "@angular/material/slider";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { MatSortModule } from "@angular/material/sort";
import { MatStepperModule } from "@angular/material/stepper";
import { MatTableModule } from "@angular/material/table";
import { MatTabsModule } from "@angular/material/tabs";
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatTooltipModule } from "@angular/material/tooltip";
import { NgxChartsModule } from "@swimlane/ngx-charts";
import { TextMaskModule } from "angular2-text-mask";

import { NgxCurrencyModule } from "ngx-currency";
import { RichTextEditorAllModule } from "@syncfusion/ej2-angular-richtexteditor";

import { PageTypographyComponent } from "./typography/typography.component";
import { PageNotFoundComponent } from "./not-found/not-found.component";
import { PageLayoutsComponent } from "./layouts/layouts.component";
import { PageSignIn1Component } from "./extra-pages/sign-in-1/sign-in-1.component";
import { PageSignIn2Component } from "./extra-pages/sign-in-2/sign-in-2.component";
import { PageSignIn3Component } from "./extra-pages/sign-in-3/sign-in-3.component";
import { PageSignUp1Component } from "./extra-pages/sign-up-1/sign-up-1.component";
import { PageSignUp2Component } from "./extra-pages/sign-up-2/sign-up-2.component";
import { PageForgotComponent } from "./extra-pages/forgot/forgot.component";
import { PageConfirmComponent } from "./extra-pages/confirm/confirm.component";
import { Page404Component } from "./extra-pages/page-404/page-404.component";
import { Page500Component } from "./extra-pages/page-500/page-500.component";

import { AuthtokenComponent } from "./auth/authtoken/authtoken.component";
import { AircraftsGridComponent } from "./aircrafts/aircrafts-grid/aircrafts-grid.component";
import { ContactsEditComponent } from "./contacts/contacts-edit/contacts-edit.component";
import { ContactsDialogNewContactComponent } from "./contacts/contacts-edit-modal/contacts-edit-modal.component";
import { ContactsDialogConfirmContactDeleteComponent } from "./contacts/contact-confirm-delete-modal/contact-confirm-delete-modal.component";
import { DialogConfirmAircraftDeleteComponent } from "./customer-aircrafts/customer-aircrafts-confirm-delete-modal/customer-aircrafts-confirm-delete-modal.component";
import { ContactsGridComponent } from "./contacts/contacts-grid/contacts-grid.component";
import { ContactsHomeComponent } from "./contacts/contacts-home/contacts-home.component";
import { CustomersEditComponent } from "./customers/customers-edit/customers-edit.component";
import { CustomerAircraftsDialogNewAircraftComponent } from "./customer-aircrafts/customer-aircrafts-dialog-new-aircraft/customer-aircrafts-dialog-new-aircraft.component";
import { CustomerAircraftsEditComponent } from "./customer-aircrafts/customer-aircrafts-edit/customer-aircrafts-edit.component";
import { CustomerAircraftsGridComponent } from "./customer-aircrafts/customer-aircrafts-grid/customer-aircrafts-grid.component";
import { CustomerCompanyTypeDialogComponent } from "./customers/customer-company-type-dialog/customer-company-type-dialog.component";
import { CustomersDialogNewCustomerComponent } from "./customers/customers-dialog-new-customer/customers-dialog-new-customer.component";
import { CustomersGridComponent } from "./customers/customers-grid/customers-grid.component";
import { CustomersHomeComponent } from "./customers/customers-home/customers-home.component";
import { DashboardAdminComponent } from "./dashboards/dashboard-admin/dashboard-admin.component";
import { DashboardFboComponent } from "./dashboards/dashboard-fbo/dashboard-fbo.component";
import { DashboardHomeComponent } from "./dashboards/dashboard-home/dashboard-home.component";
import { FbosContactsComponent } from "./fbos/fbos-contacts/fbos-contacts.component";
import { FbosDialogNewFboComponent } from "./fbos/fbos-dialog-new-fbo/fbos-dialog-new-fbo.component";
import { FboPricesHomeComponent } from "./fbo-prices/fbo-prices-home/fbo-prices-home.component";
import { FbosHomeComponent } from "./fbos/fbos-home/fbos-home.component";
import { FbosGridComponent } from "./fbos/fbos-grid/fbos-grid.component";
import { FbosEditComponent } from "./fbos/fbos-edit/fbos-edit.component";
import { FuelreqsGridComponent } from "./fuelreqs/fuelreqs-grid/fuelreqs-grid.component";
import { FuelreqsHomeComponent } from "./fuelreqs/fuelreqs-home/fuelreqs-home.component";
import { GroupsDialogNewGroupComponent } from "./groups/groups-dialog-new-group/groups-dialog-new-group.component";
import { GroupsEditComponent } from "./groups/groups-edit/groups-edit.component";
import { GroupsGridComponent } from "./groups/groups-grid/groups-grid.component";
import { GroupsHomeComponent } from "./groups/groups-home/groups-home.component";
import { PricingTemplatesDialogNewTemplateComponent } from "./pricing-templates/pricing-templates-dialog-new-template/pricing-templates-dialog-new-template.component";
import { PricingTemplatesDialogCopyTemplateComponent } from "./pricing-templates/pricing-template-dialog-copy-template/pricing-template-dialog-copy-template.component";
import { PricingTemplatesDialogDeleteWarningComponent } from "./pricing-templates/pricing-template-dialog-delete-warning-template/pricing-template-dialog-delete-warning.component";
import { FboPricesSelectDefaultTemplateComponent } from "./fbo-prices/fbo-prices-select-default-template/fbo-prices-select-default-template.component";
import { PricingTemplatesEditComponent } from "./pricing-templates/pricing-templates-edit/pricing-templates-edit.component";
import { PricingTemplatesGridComponent } from "./pricing-templates/pricing-templates-grid/pricing-templates-grid.component";
import { PricingTemplatesHomeComponent } from "./pricing-templates/pricing-templates-home/pricing-templates-home.component";
import { RampFeesCategoryComponent } from "./ramp-fees/ramp-fees-category/ramp-fees-category.component";
import { RampFeesDialogNewFeeComponent } from "./ramp-fees/ramp-fees-dialog-new-fee/ramp-fees-dialog-new-fee.component";
import { RampFeesImportInformationComponent } from './ramp-fees/ramp-fees-import-information-dialog/ramp-fees-import-information-dialog.component';
import { RampFeesHomeComponent } from "./ramp-fees/ramp-fees-home/ramp-fees-home.component";
import { AnalyticsHomeComponent } from "./analytics/analytics-home/analytics-home.component";
import { UsersDialogNewUserComponent } from "./users/users-dialog-new-user/users-dialog-new-user.component";
import { UsersEditComponent } from "./users/users-edit/users-edit.component";
import { UsersGridComponent } from "./users/users-grid/users-grid.component";
import { UsersHomeComponent } from "./users/users-home/users-home.component";

// Shared
import { AccountProfileComponent } from ".././shared/components/account-profile/account-profile.component";
import { AirportAutocompleteComponent } from ".././shared/components/airport-autocomplete/airport-autocomplete.component";
import { DeleteConfirmationComponent } from ".././shared/components/delete-confirmation/delete-confirmation.component";
import { CloseConfirmationComponent } from ".././shared/components/close-confirmation/close-confirmation.component";
import { DistributionWizardMainComponent } from ".././shared/components/distribution-wizard/distribution-wizard-main/distribution-wizard-main.component";
import { DistributionWizardReviewComponent } from ".././shared/components/distribution-wizard/distribution-wizard-review/distribution-wizard-review.component";
import { EmailContentEditComponent } from ".././shared/components/email-content-edit/email-content-edit.component";
import { EmailContentSelectionComponent } from ".././shared/components/email-content-selection/email-content-selection.component";
import { ForgotPasswordDialogComponent } from ".././shared/components/forgot-password/forgot-password-dialog/forgot-password-dialog.component";
import { ManageConfirmationComponent } from ".././shared/components/manage-confirmation/manage-confirmation.component";
import { NotificationComponent } from ".././shared/components/notification/notification.component";
import { PricingExpiredNotificationComponent } from ".././shared/components/pricing-expired-notification/pricing-expired-notification.component";
import { TemporaryAddOnMarginComponent } from ".././shared/components/temporary-add-on-margin/temporary-add-on-margin.component";
import { StatisticsOrdersByLocationComponent } from ".././shared/components/statistics-orders-by-location/statistics-orders-by-location.component";
import { StatisticsTotalAircraftComponent } from ".././shared/components/statistics-total-aircraft/statistics-total-aircraft.component";
import { StatisticsTotalCustomersComponent } from ".././shared/components/statistics-total-customers/statistics-total-customers.component";
import { StatisticsTotalOrdersComponent } from ".././shared/components/statistics-total-orders/statistics-total-orders.component";
import { PopoverModule } from ".././shared/components/popover/popover.module";
import { TooltipModalComponent } from ".././shared/components/tooltip-modal/tooltip-modal.component";
import { AnalyticsOrdersQuoteChartComponent } from "../shared/components/analytics-orders-quote-chart/analytics-orders-quote-chart.component";
import { AnalyticsOrdersOverTimeChartComponent } from "../shared/components/analytics-orders-over-time-chart/analytics-orders-over-time-chart.component";
import { FuelReqsExportModalComponent } from "../shared/components/fuelreqs-export/fuelreqs-export.component";

// Pipes
import { AppPipesModule } from "../app-pipes.module";


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
        NgxCurrencyModule,
        RichTextEditorAllModule,
        AppPipesModule,
        PopoverModule,
    ],
    declarations: [
        PageTypographyComponent,
        PageNotFoundComponent,
        PageSignIn1Component,
        PageSignIn2Component,
        PageSignIn3Component,
        PageSignUp1Component,
        PageSignUp2Component,
        PageForgotComponent,
        PageConfirmComponent,
        Page404Component,
        Page500Component,
        PageLayoutsComponent,
        AuthtokenComponent,
        ContactsEditComponent,
        ContactsDialogNewContactComponent,
        ContactsDialogConfirmContactDeleteComponent,
        DialogConfirmAircraftDeleteComponent,
        ContactsGridComponent,
        ContactsHomeComponent,
        CustomerAircraftsDialogNewAircraftComponent,
        CustomerAircraftsEditComponent,
        CustomerAircraftsGridComponent,
        CustomerCompanyTypeDialogComponent,
        CustomersDialogNewCustomerComponent,
        CustomersEditComponent,
        CustomersGridComponent,
        CustomersHomeComponent,
        DashboardAdminComponent,
        DashboardFboComponent,
        DashboardHomeComponent,
        FbosContactsComponent,
        FbosDialogNewFboComponent,
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
        DeleteConfirmationComponent,
        CloseConfirmationComponent,
        DistributionWizardMainComponent,
        DistributionWizardReviewComponent,
        AccountProfileComponent,
        EmailContentEditComponent,
        EmailContentSelectionComponent,
        ForgotPasswordDialogComponent,
        ManageConfirmationComponent,
        NotificationComponent,
        PricingExpiredNotificationComponent,
        TemporaryAddOnMarginComponent,
        StatisticsOrdersByLocationComponent,
        StatisticsTotalAircraftComponent,
        StatisticsTotalCustomersComponent,
        StatisticsTotalOrdersComponent,
        TooltipModalComponent,
        AircraftsGridComponent,
        FuelReqsExportModalComponent
    ],
    exports: [],
    entryComponents: [
        DeleteConfirmationComponent,
        CloseConfirmationComponent,
        CustomerAircraftsDialogNewAircraftComponent,
        CustomerAircraftsEditComponent,
        CustomerCompanyTypeDialogComponent,
        CustomersDialogNewCustomerComponent,
        ContactsDialogNewContactComponent,
        ContactsDialogConfirmContactDeleteComponent,
        DialogConfirmAircraftDeleteComponent,
        DistributionWizardMainComponent,
        DistributionWizardReviewComponent,
        AccountProfileComponent,
        ForgotPasswordDialogComponent,
        FbosDialogNewFboComponent,
        GroupsDialogNewGroupComponent,
        ManageConfirmationComponent,
        NotificationComponent,
        PricingExpiredNotificationComponent,
        TemporaryAddOnMarginComponent,
        PricingTemplatesDialogNewTemplateComponent,
        PricingTemplatesDialogDeleteWarningComponent,
        PricingTemplatesDialogCopyTemplateComponent,
        FboPricesSelectDefaultTemplateComponent,
        RampFeesDialogNewFeeComponent,
        RampFeesImportInformationComponent,
        UsersDialogNewUserComponent,
        TooltipModalComponent,
        FuelReqsExportModalComponent
    ],
})
export class PagesModule {}
