import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// Layout Components
import { DefaultLayoutComponent } from './layouts/default/default.component';
import { LandingSiteLayoutComponent } from './layouts/landing-site/landing-site.component';

// Page Components
import { AuthtokenComponent } from './pages/auth/authtoken/authtoken.component';
import { LoginComponent } from './pages/auth/login/login.component';
import { ResetPasswordComponent } from './pages/auth/reset-password/reset-password.component';
import { CustomersEditComponent } from './pages/customers/customers-edit/customers-edit.component';
import { CustomersHomeComponent } from './pages/customers/customers-home/customers-home.component';
import { DashboardFboComponent } from './pages/dashboards/dashboard-fbo/dashboard-fbo.component';
import { DashboardHomeComponent } from './pages/dashboards/dashboard-home/dashboard-home.component';
import { FboPricesHomeComponent } from './pages/fbo-prices/fbo-prices-home/fbo-prices-home.component';
import { FbosHomeComponent } from './pages/fbos/fbos-home/fbos-home.component';
import { FbosEditComponent } from './pages/fbos/fbos-edit/fbos-edit.component';
import { FuelreqsHomeComponent } from './pages/fuelreqs/fuelreqs-home/fuelreqs-home.component';
import { GroupsEditComponent } from './pages/groups/groups-edit/groups-edit.component';
import { GroupsHomeComponent } from './pages/groups/groups-home/groups-home.component';
import { PricingTemplatesEditComponent } from './pages/pricing-templates/pricing-templates-edit/pricing-templates-edit.component';
import { PricingTemplatesHomeComponent } from './pages/pricing-templates/pricing-templates-home/pricing-templates-home.component';
import { RampFeesHomeComponent } from './pages/ramp-fees/ramp-fees-home/ramp-fees-home.component';
import { AnalyticsHomeComponent } from './pages/analytics/analytics-home/analytics-home.component';
import { UsersEditComponent } from './pages/users/users-edit/users-edit.component';
import { UsersHomeComponent } from './pages/users/users-home/users-home.component';

// Guards
import { AuthGuard } from './guards';
const defaultRoutes: Routes = [{
    path: 'customers',
    component: CustomersHomeComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'customers/:id',
    component: CustomersEditComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'dashboard',
    component: DashboardHomeComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'dashboard-fbo',
    component: DashboardFboComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'fbo-prices',
    component: FboPricesHomeComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'groups',
    component: GroupsHomeComponent,
    canActivate: [AuthGuard],
    data: {
      expectedRoles: [3],
    },
  },
  {
    path: 'groups/:id',
    component: GroupsEditComponent,
    canActivate: [AuthGuard],
    data: {
      expectedRoles: [3],
    },
  },
  {
    path: 'fbos',
    component: FbosHomeComponent,
    canActivate: [AuthGuard],
    data: {
      expectedRoles: [2, 3],
    },
  },
  {
    path: 'fbos/:id',
    component: FbosEditComponent,
    canActivate: [AuthGuard],
    data: {
      expectedRoles: [2, 3],
    },
  },
  {
    path: 'fuelreqs',
    component: FuelreqsHomeComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'pricing-templates',
    component: PricingTemplatesHomeComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'pricing-templates/:id',
    component: PricingTemplatesEditComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'fuelreqs',
    component: FuelreqsHomeComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'rampfees',
    component: RampFeesHomeComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'analytics',
    component: AnalyticsHomeComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'users',
    component: UsersHomeComponent,
    canActivate: [AuthGuard],
    data: {
      expectedRoles: [1, 2, 3],
    },
  },
  {
    path: 'users/:id',
    component: UsersEditComponent,
    canActivate: [AuthGuard],
    data: {
      expectedRoles: [1, 2, 3],
    },
  },
  {
    path: '**',
    component: DashboardHomeComponent,
    canActivate: [AuthGuard]
  },
];

const landingSiteRoutes: Routes = [{
  path: 'authtoken/:token',
  component: AuthtokenComponent,
  data: {
    expectedRoles: [],
  },
}, ];

const routes: Routes = [{
    path: '',
    component: LandingSiteLayoutComponent,
    children: landingSiteRoutes,
  },
  {
    path: 'default-layout',
    component: DefaultLayoutComponent,
    children: defaultRoutes,
  },
  {
    path: 'app-login',
    component: LoginComponent,
  },
  {
    path: 'reset-password',
    component: ResetPasswordComponent
  },
  {
    path: '**',
    component: LandingSiteLayoutComponent,
    children: landingSiteRoutes,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
