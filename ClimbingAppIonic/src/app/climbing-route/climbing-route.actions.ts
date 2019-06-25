import { Action } from '@ngrx/store';
import { SelectedClimbingRoute } from './climbing-route.reducer';

export enum ClimbingRouteActionTypes {
  SetSelectedClimbingRoute = '[ClimbingRoute] Set Selected ClimbingRoute',
}

export class SetSelectedClimbingRouteAction implements Action {
  readonly type = ClimbingRouteActionTypes.SetSelectedClimbingRoute;

  constructor(public payload: SelectedClimbingRoute) { }
}

export type ClimbingRouteActions = SetSelectedClimbingRouteAction;
