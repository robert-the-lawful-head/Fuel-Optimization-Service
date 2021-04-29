import { createFeatureSelector, createSelector } from '@ngrx/store';

import { State } from '../../reducers';
import { groupFeatureKey, GroupGridState } from '../../reducers/group';

const selectGroupState = createFeatureSelector<State, GroupGridState>(groupFeatureKey);

export const getGroupGridState = createSelector(
    selectGroupState,
    (state: GroupGridState) => state
);
