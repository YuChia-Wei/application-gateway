import { Component } from '@angular/core';
import { EmployeeService } from './employee.service';
import { EmployeeViewModel } from './employeeViewModel';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass'],
})
export class AppComponent {
  title = 'test';
  employee: { empName: string | undefined } | undefined;

  constructor(private employeeService: EmployeeService) {}

  showEmp() {
    this.employeeService.getEmployeeInfo().subscribe(
      (data: EmployeeViewModel) =>
        (this.employee = {
          empName: data.empName,
        })
    );
  }

  ngOnInit(): void {
  }

  clickBtn() {
    this.showEmp();
    this.title = this.employee?.empName + '';
    console.log(Date.now().toString()+'_____'+this.title);
  }
}
