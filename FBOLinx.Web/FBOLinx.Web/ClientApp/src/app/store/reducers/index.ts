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

export interface State {
    [pricingTemplateFeatureKey]: PricingTemplateGridState;
    [customerFeatureKey]: CustomerGridState;
}

export const reducers: ActionReducerMap<State> = {
    [pricingTemplateFeatureKey]: pricingTemplateReducer,
    [customerFeatureKey]: customerReducer,
};

export const metaReducers: MetaReducer<State>[] = [];
