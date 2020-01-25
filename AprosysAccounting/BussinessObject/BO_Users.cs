using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting.BussinessObject
{
    public class BO_Users
    {
        public int id { get; set; }
        
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string userId { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public bool isActive { get; set; }
        public int createdBy { get; set; }
        public int modifiedBy { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime modifiedOn { get; set; }

        public bool adminRights { get; set; }
    }

}