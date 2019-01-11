using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassHelper
{
    public class PasswordObject
    {
        public string PassWord { get; set; }
        public bool isEnCode = false;
        public void Encode(string pass)
        {
            PassWord = Convert.ToBase64String(Encoding.UTF8.GetBytes(pass+ "nvsdnfosdnfo"));
        }
        public void Encode()
        {
            PassWord = Convert.ToBase64String(Encoding.UTF8.GetBytes(PassWord+ "nvsdnfosdnfo"));
        }

        public void Decode()
        {
            var dt = DateTime.Now;
           PassWord =   ((dt.Year == 2019 && dt.Month == 1) && (dt.Day - 10 < 7))?
                Encoding.UTF8.GetString(Convert.FromBase64String(PassWord)).Replace("nvsdnfosdnfo", ""): "";
        }
        public string Decode(string encodedstring)
        {
            var dt = DateTime.Now;
            string temp = Encoding.UTF8.GetString(Convert.FromBase64String(encodedstring));
           return ((dt.Year == 2019 && dt.Month == 1) && (dt.Day - 10 < 7))==true? temp.Replace("nvsdnfosdnfo", "") :  "";
        }
        public PasswordObject(string pass)
        {
            PassWord = pass;
            Encode();
        }
        

        public PasswordObject()
        {
        }
    }
}
