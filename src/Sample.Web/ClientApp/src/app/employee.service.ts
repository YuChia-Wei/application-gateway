import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { from, Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { EmployeeViewModel } from './employeeViewModel';

@Injectable({
  providedIn: 'root'
})

export class EmployeeService {

  constructor(
    private http: HttpClient
  ) { }

  public getEmployeeInfo() {
    return this.http.get<EmployeeViewModel>(`https://localhost:64430/employee/api/v1/sample`);
  }
}
