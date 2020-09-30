import { createAction, props } from '@ngrx/store';

export const breadcrumbSet = createAction(
  '[Default Layout] Breadcrumb Set',
  props<{ breadcrumbs: any[] | null }>()
);
