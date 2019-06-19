import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { CameraPhoto, CameraResultType, CameraSource } from '@capacitor/core';
import { ImageRecognitionQueryRequest, ClimbingRouteService } from '../shared/api';
import {
  TakePhotoAction,
  QueryImageRecognitionAction,
  QueryActionTypes,
  SetPhotoAction,
  OpenAnalyzingModalAction,
  CloseAnalyzingModalAction,
  SetImageRecognitionQueryResultAction,
} from './query.actions';
import { switchMap, flatMap, catchError, tap, startWith } from 'rxjs/operators';
import { empty, defer, of, concat } from 'rxjs';
import { Camera } from '../shared/native/camera';
import { ModalController } from '@ionic/angular';
import { AnalyzingPage } from './analyzing';
import { IncreaseRequestSemaphoreAction, DecreaseRequestSemaphoreAction } from '../app.actions';
import { GoAction } from '../router.actions';
import { Action } from '@ngrx/store';
import { SetSelectedClimbingRouteAction } from '../climbing-route/climbing-route.actions';

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
        flatMap(photo => [
          new SetPhotoAction(photo),
          new OpenAnalyzingModalAction(),
        ]),
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
    switchMap((a) =>
      concat(
        a.type === QueryActionTypes.CancelImageRecognition ?
          of()
          :
          this.climbingRouteService.query(mapPhotoToQuery((a as QueryImageRecognitionAction).payload)).pipe(
            flatMap(result => {
              if ( result.result === 'Match') {
                return [
                  new SetImageRecognitionQueryResultAction(result),
                  new SetSelectedClimbingRouteAction(result.climbingRoute),
                  new GoAction({ path: ['/sites', 'routes', result.climbingRoute.id]}),
                ];
              } else {
                return [
                  new SetImageRecognitionQueryResultAction(result),
                  new GoAction({ path: ['/sites', 'create-route']})
                ];
              }
            }),
            startWith(new IncreaseRequestSemaphoreAction()),
            catchError((error) => {
              console.log(JSON.stringify(error));
              return empty();
            })
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
