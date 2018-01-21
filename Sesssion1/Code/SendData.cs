using Sesssion1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sesssion1.Code
{

    public delegate void GetData(object sender, SendData data);
    public delegate void DataSetToView(object sender, DataViewToForm view);
    public delegate void Check(object sender, CheckValidate validate);

    public class SendData : EventArgs
    {
        public User user { get; set; } 
    }

    public class CheckValidate : EventArgs
    {
        public bool check { get; set; }
    }

    public class DataViewToForm : EventArgs
    {
        public User user { get; set; }
    }
}
