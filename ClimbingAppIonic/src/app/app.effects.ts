import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { Plugins, CameraResultType, CameraSource, CameraPhoto } from '@capacitor/core';
import { from, Observable, empty } from 'rxjs';
import { flatMap, catchError, switchMap, map } from 'rxjs/operators';
import { TakePhotoAction, AppActionTypes, QueryImageRecognitionAction } from './app.actions';
import { Camera } from './shared/native';
import { ImageRecognitionService, ImageRecognitionQueryRequest } from './shared/api';

@Injectable()
export class AppEffects {

  @Effect() takePhoto$ = this.actions$.pipe(
    ofType<TakePhotoAction>(AppActionTypes.TakePhoto),
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
    ofType<QueryImageRecognitionAction>(AppActionTypes.QueryImageRecognition),
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
