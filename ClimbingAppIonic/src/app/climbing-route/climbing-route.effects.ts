import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import {
  LoadClimbingSitesAction,
  ClimbingRouteActionTypes,
  SetClimbingSitesAction,
  CreateClimbingRouteAction,
  CreateClimbingRouteActionPayload,
  SetSelectedClimbingRouteAction,
} from './climbing-route.actions';
import { switchMap, map, startWith, catchError, withLatestFrom, flatMap } from 'rxjs/operators';
import { ClimbingRouteService, ClimbingSiteResponse, Image, CreateClimbingRouteRequest } from '../shared/api';
import { of, empty, concat, Observable } from 'rxjs';
import { DecreaseRequestSemaphoreAction, IncreaseRequestSemaphoreAction } from '../app.actions';
import { State } from '../app.reducer';
import { Store, select } from '@ngrx/store';
import { selectClimbingRouteState } from './climbing-route.reducer';
import { GoAction } from '../router.actions';



@Injectable()
export class ClimbingRouteEffects {

  @Effect() loadClimbingSites$ = this.actions$.pipe(
    ofType<LoadClimbingSitesAction>(ClimbingRouteActionTypes.LoadClimbingSites),
    switchMap(() =>
      concat(
        this.climbingRouteService.getClimbingSites().pipe(
          map(response => new SetClimbingSitesAction(response)),
          startWith(new IncreaseRequestSemaphoreAction()),
          catchError((error) => {
            console.log(JSON.stringify(error));
            return empty();
          }),
        ),
        of(new DecreaseRequestSemaphoreAction())
      ),
    ),
  );

  @Effect() createClimbingRoute$ = this.actions$.pipe(
    ofType<CreateClimbingRouteAction>(ClimbingRouteActionTypes.CreateClimbingRoute),
    switchMap(a => this.getOrCreateClimbingSite(a.payload.siteId, a.payload.siteName).pipe(
      withLatestFrom(this.store$.pipe(
        select(s => selectClimbingRouteState(s).proposed.image),
      )),
      switchMap(([site, image]) => this.climbingRouteService.createClimbingRoute(
          site.id,
          this.mapToCreateClimbingRouteRequest(a.payload, image),
        ).pipe(
          flatMap(response => [
            new SetSelectedClimbingRouteAction({ ...response, site: site }),
            new GoAction({ path: ['/sites', 'routes', response.id] }),
          ]),
        )
      ),
      catchError(() => empty()),
    )),
  );

  constructor(private actions$: Actions, private store$: Store<State>, private climbingRouteService: ClimbingRouteService) {}

  private getOrCreateClimbingSite(siteId: string, siteName: string): Observable<ClimbingSiteResponse> {
    if (siteId === '') {
      return this.climbingRouteService.createClimbingSite({ name: siteName });
    } else {
      return this.store$.pipe(
        select(s => selectClimbingRouteState(s).climbingSites),
        map(sites => sites.find(s => s.id === siteId)),
      );
    }
  }

  private mapToCreateClimbingRouteRequest(data: CreateClimbingRouteActionPayload, image: Image): CreateClimbingRouteRequest {
    return {
      name: data.name,
      description: data.description,
      type: data.type,
      grade: data.grade,
      image: image,
    };
  }
}
