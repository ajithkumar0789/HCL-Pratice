
import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-department-management',
  standalone: true,
  imports: [RouterLink, RouterOutlet],
  templateUrl: './department-management.html',
  styleUrl: './department-management.css'
})
export class DepartmentManagement {}
