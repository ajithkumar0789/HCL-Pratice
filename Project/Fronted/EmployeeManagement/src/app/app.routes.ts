
import { Routes } from '@angular/router';

import { Dashboard } from './components/dashboard/dashboard';
import { EmployeeManagement } from './components/employee-management/employee-management';
import { DepartmentManagement } from './components/department-management/department-management';

const routes: Routes = [

  {
    path: 'home',
    component: Dashboard
  },

  {
    path: 'employees',
    component: EmployeeManagement,
    children: [
      {
        path: 'add',
        loadComponent: () =>
          import('./components/add-employee/add-employee')
            .then(m => m.AddEmployeeComponent)
      },
      {
        path: 'list',
        loadComponent: () =>
          import('./components/employee-list/employee-list')
            .then(m => m.EmployeeListComponent)
      },
      {
        path: 'search',
        loadComponent: () =>
          import('./components/employee-list/employee-list')
            .then(m => m.EmployeeListComponent)
      },
      {
        path: '',
        redirectTo: 'list',
        pathMatch: 'full'
      }
    ]
  },

  {
    path: 'departments',
    component: DepartmentManagement,
    children: [
      {
        path: 'add',
        loadComponent: () =>
          import('./components/add-department/add-department')
            .then(m => m.AddDepartmentComponent)
      },
      {
        path: 'list',
        loadComponent: () =>
          import('./components/department-list/department-list')
            .then(m => m.DepartmentListComponent)
      },
      {
        path: '',
        redirectTo: 'list',
        pathMatch: 'full'
      }
    ]
  },

  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },

  {
    path: '**',
    redirectTo: 'home'
  }

];

export default routes;
