import { Action } from '@ngrx/store';
import { CameraPhoto } from '@capacitor/core';

export enum QueryActionTypes {
  TakePhoto = '[Query] Take Photo',
  QueryImageRecognition = '[Query] Query Image Recognition',
}

export class TakePhotoAction implements Action {
  readonly type = QueryActionTypes.TakePhoto;
}

export class QueryImageRecognitionAction implements Action {
  readonly type = QueryActionTypes.QueryImageRecognition;

  constructor(public payload: CameraPhoto) { }
}

export type QueryActions =
  TakePhotoAction |
  QueryImageRecognitionAction;
