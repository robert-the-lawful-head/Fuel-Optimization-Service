import { NgModule }                     from '@angular/core';
import { Routes, RouterModule }         from '@angular/router';

//Layout Components
import { DefaultLayoutComponent }       from './layouts/default/default.component';
import { BoxedLayoutComponent }         from './layouts/boxed/boxed.component';
import { DefaultCLayoutComponent }      from './layouts/default-c/default-c.component';
import { BoxedCLayoutComponent }        from './layouts/boxed-c/boxed-c.component';
import { ExtraLayoutComponent } from './layouts/extra/extra.component';
import { LandingSiteLayoutComponent } from './layouts/landing-site/landing-site.component';

import { PageTypographyComponent }      from './pages/typography/typography.component';
import { PageNotFoundComponent }        from './pages/not-found/not-found.component';

import { PageSignIn1Component }         from './pages/extra-pages/sign-in-1/sign-in-1.component';
import { PageSignIn2Component }         from './pages/extra-pages/sign-in-2/sign-in-2.component';
import { PageSignIn3Component }         from './pages/extra-pages/sign-in-3/sign-in-3.component';
import { PageSignUp1Component }         from './pages/extra-pages/sign-up-1/sign-up-1.component';
import { PageSignUp2Component }         from './pages/extra-pages/sign-up-2/sign-up-2.component';
import { PageForgotComponent }          from './pages/extra-pages/forgot/forgot.component';
import { PageConfirmComponent }         from './pages/extra-pages/confirm/confirm.component';
import { Page404Component }             from './pages/extra-pages/page-404/page-404.component';
import { Page500Component }             from './pages/extra-pages/page-500/page-500.component';
import { PageAboutUsComponent }         from './pages/pages-service/about-us/about-us.component';
import { PageFaqComponent }             from './pages/pages-service/faq/faq.component';
import { PageTimelineComponent }        from './pages/pages-service/timeline/timeline.component';
import { PageInvoiceComponent }         from './pages/pages-service/invoice/invoice.component';
import { PageCalendarComponent }        from './pages/calendar/calendar.component';
import { PageSimpleTableComponent }     from './pages/tables/simple-table/simple-table.component';
import { PageBootstrapTablesComponent } from './pages/tables/bootstrap-tables/bootstrap-tables.component';
import { PageSortingTableComponent }    from './pages/tables/sorting-table/sorting-table.component';
import { PageFilteringTableComponent }  from './pages/tables/filtering-table/filtering-table.component';
import { PagePaginationTableComponent } from './pages/tables/pagination-table/pagination-table.component';
import { PageGoogleMapComponent }       from './pages/maps/google-map/google-map.component';
import { PageLeafletMapComponent }      from './pages/maps/leaflet-map/leaflet-map.component';
import { PageWidgetsComponent }         from './pages/widgets/widgets.component';
import { PageLayoutsComponent } from './pages/layouts/layouts.component';

//Page Components
import { AppComponent } from './app.component';
import { AuthtokenComponent} from './pages/auth/authtoken/authtoken.component';
import { ContactsEditComponent } from './pages/contacts/contacts-edit/contacts-edit.component';
import { ContactsGridComponent } from './pages/contacts/contacts-grid/contacts-grid.component';
import { CustomersEditComponent } from './pages/customers/customers-edit/customers-edit.component';
import { CustomersGridComponent } from './pages/customers/customers-grid/customers-grid.component';
import { CustomersHomeComponent } from './pages/customers/customers-home/customers-home.component';
import { DashboardAdminComponent } from './pages/dashboards/dashboard-admin/dashboard-admin.component';
import { DashboardFboComponent } from './pages/dashboards/dashboard-fbo/dashboard-fbo.component';
import { DashboardHomeComponent } from './pages/dashboards/dashboard-home/dashboard-home.component';
import { FboPricesHomeComponent } from './pages/fbo-prices/fbo-prices-home/fbo-prices-home.component';
import { FbosHomeComponent } from './pages/fbos/fbos-home/fbos-home.component';
import { FbosGridComponent } from './pages/fbos/fbos-grid/fbos-grid.component';
import { FbosEditComponent } from './pages/fbos/fbos-edit/fbos-edit.component';
import { FuelreqsGridComponent } from './pages/fuelreqs/fuelreqs-grid/fuelreqs-grid.component';
import { FuelreqsHomeComponent } from './pages/fuelreqs/fuelreqs-home/fuelreqs-home.component';
import { GroupsEditComponent } from './pages/groups/groups-edit/groups-edit.component';
import { GroupsGridComponent } from './pages/groups/groups-grid/groups-grid.component';
import { GroupsHomeComponent } from './pages/groups/groups-home/groups-home.component';
import { PricingTemplatesEditComponent } from './pages/pricing-templates/pricing-templates-edit/pricing-templates-edit.component';
import { PricingTemplatesGridComponent } from './pages/pricing-templates/pricing-templates-grid/pricing-templates-grid.component';
import { PricingTemplatesHomeComponent } from './pages/pricing-templates/pricing-templates-home/pricing-templates-home.component';
import { RampFeesHomeComponent } from './pages/ramp-fees/ramp-fees-home/ramp-fees-home.component';
import { UsersEditComponent } from './pages/users/users-edit/users-edit.component';
import { UsersGridComponent } from './pages/users/users-grid/users-grid.component';
import { UsersHomeComponent } from './pages/users/users-home/users-home.component';

//Guards
import { AuthGuard } from './guards';

const defaultRoutes: Routes = [
    { path: 'customers', component: CustomersHomeComponent, canActivate: [AuthGuard] },
    { path: 'customers/:id', component: CustomersEditComponent, canActivate: [AuthGuard] },
    {
        path: 'dashboard-admin',
        component: DashboardAdminComponent,
        canActivate: [AuthGuard],
        data: {
            expectedRoles: [3]
        }
    },
    { path: 'dashboard', component: DashboardHomeComponent, canActivate: [AuthGuard] },
    {
        path: 'dashboard-fbo',
        component: DashboardFboComponent,
        canActivate: [AuthGuard]
    },
    { path: 'fbo-prices', component: FboPricesHomeComponent, canActivate: [AuthGuard] },
    {
        path: 'groups',
        component: GroupsHomeComponent,
        canActivate: [AuthGuard],
        data: {
            expectedRoles: [3]
        }
    },
    {
        path: 'groups/:id',
        component: GroupsEditComponent,
        canActivate: [AuthGuard],
        data: {
            expectedRoles: [3]
        }
    },
    {
        path: 'fbos',
        component: FbosHomeComponent,
        canActivate: [AuthGuard],
        data: {
            expectedRoles: [2, 3]
        }
    },
    {
        path: 'fbos/:id',
        component: FbosEditComponent,
        canActivate: [AuthGuard],
        data: {
            expectedRoles: [2, 3]
        }
    },
    { path: 'fuelreqs', component: FuelreqsHomeComponent, canActivate: [AuthGuard] },
    { path: 'pricing-templates', component: PricingTemplatesHomeComponent, canActivate: [AuthGuard] },
    { path: 'pricing-templates/:id', component: PricingTemplatesEditComponent, canActivate: [AuthGuard] },
    { path: 'fuelreqs', component: FuelreqsHomeComponent, canActivate: [AuthGuard] },
    { path: 'rampfees', component: RampFeesHomeComponent, canActivate: [AuthGuard] },
    {
        path: 'users',
        component: UsersHomeComponent,
        canActivate: [AuthGuard],
        data: {
            expectedRoles: [1, 2, 3]
        }
    },
    {
        path: 'users/:id',
        component: UsersEditComponent,
        canActivate: [AuthGuard],
        data: {
            expectedRoles: [1, 2, 3]
        }
    },
    { path: '**', component: DashboardHomeComponent, canActivate: [AuthGuard] }
];

const boxedRoutes: Routes = [
  { path: 'layouts', component: PageLayoutsComponent }
];

const boxedCRoutes: Routes = [
  { path: 'layouts', component: PageLayoutsComponent }
];

const defaultCRoutes: Routes = [
  { path: 'layouts', component: PageLayoutsComponent }
];

const extraRoutes: Routes = [
  { path: 'sign-in', component: PageSignIn1Component },
  { path: 'sign-in-social', component: PageSignIn3Component },
  { path: 'sign-up', component: PageSignUp1Component },
  { path: 'forgot', component: PageForgotComponent },
  { path: 'confirm', component: PageConfirmComponent },
  { path: 'page-404', component: Page404Component },
    { path: 'page-500', component: Page500Component },
    {
        path: 'authtoken/:token',
        component: AuthtokenComponent
    }
];

const landingSiteRoutes: Routes = [
    {
        path: 'authtoken/:token',
        component: AuthtokenComponent,
        data: {
            expectedRoles: []
        }
    }
];

export const routes: Routes = [
    {
        path: '',
        redirectTo: '/landing-site-layout',
        pathMatch: 'full'
    },
    {
        path: 'default-layout',
        component: DefaultLayoutComponent,
        children: defaultRoutes
    },
    {
        path: 'default-c-layout',
        component: DefaultCLayoutComponent,
        children: defaultCRoutes
    },
    {
        path: 'boxed-layout',
        component: BoxedLayoutComponent,
        children: boxedRoutes
    },
    {
        path: 'boxed-c-layout',
        component: BoxedCLayoutComponent,
        children: boxedCRoutes
    },
    {
        path: 'extra-layout',
        component: ExtraLayoutComponent,
        children: extraRoutes
    },
    {
        path: 'landing-site-layout',
        component: LandingSiteLayoutComponent,
        children: landingSiteRoutes
    },
    {
        path: '**',
        component: LandingSiteLayoutComponent,
        children: landingSiteRoutes
    }
];

@NgModule({
  imports: [],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule {}
