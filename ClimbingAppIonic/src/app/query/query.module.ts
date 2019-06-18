import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { QueryEffects } from './query.effects';
import * as queryReducer from './query.reducer';
import { AnalyzingPage } from './analyzing';
import { QueryRoutingModule } from './query.router.module';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    IonicModule,
    StoreModule.forFeature(queryReducer.STORE_FEATURE_QUERY, queryReducer.reducer),
    EffectsModule.forFeature([QueryEffects]),
    QueryRoutingModule,
  ],
  declarations: [AnalyzingPage],
})
export class QueryModule { }
