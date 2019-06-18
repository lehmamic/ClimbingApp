import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store, select } from '@ngrx/store';

import { State } from 'src/app/app.reducer';
import { selectQueryState } from '../query.reducer';
import { map, filter, takeUntil } from 'rxjs/operators';
import { Observable, Subject } from 'rxjs';
import { QueryImageRecognitionAction, CancelQueryImageRecognitionAction } from '../query.actions';
import { CameraPhoto } from '@capacitor/core';

@Component({
  selector: 'app-analyzing',
  templateUrl: './analyzing.page.html',
  styleUrls: ['./analyzing.page.scss'],
})
export class AnalyzingPage implements OnInit, OnDestroy{
  public imageUrl$: Observable<string>;
  public requestPending$: Observable<boolean>;
  private notifier$ = new Subject();
  private photo: CameraPhoto;

  constructor(private store$: Store<State>) { }

  ngOnInit() {
    this.imageUrl$ = this.store$.pipe(
      select(s => selectQueryState(s).image),
      filter(image => image != null),
      map(image => `data:image/jpg;base64,${image.base64String}`),
    );

    this.store$.pipe(
      select(s => selectQueryState(s).image),
      takeUntil(this.notifier$)
    ).subscribe(i => {
      this.photo = i;
    });

    this.requestPending$ = this.store$.pipe(
      select(s => s.app.request.isPending),
    );
  }

  ngOnDestroy(): void {
    this.notifier$.next();
  }

  public queryClimbingRoute(): void {
    this.store$.dispatch(new QueryImageRecognitionAction(this.photo));
  }

  public cancelQuery(): void {
    this.store$.dispatch(new CancelQueryImageRecognitionAction());
  }
}
