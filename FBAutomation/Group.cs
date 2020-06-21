using System;
using System.Collections.Generic;
using System.Text;

namespace FBAutomation
{
    public class Group
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }

        public string FbGroupID { get; set; }

        public virtual ICollection<Assigment> Assigments { get; set; }


    }
}
