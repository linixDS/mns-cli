using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core;
using MikroTik.Types;
using MikroTik.Client;
using System.Runtime.InteropServices;


namespace mikrotik
{
    public class CAPSMCLASS
    {
        public void help()
        {
            Console.Clear();
            Console.WriteLine("NAME");
            Console.WriteLine("\t MikroTik CAPsMANGER Controller");
            Console.WriteLine("");
            Console.WriteLine("SHORT DESCRIPTION");
            Console.WriteLine("\t");
            Console.WriteLine("");
            Console.WriteLine("SYNTEX");
            Console.WriteLine("\t mikrotik caps help");


            Console.WriteLine("");
        }
    }
}
