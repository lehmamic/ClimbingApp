import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { CameraPhoto, CameraResultType, CameraSource } from '@capacitor/core';
import { ImageRecognitionService, ImageRecognitionQueryRequest } from '../shared/api';
import { TakePhotoAction, QueryImageRecognitionAction, QueryActionTypes } from './query.actions';
import { switchMap, flatMap, catchError, map } from 'rxjs/operators';
import { empty } from 'rxjs';
import { Camera } from '../shared/native/camera';

@Injectable()
export class QueryEffects {
  @Effect() takePhoto$ = this.actions$.pipe(
    ofType<TakePhotoAction>(QueryActionTypes.TakePhoto),
    switchMap(() => this.camera.getPhoto({
        quality: 100,
        allowEditing: false,
        resultType: CameraResultType.Base64,
        source: CameraSource.Prompt,
      }).pipe(
        flatMap(photo => {
          console.log(JSON.stringify(photo));
          return [new QueryImageRecognitionAction(photo)];
        }),
        catchError(() => empty()),
    )),
  );

  @Effect() queryImageRecognition$ = this.actions$.pipe(
    ofType<QueryImageRecognitionAction>(QueryActionTypes.QueryImageRecognition),
    map(a => mapPhotoToQuery(a.payload)),
    switchMap((dto) => this.imageRecognitionService.query(dto).pipe(
      flatMap(result => {
        console.log(JSON.stringify(result));
        return [];
      }),
      catchError((error) => {
        console.log(JSON.stringify(error));
        return empty();
      }),
    ))
  );

  constructor(
    private actions$: Actions,
    private camera: Camera,
    private imageRecognitionService: ImageRecognitionService) {}
}

function mapPhotoToQuery(photo: CameraPhoto): ImageRecognitionQueryRequest {
  return {
    image: {
      base64: photo.base64String,
    },
  };
}
