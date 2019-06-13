import { Component } from '@angular/core';
import { State } from '../app.reducer';
import { Store } from '@ngrx/store';
import { TakePhotoAction } from '../query';

@Component({
  selector: 'app-tabs',
  templateUrl: 'tabs.page.html',
  styleUrls: ['tabs.page.scss']
})
export class TabsPage {
  constructor(private store$: Store<State>) { }

  public takePhoto(): void {
    this.store$.dispatch(new TakePhotoAction());
  }
}
