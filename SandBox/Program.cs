﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SandBox
{
    class Program
    {
        static void Main(string[] args)
        {
            var multi = new MultiThread(10);
            multi.Perform();

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
