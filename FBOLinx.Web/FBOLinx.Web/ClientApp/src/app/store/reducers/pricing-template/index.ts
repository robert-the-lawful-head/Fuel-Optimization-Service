import { createReducer, on } from '@ngrx/store';
import { pricingTemplateGridSet, pricingTemplateGridClear } from '../../actions';

export const pricingTemplateFeatureKey = 'pricing-template';

export interface PricingTemplateGridState {
    filter: string;
    page: number;
    order: string;
    orderBy: string;
}

const initialState: PricingTemplateGridState = {
    filter: null,
    page: null,
    order: null,
    orderBy: null,
};

export const pricingTemplateReducer = createReducer(
    initialState,
    on(pricingTemplateGridSet, (state, action) => ({
        ...state,
        filter: action.filter,
        page: action.page,
        order: action.order,
        orderBy: action.orderBy,
    })),
    on(pricingTemplateGridClear, state => ({
        ...state,
        ...initialState,
    }))
);
