import { ActionReducerMap, MetaReducer} from "@ngrx/store";
import {
    pricingTemplateFeatureKey,
    PricingTemplateGridState,
    pricingTemplateReducer,
} from "./pricing-template";
import {
    customerFeatureKey,
    CustomerGridState,
    customerReducer
} from "./customer";
import {
    breadcrumbFeatureKey,
    BreadcrumbState,
    breadcrumbReducer
} from "./breadcrumb";

export interface State {
    [pricingTemplateFeatureKey]: PricingTemplateGridState;
    [customerFeatureKey]: CustomerGridState;
    [breadcrumbFeatureKey]: BreadcrumbState;
}

export const reducers: ActionReducerMap<State> = {
    [pricingTemplateFeatureKey]: pricingTemplateReducer,
    [customerFeatureKey]: customerReducer,
    [breadcrumbFeatureKey]: breadcrumbReducer,
};

export const metaReducers: MetaReducer<State>[] = [];
