using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iText.Commons.Utils;

namespace API.Interfaces
{
    public interface ITestingService
    {
        public void PrintText();

        public bool FileExistTest(string name);


    }
}