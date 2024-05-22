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

            if (File.Exists(@"../API/wwwroot/files/" + name)) { return true; } else { return false; }


        }

        public void PrintText()
        {
            Console.WriteLine("Text is printed from service");
        }

    }
}