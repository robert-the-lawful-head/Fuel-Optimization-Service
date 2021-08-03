import { ActionReducerMap, MetaReducer } from '@ngrx/store';

import {
    customerFeatureKey,
    CustomerGridState,
    customerReducer,
} from './customer';
import { groupFeatureKey, GroupGridState, groupReducer } from './group';
import {
    pricingTemplateFeatureKey,
    PricingTemplateGridState,
    pricingTemplateReducer,
} from './pricing-template';

export interface State {
    [pricingTemplateFeatureKey]: PricingTemplateGridState;
    [customerFeatureKey]: CustomerGridState;
    [groupFeatureKey]: GroupGridState;
}

export const reducers: ActionReducerMap<State> = {
    [customerFeatureKey]: customerReducer,
    [groupFeatureKey]: groupReducer,
    [pricingTemplateFeatureKey]: pricingTemplateReducer,
};

export const metaReducers: MetaReducer<State>[] = [];
