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

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    StoreModule.forFeature(queryReducer.STORE_FEATURE_QUERY, queryReducer.reducer),
    EffectsModule.forFeature([QueryEffects]),
    QueryRoutingModule,
  ],
  declarations: [AnalyzingPage],
})
export class QueryModule { }
