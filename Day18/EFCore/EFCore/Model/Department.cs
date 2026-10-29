using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.Model
{
    public class Department
    {
        public int DepartmentId {  get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string? Location { get; set; }




    }
}
