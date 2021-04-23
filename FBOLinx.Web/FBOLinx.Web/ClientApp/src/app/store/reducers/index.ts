import { ActionReducerMap, MetaReducer } from '@ngrx/store';
import { pricingTemplateFeatureKey, PricingTemplateGridState, pricingTemplateReducer, } from './pricing-template';
import { customerFeatureKey, CustomerGridState, customerReducer } from './customer';
import { groupFeatureKey, GroupGridState, groupReducer } from './group';

export interface State {
    [pricingTemplateFeatureKey]: PricingTemplateGridState;
    [customerFeatureKey]: CustomerGridState;
    [groupFeatureKey]: GroupGridState;
}

export const reducers: ActionReducerMap<State> = {
    [pricingTemplateFeatureKey]: pricingTemplateReducer,
    [customerFeatureKey]: customerReducer,
    [groupFeatureKey]: groupReducer,
};

export const metaReducers: MetaReducer<State>[] = [];
