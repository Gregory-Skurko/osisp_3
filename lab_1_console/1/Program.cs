using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace SortingBigFile
{
    class Program
    {        
        static void Main(string[] args)
        {
            
            string fileName = Console.ReadLine();
            string fileName_before_sort = Console.ReadLine();
            string fileName_after_sort = Console.ReadLine();

            SortingBigFile.WriteBinaryFileToNumericFile(fileName, fileName_before_sort, sizeof(Int64));
            SortingBigFile.QuickSortInt64(fileName);
            SortingBigFile.WriteBinaryFileToNumericFile(fileName, fileName_after_sort, sizeof(Int64));
        }
    }
}
