import { createFeatureSelector, createSelector } from "@ngrx/store";

import { State } from "../../reducers";
import {breadcrumbFeatureKey, BreadcrumbState} from "../../reducers/breadcrumb";

const selectState = createFeatureSelector<State, BreadcrumbState>(breadcrumbFeatureKey);

export const getBreadcrumbs = createSelector(
  selectState,
  (state: BreadcrumbState) => state.breadcrumbs
);
