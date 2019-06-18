import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { CameraPhoto, CameraResultType, CameraSource } from '@capacitor/core';
import { ImageRecognitionQueryRequest, ClimbingRouteService } from '../shared/api';
import { TakePhotoAction, QueryImageRecognitionAction, QueryActionTypes, SetPhotoAction, OpenAnalyzingModalAction, CloseAnalyzingModalAction } from './query.actions';
import { switchMap, flatMap, catchError, map, tap, startWith } from 'rxjs/operators';
import { empty, defer, of, concat } from 'rxjs';
import { Camera } from '../shared/native/camera';
import { ModalController } from '@ionic/angular';
import { AnalyzingPage } from './analyzing';
import { IncreaseRequestSemaphoreAction, DecreaseRequestSemaphoreAction } from '../app.actions';
import { GoAction } from '../router.actions';
import { Action } from '@ngrx/store';

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
          ];
        }),
        catchError(() => empty()),
    )),
  );

  @Effect({ dispatch: false }) openAnalyzingModal$ = this.actions$.pipe(
    ofType<OpenAnalyzingModalAction>(QueryActionTypes.OpenAnalyzingModal),
    flatMap(() => defer(() => this.modalController.create({ component: AnalyzingPage }))),
    tap(modal => modal.present()),
    flatMap(() => []),
  );

  @Effect({ dispatch: false }) closeAnalyzingModal$ = this.actions$.pipe(
    ofType<CloseAnalyzingModalAction>(QueryActionTypes.CloseAnalyzingModal),
    flatMap(() => defer(() => this.modalController.getTop())),
    tap(modal => modal.dismiss()),
    flatMap(() => []),
  );

  @Effect() queryImageRecognition$ = this.actions$.pipe(
    ofType<Action>(QueryActionTypes.QueryImageRecognition, QueryActionTypes.CancelImageRecognition),
    switchMap((a) => a.type === QueryActionTypes.CancelImageRecognition ?
      of(
        new DecreaseRequestSemaphoreAction(),
        new CloseAnalyzingModalAction(),
        )
      :
      concat(
        this.climbingRouteService.query(mapPhotoToQuery((a as QueryImageRecognitionAction).payload)).pipe(
          flatMap(result => {
            if ( result.result === 'Match') {
              return [new GoAction({ path: ['/sites', 'routes', result.climbingSite.id]})];
            } else {
              return [new GoAction({ path: ['/sites', 'create-route']})];
            }
          }),
          startWith(new IncreaseRequestSemaphoreAction()),
          catchError((error) => {
            console.log(JSON.stringify(error));
            return empty();
          }),
        ),
        of(
          new DecreaseRequestSemaphoreAction(),
          new CloseAnalyzingModalAction(),
        )
      )
    )
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
