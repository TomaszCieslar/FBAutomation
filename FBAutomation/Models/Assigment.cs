using System;
using System.Collections.Generic;
using System.Text;

namespace FBAutomation.Models
{
    public class Assigment
    {
        public int AssigmentID { get; set; }
        public int GroupID { get; set; }
        public int UserID { get; set; }

        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}
