import { District,IDistrict } from '../models/districts';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { WeatherDataService } from '../_services/WeatherDataService';
import { FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'weatherInfo',
  templateUrl: './weatherInfo.component.html',
})
export class WeatherInfoComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  frmDist: IDistrict;
  distList: any[];
  coolestDistList: any;
  compareMessage: any;
  searchText = '';
  weatherCheckForm: FormGroup;

  constructor(public masterService: WeatherDataService, private router: Router, private http: HttpClient, private _formBuilder: FormBuilder) {
}

  ngOnInit() {
    this.createWeatherCheckForm();
    this.getDistList();
  }

  getDistList(){
    this.masterService.getDistricts().subscribe(response => {
      const data = response as any;
      this.distList = data.districts as IDistrict[];
    }, error => {
        console.log(error);
    });
  }

  getCoolestDistricts(){
    this.masterService.getCoolestDistList().subscribe(response => {
      this.coolestDistList = response
    }, error => {
        console.log(error);
    });
  }

  getTravelComparison()
  {
    if(this.weatherCheckForm.value.fromDistrict === "" || this.weatherCheckForm.value.fromDistrict === undefined)
    {
      alert("Select travel from location");
      return;
    }
    if(this.weatherCheckForm.value.toDistrict === "" || this.weatherCheckForm.value.toDistrict === undefined)
    {
      alert("Select Destination");
      return;
    }
    if(this.weatherCheckForm.value.fromDistrict === this.weatherCheckForm.value.toDistrict)
    {
      alert("Travel Location has to be different");
      return;
    }

    if(this.weatherCheckForm.value.travelDate === "" || this.weatherCheckForm.value.travelDate === null || this.weatherCheckForm.value.travelDate === undefined)
    {
      alert("Select Travel Date");
      return;
    }

    const f = this.distList.filter(x => x.id === this.weatherCheckForm.value.fromDistrict);
    var t = this.distList.filter(x => x.id === this.weatherCheckForm.value.toDistrict);

    this.masterService.getTravelComparison(f[0].lat, f[0].long, t[0].lat, t[0].long, this.weatherCheckForm.value.travelDate).subscribe(response => {
      debugger;
      this.compareMessage = response;
      alert(this.compareMessage);
    }, error => {
        console.log(error);
    });

  }

  createWeatherCheckForm() {
    this.weatherCheckForm = this._formBuilder.group({
      fromDistrict: new FormControl(''),
      toDistrict: new FormControl(''),
      travelDate: new FormControl(''),
    });
  }

}
