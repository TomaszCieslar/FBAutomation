﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FBAutomation.Models
{
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FbID { get; set; }

        public virtual ICollection<Assigment> Assigments { get; set; }


    }
}