import { createFeatureSelector, createSelector } from '@ngrx/store';

import { State } from '../../reducers';
import { customerFeatureKey, CustomerGridState } from '../../reducers/customer';

const selectCustomerState = createFeatureSelector<State, CustomerGridState>(
    customerFeatureKey
);

export const getCustomerGridState = createSelector(
    selectCustomerState,
    (state: CustomerGridState) => state
);
