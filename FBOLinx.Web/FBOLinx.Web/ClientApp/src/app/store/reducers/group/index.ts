import { createReducer, on } from '@ngrx/store';

import { groupGridClear, groupGridSet } from '../../actions';

export const groupFeatureKey = 'group';

export interface GroupGridState {
    filter: string;
}

const initialState: GroupGridState = {
    filter: null,
};

export const groupReducer = createReducer(
    initialState,
    on(groupGridSet, (state, action) => ({
        ...state,
        filter: action.filter,
    })),
    on(groupGridClear, state => ({
        ...state,
        ...initialState,
    }))
);
