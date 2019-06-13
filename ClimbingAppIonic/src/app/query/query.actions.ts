import { Action } from '@ngrx/store';
import { CameraPhoto } from '@capacitor/core';

export enum QueryActionTypes {
  TakePhoto = '[Query] Take Photo',
  SetPhoto = '[Query] Set Photo',
  OpenAnalyzingModal = '[Query] Open Analyzing Modal',
  QueryImageRecognition = '[Query] Query Image Recognition',
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

export class QueryImageRecognitionAction implements Action {
  readonly type = QueryActionTypes.QueryImageRecognition;

  constructor(public payload: CameraPhoto) { }
}

export type QueryActions =
  TakePhotoAction |
  SetPhotoAction |
  QueryImageRecognitionAction;
