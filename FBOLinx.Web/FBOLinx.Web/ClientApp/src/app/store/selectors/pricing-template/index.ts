import { createFeatureSelector, createSelector } from "@ngrx/store";

import { State } from "../../reducers";
import {pricingTemplateFeatureKey, PricingTemplateGridState} from "../../reducers/pricing-template";

const selectPricingTemplateState = createFeatureSelector<State, PricingTemplateGridState>(pricingTemplateFeatureKey);

export const getPricingTemplateState = createSelector(
    selectPricingTemplateState,
    (state: PricingTemplateGridState) => state
);
