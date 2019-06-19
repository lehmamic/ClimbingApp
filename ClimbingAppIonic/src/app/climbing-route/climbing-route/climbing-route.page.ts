import { Component, OnInit } from '@angular/core';
import { State } from 'src/app/app.reducer';
import { Store, select } from '@ngrx/store';
import { selectClimbingRouteState, SelectedClimbingRoute } from '../climbing-route.reducer';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-climbing-route',
  templateUrl: './climbing-route.page.html',
  styleUrls: ['./climbing-route.page.scss'],
})
export class ClimbingRoutePage implements OnInit {

  public climbingRoute$: Observable<SelectedClimbingRoute>;

  constructor(private store: Store<State>) { }

  ngOnInit() {
    this.climbingRoute$ = this.store.pipe(
      select((s) => selectClimbingRouteState(s).selected),
    );
  }

}
