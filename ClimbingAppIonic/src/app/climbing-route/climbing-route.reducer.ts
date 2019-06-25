import { Action, createFeatureSelector } from '@ngrx/store';
import { ClimbingRouteType } from '../shared/api';
import { ClimbingRouteActionTypes, ClimbingRouteActions } from './climbing-route.actions';

export const STORE_FEATURE_CLIMBING_ROUTE = 'climbing-route';

export interface ClimbingSite {
  id: string;
  name: string;
  description: string;
}

export interface SelectedClimbingRoute {
  id: string;
  name: string;
  description: string;
  grade: string;
  type: ClimbingRouteType;
  imageUri: string;
  site: ClimbingSite;
}

export interface ClimbingRouteState {
  selected: SelectedClimbingRoute;
}

export const selectClimbingRouteState = createFeatureSelector<ClimbingRouteState>(STORE_FEATURE_CLIMBING_ROUTE);

export const initialClimbingRouteState: ClimbingRouteState = {
  selected: {
    id: '',
    name: '',
    description: '',
    grade: '',
    type: 'SportClimbing',
    imageUri: '',
    site: {
      id: '',
      name: '',
      description: '',
    },
  },
};

export function reducer(state = initialClimbingRouteState, action: ClimbingRouteActions): ClimbingRouteState {
  switch (action.type) {
    case ClimbingRouteActionTypes.SetSelectedClimbingRoute: {
      return {
        ...state,
        selected: action.payload,
      };
    }

    default: {
      return state;
    }
  }
}
