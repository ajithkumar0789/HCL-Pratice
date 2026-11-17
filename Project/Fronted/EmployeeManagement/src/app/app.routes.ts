import { Routes } from '@angular/router';
import { AddEmployeeComponent } from './components/add-employee/add-employee';
import { EmployeeListComponent } from './components/employee-list/employee-list';

const routes: Routes = [
  {
    path: 'add-employee',
    component: AddEmployeeComponent
  },
  {
    path: 'employees-list',
    component: EmployeeListComponent
  },
  {
    path: '',
    redirectTo: 'add-employee',
    pathMatch: 'full'
  }
];

export default routes;