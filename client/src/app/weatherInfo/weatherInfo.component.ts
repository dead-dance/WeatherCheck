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
  distList: any[];
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
    this.masterService.getCoolestDistList(this.distList).subscribe(response => {
      const data = response as any;
      this.distList = data.districts as IDistrict[];
    }, error => {
        console.log(error);
    });
  }

  createWeatherCheckForm() {
    this.weatherCheckForm = this._formBuilder.group({
      districtLoad: new FormControl(''),
      fromDistrict: new FormControl(''),
      toDistrict: new FormControl(''),
      travelDate: new FormControl(''),
    });
  }

  // onSubmit(form: NgForm) {
  //   debugger;
  //   if (this.masterService.bcdsFormData.id == 0)
  //     this.insertBcds(form);
  //   else
  //     this.updateBcds(form);
  // }



  resetSearch(){
    this.searchText = '';
}


  // resetForm(form: NgForm) {
  //   form.form.reset();
  //   this.masterService.bcdsFormData = new BcdsInfo();
  // }
  // resetForm(form: NgForm) {
  //   this.searchText = '';
  //   form.reset();
  //   this.config = {
  //     currentPage: 1,
  //     itemsPerPage: 10,
  //     totalItems:50,
  //     };
  // }
  // resetPage() {
  //   this.masterService.bcdsFormData=new BcdsInfo();
  //   this.config = {
  //     currentPage: 1,
  //     itemsPerPage: 10,
  //     totalItems:50,
  //     };
  // }
}
