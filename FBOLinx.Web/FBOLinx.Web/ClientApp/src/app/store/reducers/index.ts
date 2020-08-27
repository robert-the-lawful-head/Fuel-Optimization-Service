import { ActionReducerMap, MetaReducer} from "@ngrx/store";
import {
    pricingTemplateFeatureKey,
    PricingTemplateGridState,
    pricingTemplateReducer
} from "./pricing-template";

export interface State {
    [pricingTemplateFeatureKey]: PricingTemplateGridState;
}

export const reducers: ActionReducerMap<State> = {
    [pricingTemplateFeatureKey]: pricingTemplateReducer,
};

export const metaReducers: MetaReducer<State>[] = [];
