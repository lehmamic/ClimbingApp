import { Action } from '@ngrx/store';

export enum AppActionTypes {
  IncreaseRequestSemaphore = '[App] Increase Request Semaphore',
  DecreaseRequestSemaphore = '[App] Decrease Request Semaphore',
}

export class IncreaseRequestSemaphoreAction implements Action {
  readonly type = AppActionTypes.IncreaseRequestSemaphore;
}

export class DecreaseRequestSemaphoreAction implements Action {
  readonly type = AppActionTypes.DecreaseRequestSemaphore;
}

export type AppActions =
  IncreaseRequestSemaphoreAction |
  DecreaseRequestSemaphoreAction;
