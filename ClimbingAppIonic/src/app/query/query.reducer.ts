import { QueryActionTypes, QueryActions } from './query.actions';
import { createFeatureSelector } from '@ngrx/store';
import { CameraPhoto } from '@capacitor/core';

export const STORE_FEATURE_QUERY = 'query';

export interface QueryState {
  image?: CameraPhoto;
}

export const initialQueryState: QueryState = {
};

export const selectQueryState = createFeatureSelector<QueryState>(STORE_FEATURE_QUERY);

export function reducer(state = initialQueryState, action: QueryActions): QueryState {
  switch (action.type) {
    case QueryActionTypes.SetPhoto: {
      return {
        ...state,
        image: { ...action.payload },
      };
    }

    default: {
      return state;
    }
  }
}
