
import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-employee-management',
  standalone: true,
  imports: [RouterLink, RouterOutlet],
  templateUrl: './employee-management.html',
  styleUrl: './employee-management.css'
})
export class EmployeeManagement {}
