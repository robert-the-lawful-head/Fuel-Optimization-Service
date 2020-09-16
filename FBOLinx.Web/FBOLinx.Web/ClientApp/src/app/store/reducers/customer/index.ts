import { createReducer, on } from "@ngrx/store";
import { customerGridSet, customerGridClear } from "../../actions";

export const customerFeatureKey = "customer";

export interface CustomerGridState {
    filter: string;
    page: number;
    order: string;
    orderBy: string;
    filterType: number;
}

const initialState: CustomerGridState = {
    filter: null,
    page: null,
    order: null,
    orderBy: null,
    filterType: null,
};

export const customerReducer = createReducer(
    initialState,
    on(customerGridSet, (state, action) => ({
        ...state,
        filter: action.filter,
        page: action.page,
        order: action.order,
        orderBy: action.orderBy,
        filterType: action.filterType,
    })),
    on(customerGridClear, state => ({
        ...state,
        ...initialState,
    }))
);
