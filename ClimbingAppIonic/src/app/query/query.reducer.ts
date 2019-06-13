import { QueryActionTypes, QueryActions } from './query.actions';
import { createFeatureSelector } from '@ngrx/store';

export const STORE_FEATURE_QUERY = 'query';

export interface Image {
  base64: string;
}

export interface QueryState {
  image?: Image;
}

export const initialQueryState: QueryState = {
};

export const selectQueryState = createFeatureSelector<QueryState>(STORE_FEATURE_QUERY);

export function reducer(state = initialQueryState, action: QueryActions): QueryState {
  switch (action.type) {
    case QueryActionTypes.SetPhoto: {
      return {
        ...state,
        image: { base64: action.payload.base64String },
      };
    }

    default: {
      return state;
    }
  }
}
