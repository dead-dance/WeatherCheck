import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WeatherInfoComponent } from './weatherInfo/weatherInfo.component';

const routes: Routes = [
  {path: 'weather', component: WeatherInfoComponent}, 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }