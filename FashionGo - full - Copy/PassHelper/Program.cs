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
            string Oldpass = "khong noi";
            PasswordObject pass = new PasswordObject(Oldpass);
            Console.WriteLine("old:"+ Oldpass);
            Console.WriteLine("EndCode:" + pass.PassWord);
            Console.WriteLine("DeCode:" + pass.Decode(pass.PassWord));
            Console.ReadKey();
        }
    }
}
