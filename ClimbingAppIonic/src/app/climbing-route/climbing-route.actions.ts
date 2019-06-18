import { Action } from '@ngrx/store';

export enum ClimbingRouteActionTypes {
  LoadClimbingRoutes = '[ClimbingRoute] Load ClimbingRoutes',
  
  
}

export class LoadClimbingRoutes implements Action {
  readonly type = ClimbingRouteActionTypes.LoadClimbingRoutes;
}


export type ClimbingRouteActions = LoadClimbingRoutes;
