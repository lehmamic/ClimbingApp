import { Action } from '@ngrx/store';
import { SelectedClimbingRoute, ProposedClimbingRoute } from './climbing-route.reducer';
import { ClimbingSiteResponse, ClimbingRouteType } from '../shared/api';

export enum ClimbingRouteActionTypes {
  SetSelectedClimbingRoute = '[ClimbingRoute] Set Selected ClimbingRoute',
  SetProposedClimbingRoute = '[ClimbingRoute] Set Proposed ClimbingRoute',
  LoadClimbingSites = '[ClimbingRoute] Load ClimbingSites',
  SetClimbingSites = '[ClimbingRoute] Set ClimbingSites',
  CreateClimbingSite = '[ClimbingRoute] Create ClimbingSite',
  CreateClimbingRoute = '[ClimbingRoute] Create ClimbingRoute',
}

export class SetSelectedClimbingRouteAction implements Action {
  readonly type = ClimbingRouteActionTypes.SetSelectedClimbingRoute;

  constructor(public payload: SelectedClimbingRoute) { }
}

export class LoadClimbingSitesAction implements Action {
  readonly type = ClimbingRouteActionTypes.LoadClimbingSites;
}

export class SetClimbingSitesAction implements Action {
  readonly type = ClimbingRouteActionTypes.SetClimbingSites;

  constructor(public payload: ClimbingSiteResponse[]) { }
}

export class SetProposedClimbingRouteAction implements Action {
  readonly type = ClimbingRouteActionTypes.SetProposedClimbingRoute;

  constructor(public payload: ProposedClimbingRoute) { }
}

export interface CreateClimbingRouteActionPayload {
  siteId: string;
  siteName: string;
  name: string;
  description: string;
  grade: string;
  type: ClimbingRouteType;
}

export class CreateClimbingRouteAction implements Action {
  readonly type = ClimbingRouteActionTypes.CreateClimbingRoute;

  constructor(public payload: CreateClimbingRouteActionPayload) { }
}

export type ClimbingRouteActions =
  SetSelectedClimbingRouteAction |
  SetProposedClimbingRouteAction |
  LoadClimbingSitesAction |
  SetClimbingSitesAction |
  CreateClimbingRouteAction;
