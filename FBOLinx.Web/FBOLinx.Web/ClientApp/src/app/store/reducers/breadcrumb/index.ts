import { createReducer, on } from '@ngrx/store';
import { breadcrumbSet } from '../../actions';

export const breadcrumbFeatureKey = 'breadcrumb';

export interface BreadcrumbState {
  breadcrumbs: any[];
}

const initialState: BreadcrumbState = {
  breadcrumbs: null,
};

export const breadcrumbReducer = createReducer(
    initialState,
    on(breadcrumbSet, (state, action) => ({
        ...state,
        breadcrumbs: action.breadcrumbs,
    }))
);
