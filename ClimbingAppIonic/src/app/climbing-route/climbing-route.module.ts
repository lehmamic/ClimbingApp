import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { ClimbingRouteRoutingModule } from './climbing-route.router.module';
import { IonicModule } from '@ionic/angular';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { ClimbingRouteEffects } from './climbing-route.effects';
import * as climbingRouteReducer from './climbing-route.reducer';
import { ClimbingRoutePage } from './climbing-route';
import { CreateClimbingRoutePage } from './create-climbing-route';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    ClimbingRoutePage,
    CreateClimbingRoutePage,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    ClimbingRouteRoutingModule,
    IonicModule,
    StoreModule.forFeature(climbingRouteReducer.STORE_FEATURE_CLIMBING_ROUTE, climbingRouteReducer.reducer),
    EffectsModule.forFeature([ClimbingRouteEffects]),
  ]
})
export class ClimbingRouteModule { }
