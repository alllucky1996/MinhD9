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
        public void Encode(string pass)
        {
            PassWord = Convert.ToBase64String(Encoding.UTF8.GetBytes(pass));
        }
        public void Encode()
        {
            PassWord = Convert.ToBase64String(Encoding.UTF8.GetBytes(PassWord));
        }

        public void Decode()
        {
            PassWord = Encoding.UTF8.GetString(Convert.FromBase64String(PassWord));
        }
        public string Decode(string encodedstring)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedstring));
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
