using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sesssion1.Model
{
    public class Log
    {
        public int logID { get; set; }
        public int userID { get; set; }
        public DateTime timeLogin { get; set; }
        public DateTime? timeLogout { get; set; }
        public int crashID { get; set; }
        public string description { get; set; }
    }
}
