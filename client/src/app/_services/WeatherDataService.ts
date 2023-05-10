import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
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

  getCoolestDistList(){    
    return this.http.get(this.baseUrl+ 'CheckValue/GetCoolestDistrict');
  }
  
  getTravelComparison(flat: number, flong: number, tlat: number, tlong: number, travelDate: Date){    
    return this.http.get(this.baseUrl+ 'CheckValue/GetComparedData/'+flat+'/'+flong+'/'+tlat+'/'+tlong+'/'+travelDate, {responseType: 'text'});
  }

}
