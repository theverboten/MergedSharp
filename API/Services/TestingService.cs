using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;

namespace API.Services
{
    public class TestingService : ITestingService
    {
        public bool FileExistTest(string name)
        {

            //if (File.Exists(@"../API/wwwroot/files/" + name)) { return true; } else { return false; }
            if (File.Exists(@"wwwroot/files/" + name)) { Console.WriteLine(name + " does exist"); return true; } else { Console.WriteLine("File does not exist"); return false; }



        }

        public void PrintText()
        {
            Console.WriteLine("Text is printed from service");
        }

    }
}