import { createAction, props } from '@ngrx/store';

export const customerGridSet = createAction(
    '[Customer] Grid Set',
    props<{ filter: string; page: number; order: string; orderBy: string; filterType: number }>()
);

export const customerGridClear = createAction(
    '[Customer] Grid Clear'
);
