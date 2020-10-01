import { ActionReducerMap, MetaReducer} from '@ngrx/store';
import {
    pricingTemplateFeatureKey,
    PricingTemplateGridState,
    pricingTemplateReducer,
} from './pricing-template';
import {
    customerFeatureKey,
    CustomerGridState,
    customerReducer
} from './customer';
import {
    breadcrumbFeatureKey,
    BreadcrumbState,
    breadcrumbReducer
} from './breadcrumb';
import {
    manageFeatureKey,
    ManageState,
    manageReducer
} from './manage';

export interface State {
    [pricingTemplateFeatureKey]: PricingTemplateGridState;
    [customerFeatureKey]: CustomerGridState;
    [breadcrumbFeatureKey]: BreadcrumbState;
    [manageFeatureKey]: ManageState;
}

export const reducers: ActionReducerMap<State> = {
    [pricingTemplateFeatureKey]: pricingTemplateReducer,
    [customerFeatureKey]: customerReducer,
    [breadcrumbFeatureKey]: breadcrumbReducer,
    [manageFeatureKey]: manageReducer
};

export const metaReducers: MetaReducer<State>[] = [];
