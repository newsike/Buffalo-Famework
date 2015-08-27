﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Buffalo.Executor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Buffalo Automation Engin");
            Console.WriteLine("Init Services");
            Buffalo.Core.Engine.BuffaloEngine engine = new Core.Engine.BuffaloEngine();
            engine.StartServices_ASY();
            ConsoleColor preColor= Console.ForegroundColor;
            Console.ForegroundColor=ConsoleColor.Yellow;
            Console.Write("Load Case File:");
            string testcaseFile = Console.ReadLine();
            FileStream fs=new FileStream(testcaseFile,FileMode.Open);
            StreamReader sr=new StreamReader(fs);
            string code=sr.ReadToEnd();
            sr.Close();
            fs.Close();
            engine.Action_InsertTask(code);           
            
        }
    }
}
