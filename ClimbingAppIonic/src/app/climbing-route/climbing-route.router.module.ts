import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClimbingRoutePage } from './climbing-route/climbing-route.page';
import { CreateClimbingRoutePage } from './create-climbing-route';

const routes: Routes = [
  {
    path: 'routes/:routeId',
    component: ClimbingRoutePage,
  },
  {
    path: 'create-route',
    component: CreateClimbingRoutePage,
  },
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class ClimbingRouteRoutingModule {}
