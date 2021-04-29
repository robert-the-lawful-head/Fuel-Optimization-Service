import { createAction, props } from '@ngrx/store';

export const groupGridSet = createAction(
    '[Group] Grid Set',
    props<{ filter: string }>()
);

export const groupGridClear = createAction(
    '[Group] Grid Clear'
);
