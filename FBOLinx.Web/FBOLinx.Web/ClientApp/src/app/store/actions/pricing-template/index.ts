import { createAction, props } from "@ngrx/store";

export const pricingTemplateGridSet = createAction(
    "[Pricing Template] Grid Set",
    props<{ filter: string, page: number, order: string, orderBy: string }>()
);

export const pricingTemplateGridClear = createAction(
    "[Pricing Template] Grid Clear"
);
