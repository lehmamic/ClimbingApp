import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { CameraPhoto, CameraResultType, CameraSource } from '@capacitor/core';
import { ImageRecognitionQueryRequest, ClimbingRouteService } from '../shared/api';
import { TakePhotoAction, QueryImageRecognitionAction, QueryActionTypes, SetPhotoAction, OpenAnalyzingModalAction } from './query.actions';
import { switchMap, flatMap, catchError, map, tap } from 'rxjs/operators';
import { empty, defer } from 'rxjs';
import { Camera } from '../shared/native/camera';
import { ModalController } from '@ionic/angular';
import { AnalyzingPage } from './analyzing';

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
          return [
            new SetPhotoAction(photo),
            new OpenAnalyzingModalAction(),
            // new QueryImageRecognitionAction(photo),
          ];
        }),
        catchError(() => empty()),
    )),
  );

  @Effect() openAnalyzingModal$ = this.actions$.pipe(
    ofType<OpenAnalyzingModalAction>(QueryActionTypes.OpenAnalyzingModal),
    flatMap(() => defer(() => this.modalController.create({ component: AnalyzingPage }))),
    tap(modal => modal.present()),
    flatMap(() => []),
  );

  @Effect() queryImageRecognition$ = this.actions$.pipe(
    ofType<QueryImageRecognitionAction>(QueryActionTypes.QueryImageRecognition),
    map(a => mapPhotoToQuery(a.payload)),
    switchMap((dto) => this.climbingRouteService.query(dto).pipe(
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
    private climbingRouteService: ClimbingRouteService,
    private modalController: ModalController) {}
}

function mapPhotoToQuery(photo: CameraPhoto): ImageRecognitionQueryRequest {
  return {
    image: {
      base64: photo.base64String,
    },
  };
}
