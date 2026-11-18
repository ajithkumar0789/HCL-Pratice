export interface Department {
  id: number;
  departmentName: string;
  description: string | null;
}

export interface CreateDepartment {
  departmentName: string;
  description: string | null;
}