import {
  ActionReducer,
  ActionReducerMap,
  createFeatureSelector,
  createSelector,
  MetaReducer
} from '@ngrx/store';
import { environment } from '../environments/environment';
import { routerReducer, RouterReducerState } from '@ngrx/router-store';
import { AppActions, AppActionTypes } from './app.actions';

export interface RequestState {
  semaphore: number;
  isPending: boolean;
}

export interface AppState {
  request: RequestState;
}

export const initialAppState: AppState = {
  request: {
    semaphore: 0,
    isPending: false,
  },
};

export interface State {
  app: AppState;
  router: RouterReducerState;
}

export function appReducer(state = initialAppState, action: AppActions): AppState {
  switch (action.type) {
    case AppActionTypes.IncreaseRequestSemaphore: {
      const newSemaphoreValue = state.request.semaphore + 1;

      return {
        ...state,
        request: {
          semaphore: newSemaphoreValue,
          isPending: newSemaphoreValue > 0,
        },
      };
    }

    case AppActionTypes.DecreaseRequestSemaphore: {
      const newSemaphoreValue = state.request.semaphore - 1;

      return {
        ...state,
        request: {
          semaphore: newSemaphoreValue,
          isPending: newSemaphoreValue > 0,
        },
      };
    }

    default: {
      return state;
    }
  }
}

export const reducers: ActionReducerMap<State> = {
    app: appReducer,
    router: routerReducer,
};

export const metaReducers: MetaReducer<State>[] = !environment.production ? [] : [];
