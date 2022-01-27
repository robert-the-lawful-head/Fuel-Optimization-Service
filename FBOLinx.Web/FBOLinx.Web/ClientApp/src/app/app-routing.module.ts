import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// Guards
import { AuthGuard } from './guards';
// Layout Components
import { DefaultLayoutComponent } from './layouts/default/default.component';
import { LandingSiteLayoutComponent } from './layouts/landing-site/landing-site.component';
import { OutsideTheGateLayoutComponent } from './layouts/outside-the-gate/outside-the-gate.component';
import { AnalyticsHomeComponent } from './pages/analytics/analytics-home/analytics-home.component';
// Page Components
import { AuthtokenComponent } from './pages/auth/authtoken/authtoken.component';
import { LoginComponent } from './pages/auth/login/login.component';
import { ResetPasswordComponent } from './pages/auth/reset-password/reset-password.component';
import { CustomersEditComponent } from './pages/customers/customers-edit/customers-edit.component';
import { CustomersHomeComponent } from './pages/customers/customers-home/customers-home.component';
import { DashboardFboComponent } from './pages/dashboards/dashboard-fbo/dashboard-fbo.component';
import { DashboardFboUpdatedComponent } from './pages/dashboards/dashboard-fbo-updated/dashboard-fbo-updated.component';
import { DashboardHomeComponent } from './pages/dashboards/dashboard-home/dashboard-home.component';
import { EmailTemplatesEditComponent } from './pages/email-templates/email-templates-edit/email-templates-edit.component';
import { EmailTemplatesHomeComponent } from './pages/email-templates/email-templates-home/email-templates-home.component';
import { FboPricesHomeComponent } from './pages/fbo-prices/fbo-prices-home/fbo-prices-home.component';
import { FbosEditComponent } from './pages/fbos/fbos-edit/fbos-edit.component';
import { FbosHomeComponent } from './pages/fbos/fbos-home/fbos-home.component';
import { FlightWatchComponent } from './pages/flight-watch/flight-watch/flight-watch.component';
import { FuelreqsHomeComponent } from './pages/fuelreqs/fuelreqs-home/fuelreqs-home.component';
import { GroupAnalyticsHomeComponent } from './pages/group-analytics/group-analytics-home/group-analytics-home.component';
import { GroupCustomersHomeComponent } from './pages/group-customers/group-customers-home/group-customers-home.component';
import { GroupsEditComponent } from './pages/groups/groups-edit/groups-edit.component';
import { GroupsHomeComponent } from './pages/groups/groups-home/groups-home.component';
import { PricingTemplatesEditComponent } from './pages/pricing-templates/pricing-templates-edit/pricing-templates-edit.component';
import { PricingTemplatesHomeComponent } from './pages/pricing-templates/pricing-templates-home/pricing-templates-home.component';
import { RampFeesHomeComponent } from './pages/ramp-fees/ramp-fees-home/ramp-fees-home.component';
import { UsersEditComponent } from './pages/users/users-edit/users-edit.component';
import { UsersHomeComponent } from './pages/users/users-home/users-home.component';
import { FboGeofencingHomeComponent } from './pages/fbo-geofencing/fbo-geofencing-home/fbo-geofencing-home.component';

const defaultRoutes: Routes = [
    {
        canActivate: [AuthGuard],
        component: CustomersHomeComponent,
        path: 'customers',
    },
    {
        canActivate: [AuthGuard],
        component: CustomersEditComponent,
        path: 'customers/:id',
    },
    {
        canActivate: [AuthGuard],
        component: DashboardHomeComponent,
        path: 'dashboard',
    },
    {
        canActivate: [AuthGuard],
        component: DashboardFboComponent,
        path: 'dashboard-fbo',
    },
    {
        canActivate: [AuthGuard],
        component: DashboardFboUpdatedComponent,
        path: 'dashboard-fbo-updated',
    },
    {
        canActivate: [AuthGuard],
        component: DashboardFboComponent,
        path: 'dashboard-csr',
    },
    {
        canActivate: [AuthGuard],
        component: FboPricesHomeComponent,
        path: 'fbo-prices',
    },
    {
        canActivate: [AuthGuard],
        component: GroupsHomeComponent,
        data: {
            expectedRoles: [3],
        },
        path: 'groups',
    },
    {
        canActivate: [AuthGuard],
        component: GroupsEditComponent,
        data: {
            expectedRoles: [3],
        },
        path: 'groups/:id',
    },
    {
        canActivate: [AuthGuard],
        component: FbosHomeComponent,
        data: {
            expectedRoles: [2, 3],
        },
        path: 'fbos',
    },
    {
        canActivate: [AuthGuard],
        component: FbosEditComponent,
        data: {
            expectedRoles: [2, 3],
        },
        path: 'fbos/:id',
    },
    {
        canActivate: [AuthGuard],
        component: FboGeofencingHomeComponent,
        data: {
            expectedRoles: [3],
        },
        path: 'fbo-geofencing',
    },
    {
        canActivate: [AuthGuard],
        component: GroupAnalyticsHomeComponent,
        data: {
            expectedRoles: [2, 3],
        },
        path: 'group-analytics',
    },
    {
        canActivate: [AuthGuard],
        component: GroupCustomersHomeComponent,
        data: {
            expectedRoles: [2, 3],
        },
        path: 'group-customers',
    },
    {
        canActivate: [AuthGuard],
        component: FuelreqsHomeComponent,
        path: 'fuelreqs',
    },
    {
        canActivate: [AuthGuard],
        component: PricingTemplatesHomeComponent,
        path: 'pricing-templates',
    },
    {
        canActivate: [AuthGuard],
        component: PricingTemplatesEditComponent,
        path: 'pricing-templates/:id',
    },
    {
        canActivate: [AuthGuard],
        component: EmailTemplatesHomeComponent,
        path: 'email-templates',
    },
    {
        canActivate: [AuthGuard],
        component: EmailTemplatesEditComponent,
        path: 'email-templates/:id',
    },
    {
        canActivate: [AuthGuard],
        component: FuelreqsHomeComponent,
        path: 'fuelreqs',
    },
    {
        canActivate: [AuthGuard],
        component: RampFeesHomeComponent,
        path: 'rampfees',
    },
    {
        canActivate: [AuthGuard],
        component: AnalyticsHomeComponent,
        path: 'analytics',
    },
    {
        canActivate: [AuthGuard],
        component: UsersHomeComponent,
        data: {
            expectedRoles: [1, 2, 3],
        },
        path: 'users',
    },
    {
        canActivate: [AuthGuard],
        component: UsersEditComponent,
        data: {
            expectedRoles: [1, 2, 3],
        },
        path: 'users/:id',
    },
    {
        canActivate: [AuthGuard],
        component: FlightWatchComponent,
        path: 'flight-watch',
    },
    {
        canActivate: [AuthGuard],
        component: DashboardHomeComponent,
        path: '**',
    },
];

const outsideTheGateRoutes: Routes = [
    {
        component: AuthtokenComponent,
        path: 'auth',
    },
];

const routes: Routes = [
    {
        component: LandingSiteLayoutComponent,
        path: '',
        //children: landingSiteRoutes,
    },
    {
        children: defaultRoutes,
        component: DefaultLayoutComponent,
        path: 'default-layout',
    },
    {
        children: outsideTheGateRoutes,
        component: OutsideTheGateLayoutComponent,
        path: 'outside-the-gate-layout',
    },
    {
        component: LoginComponent,
        path: 'app-login',
    },
    {
        component: ResetPasswordComponent,
        path: 'reset-password',
    },
    {
        component: LandingSiteLayoutComponent,
        path: '**',
        //children: landingSiteRoutes,
    },
];

@NgModule({
    exports: [RouterModule],
    imports: [
        RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' }),
    ],
})
export class AppRoutingModule {}
