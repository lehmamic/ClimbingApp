import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';

import { State } from 'src/app/app.reducer';
import { selectQueryState } from '../query.reducer';
import { map, filter } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-analyzing',
  templateUrl: './analyzing.page.html',
  styleUrls: ['./analyzing.page.scss'],
})
export class AnalyzingPage implements OnInit {
  public imageUrl$: Observable<string>;

  constructor(private store$: Store<State>) { }

  ngOnInit() {
    this.imageUrl$ = this.store$.pipe(
      select(s => selectQueryState(s).image),
      filter(image => image != null),
      map(image => `data:image/jpg;base64,${image.base64}`),
    );
  }
}
