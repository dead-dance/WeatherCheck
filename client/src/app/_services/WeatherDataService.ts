import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

import { map } from 'rxjs/operators';
import { Router } from '@angular/router';




@Injectable({
  providedIn: 'root'
})
export class WeatherDataService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private router: Router) { }


  getDistricts() {
    return this.http.get('https://raw.githubusercontent.com/strativ-dev/technical-screening-test/main/bd-districts.json');
  }

  getCoolestDistList(districts: any){    
    return this.http.post(this.baseUrl+ 'CheckValue/GetCoolestDistrict',  districts);
  }
  
  // getSBU(){    
  //   return this.http.get(this.baseUrl + 'employee/getSBU');
  // }

}
