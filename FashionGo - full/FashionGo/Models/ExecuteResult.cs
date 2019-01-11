using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionGo.Models
{
    public class ExecuteResult
    {
        public bool IsOk { get; set; }
        public string Mess { get; set; }
        public object Data { get; set; }
    }
}