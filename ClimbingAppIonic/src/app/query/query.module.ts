import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EffectsModule } from '@ngrx/effects';
import { QueryEffects } from './query.effects';
import * as queryReducer from './query.reducer';
import { StoreModule } from '@ngrx/store';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    StoreModule.forFeature('query', queryReducer.reducer),
    EffectsModule.forFeature([QueryEffects]),
  ],
})
export class QueryModule { }
