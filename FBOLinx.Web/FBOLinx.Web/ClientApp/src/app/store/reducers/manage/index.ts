import { createReducer, on } from '@ngrx/store';
import { userRoleSet, manageRoleSet } from '../../actions';

export const manageFeatureKey = 'manage';

export interface ManageState {
  role: number;
  currentRole: number;
}

const initialState: ManageState = {
  role: undefined,
  currentRole: undefined
};

export const manageReducer = createReducer(
    initialState,
    on(userRoleSet, (state, action) => ({
        ...state,
        role: action.role,
        currentRole: action.role
    })),
    on(manageRoleSet, (state, action) => ({
      ...state,
      currentRole: action.role
  }))
);
