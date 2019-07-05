import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { State } from 'src/app/app.reducer';
import { selectClimbingRouteState } from '../climbing-route.reducer';
import { ClimbingSiteResponse } from 'src/app/shared/api';
import { Observable, Subject } from 'rxjs';
import { map, takeWhile, takeUntil } from 'rxjs/operators';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CreateClimbingRouteAction } from '../climbing-route.actions';

@Component({
  selector: 'app-create-climbing-route',
  templateUrl: './create-climbing-route.page.html',
  styleUrls: ['./create-climbing-route.page.scss'],
})
export class CreateClimbingRoutePage implements OnInit {
  public climbingSites$: Observable<ClimbingSiteResponse[]>;
  public imageUrl$: Observable<string>;
  public climbingRouteForm: FormGroup;

  private unsubscribe$ = new Subject();

  constructor(private store$: Store<State>, private formBuilder: FormBuilder) {
  }

  ngOnInit() {
    this.climbingRouteForm = this.formBuilder.group({
      siteId: [''],
      siteName: [''],
      name: ['', Validators.required],
      type: ['', Validators.required],
      grade: ['', Validators.required],
      description: [''],
    });

    this.climbingRouteForm.get('siteId')
      .valueChanges
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(value => {
        const control = this.climbingRouteForm.get('siteName');
        if ( value !== '') {
          control.disable();
          control.clearValidators();
        } else {
          control.enable();
          control.setValidators(Validators.required);
        }
      });

    this.imageUrl$ = this.store$.pipe(
      select(s => selectClimbingRouteState(s).proposed.image),
      map(image => `data:image/jpg;base64,${image.base64}`),
    );

    this.climbingSites$ = this.store$.pipe(
      select(s => selectClimbingRouteState(s).climbingSites),
    );
  }

  public createClimbingRoute(): void {
    this.store$.dispatch(new CreateClimbingRouteAction({
      siteId: this.climbingRouteForm.get('siteId').value,
      siteName: this.climbingRouteForm.get('siteName').value,
      name: this.climbingRouteForm.get('name').value,
      type: this.climbingRouteForm.get('type').value,
      grade: this.climbingRouteForm.get('grade').value,
      description: this.climbingRouteForm.get('description').value,
    }));
  }
}
