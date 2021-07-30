import { createReducer, on } from '@ngrx/store';

import { pricingTemplateGridClear, pricingTemplateGridSet } from '../../actions';

export const pricingTemplateFeatureKey = 'pricing-template';

export interface PricingTemplateGridState {
    filter: string;
    page: number;
    order: string;
    orderBy: string;
}

const initialState: PricingTemplateGridState = {
    filter: null,
    order: null,
    orderBy: null,
    page: null,
};

export const pricingTemplateReducer = createReducer(
    initialState,
    on(pricingTemplateGridSet, (state, action) => ({
        ...state,
        filter: action.filter,
        order: action.order,
        orderBy: action.orderBy,
        page: action.page,
    })),
    on(pricingTemplateGridClear, state => ({
        ...state,
        ...initialState,
    }))
);
