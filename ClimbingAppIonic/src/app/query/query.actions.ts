import { Action } from '@ngrx/store';
import { CameraPhoto } from '@capacitor/core';
import { ImageRecognitionQueryRequest, ImageRecognitionQueryResponse } from '../shared/api';

export enum QueryActionTypes {
  TakePhoto = '[Query] Take Photo',
  SetPhoto = '[Query] Set Photo',
  OpenAnalyzingModal = '[Query] Open Analyzing Modal',
  CloseAnalyzingModal = '[Query] Close Analyzing Modal',
  QueryImageRecognition = '[Query] Query Image Recognition',
  CancelImageRecognition = '[Query] Cancel Query Image Recognition',
  SetImageRecognitionQueryResult = '[Query] Set Image Recognition Query REsult',
}

export class TakePhotoAction implements Action {
  readonly type = QueryActionTypes.TakePhoto;
}

export class SetPhotoAction implements Action {
  readonly type = QueryActionTypes.SetPhoto;

  constructor(public payload: CameraPhoto) { }
}

export class OpenAnalyzingModalAction implements Action {
  readonly type = QueryActionTypes.OpenAnalyzingModal;
}

export class CloseAnalyzingModalAction implements Action {
  readonly type = QueryActionTypes.CloseAnalyzingModal;
}

export class QueryImageRecognitionAction implements Action {
  readonly type = QueryActionTypes.QueryImageRecognition;

  constructor(public payload: CameraPhoto) { }
}

export class SetImageRecognitionQueryResultAction implements Action {
  readonly type = QueryActionTypes.SetImageRecognitionQueryResult;

  constructor(public payload: ImageRecognitionQueryResponse) { }
}

export class CancelQueryImageRecognitionAction implements Action {
  readonly type = QueryActionTypes.CancelImageRecognition;
}

export type QueryActions =
  TakePhotoAction |
  SetPhotoAction |
  OpenAnalyzingModalAction |
  CloseAnalyzingModalAction |
  QueryImageRecognitionAction |
  SetImageRecognitionQueryResultAction |
  CancelQueryImageRecognitionAction;
