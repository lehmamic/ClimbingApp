import { QueryActionTypes, QueryActions } from './query.actions';
import { createFeatureSelector } from '@ngrx/store';
import { CameraPhoto } from '@capacitor/core';
import { ImageRecognitionQueryResponse } from '../shared/api';

export const STORE_FEATURE_QUERY = 'query';

export interface QueryState {
  image?: CameraPhoto;
  response: ImageRecognitionQueryResponse,
}

export const initialQueryState: QueryState = {
  response: {
    result: 'NoMatch',
    climbingRoute: {
      id: '',
      name: '',
      description: '',
      grade: '',
      type: 'SportClimbing',
      site: {
        id: '',
        name: '',
        description: '',
      },
    }
  },
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

    case QueryActionTypes.SetImageRecognitionQueryResult: {
      return {
        ...state,
        response: action.payload,
      };
    }

    default: {
      return state;
    }
  }
}
