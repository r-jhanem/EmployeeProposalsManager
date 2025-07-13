using System.Collections.Generic;

namespace CompalintsSystem.Core.Models
{
    public class SelectDataLocationDropdownsVM
    {
        public SelectDataLocationDropdownsVM()
        {

            Collegess = new List<Colleges>();
            Departmentss = new List<Departments>();
            SubDepartmentss = new List<SubDepartments>();
        }


        public List<Colleges> Collegess { get; set; }
        public List<Departments> Departmentss { get; set; }
        public List<SubDepartments> SubDepartmentss { get; set; }
    }
}
