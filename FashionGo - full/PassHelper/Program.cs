using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            string Oldpass = "abc@2018";
            PasswordObject pass = new PasswordObject(Oldpass);
            Console.WriteLine("old:"+ Oldpass);
            Console.WriteLine("EndCode:" + pass.PassWord);
            PasswordObject p = new PasswordObject();
            Console.WriteLine("DeCode:" + p.Decode(pass.PassWord));
            Console.ReadKey();
        }
    }
}
