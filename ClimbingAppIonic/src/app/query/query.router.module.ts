import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AnalyzingPage } from './analyzing';

const routes: Routes = [
  {
    path: 'analyzing',
    component: AnalyzingPage
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class QueryRoutingModule {}
