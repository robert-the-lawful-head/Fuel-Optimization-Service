import { createFeatureSelector, createSelector } from '@ngrx/store';

import { State } from '../../reducers';
import {manageFeatureKey, ManageState} from '../../reducers/manage';

const selectState = createFeatureSelector<State, ManageState>(manageFeatureKey);

export const getRoles = createSelector(
  selectState,
  (state: ManageState) => state
);
