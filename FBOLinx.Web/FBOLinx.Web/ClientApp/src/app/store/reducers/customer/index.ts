import { createReducer, on } from '@ngrx/store';

import { customerGridClear, customerGridSet } from '../../actions';

export const customerFeatureKey = 'customer';

export interface CustomerGridState {
    filter: string;
    page: number;
    order: string;
    orderBy: string;
    filterType: number;
}

const initialState: CustomerGridState = {
    filter: null,
    filterType: null,
    order: null,
    orderBy: null,
    page: null,
};

export const customerReducer = createReducer(
    initialState,
    on(customerGridSet, (state, action) => ({
        ...state,
        filter: action.filter,
        filterType: action.filterType,
        order: action.order,
        orderBy: action.orderBy,
        page: action.page,
    })),
    on(customerGridClear, state => ({
        ...state,
        ...initialState,
    }))
);
