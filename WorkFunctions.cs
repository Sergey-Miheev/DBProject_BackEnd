using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Linq;
using System.Text;

namespace API;
class WorkFunctionsClass
{
    public static string Hashing(string password)
    {
        MD5 md5 = MD5.Create();

        byte[] b = Encoding.ASCII.GetBytes(password);
        byte[] hash = md5.ComputeHash(b);

        StringBuilder sb = new StringBuilder();
        foreach (var a in hash)
        {
            sb.Append(a.ToString("X2"));
        }

        return Convert.ToString(sb);
         
    }
}
