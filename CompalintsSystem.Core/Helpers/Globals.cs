using ComplantSystem.Service.Helpers;
using System.Collections.Generic;

namespace ComplantSystem
{
    public static class Globals
    {
        public static List<RolesList> RolesLists { get; set; } = new List<RolesList>
        {
            new RolesList{ Id = 1 , Name = "الادارة العامة"},
            new RolesList{ Id = 2 , Name = "عميد كلية"},
            new RolesList{ Id = 3 , Name = "رئيس قسم"},
            new RolesList{ Id = 4 , Name = "موظف شكوى"},
            new RolesList{ Id = 5 , Name = "طالب"}
        };
    }
}
