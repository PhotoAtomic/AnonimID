using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AnonimId.Core.Test
{
    internal class RandomString
    {
        private static string charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static string Create(int length)
        {
            return new string(
                Enumerable.Repeat(charset, length)
                  .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());

        }
    }
}
