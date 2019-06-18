import { Action } from '@ngrx/store';

export const STORE_FEATURE_CLIMBING_ROUTE = 'climbing-route';

export interface ClimbingRouteState {

}

export const initialClimbingRouteState: ClimbingRouteState = {

};

export function reducer(state = initialClimbingRouteState, action: Action): ClimbingRouteState {
  switch (action.type) {

    default:
      return state;
  }
}
