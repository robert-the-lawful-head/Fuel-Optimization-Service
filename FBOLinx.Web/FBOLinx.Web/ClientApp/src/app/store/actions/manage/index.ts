import { createAction, props } from '@ngrx/store';

export const userRoleSet = createAction(
  '[MANAGE] User role Set',
  props<{ role: number }>()
);

export const manageRoleSet = createAction(
  '[MANAGE] Manage Set',
  props<{ role: number }>()
);
