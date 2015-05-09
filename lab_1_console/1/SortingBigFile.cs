using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingBigFile
{
    static class SortingBigFile
    {
        static public void CountingSortByte(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            UInt64[] countNumbers = new UInt64[256];

            using (var memoryMap = MemoryMappedFile.CreateFromFile(fileName, FileMode.Open))
            {
                for (Int64 i = 0; i < fileInfo.Length; ++i)
                {
                    using (var accessor = memoryMap.CreateViewAccessor(i, sizeof(Byte)))
                    {
                        ++countNumbers[accessor.ReadByte(0)];
                    }
                }

                Int64 offset = 0;
                for (Int32 i = 0; i < countNumbers.Length; ++i)
                {
                    for (UInt64 j = 0; j < countNumbers[i]; ++j)
                    {
                        using (var accessor = memoryMap.CreateViewAccessor(offset, sizeof(Byte)))
                        {
                            accessor.Write(0, (Byte)i);
                        }
                        ++offset;
                    }
                }
            }
        }

        static public void QuickSortInt64(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);

            using (var memoryMap = MemoryMappedFile.CreateFromFile(fileName, FileMode.Open))
            {
                QuickSortInt64(memoryMap, 0, (fileInfo.Length >> 3) - 1);
            }
        }

        static private void QuickSortInt64(MemoryMappedFile memoryMap, Int64 left, Int64 right)
        {
            Int64 middleElement = 0;
            Int64 indexLeftElement = left;
            Int64 indexRightElement = right;
            Int64 indexMiddle = (left >> 1) + (right >> 1);

            using (var accessor_middle = memoryMap.CreateViewAccessor(indexMiddle << 3, sizeof(Int64)))
            {
                middleElement = accessor_middle.ReadInt64(0);
            }

            while (indexLeftElement <= indexRightElement)
            {

                Int64 leftElement = Int64.MinValue;

                using (var accessor = memoryMap.CreateViewAccessor(indexLeftElement << 3, sizeof(Int64)))
                {
                    leftElement = accessor.ReadInt64(0);
                }
                while (leftElement < middleElement)
                {
                    ++indexLeftElement;
                    using (var accessor = memoryMap.CreateViewAccessor(indexLeftElement << 3, sizeof(Int64)))
                    {
                        leftElement = accessor.ReadInt64(0);
                    }
                }

                Int64 rightElement = Int64.MaxValue;

                using (var accessor = memoryMap.CreateViewAccessor(indexRightElement << 3, sizeof(Int64)))
                {
                    rightElement = accessor.ReadInt64(0);
                }
                while (rightElement > middleElement)
                {
                    --indexRightElement;
                    using (var accessor = memoryMap.CreateViewAccessor(indexRightElement << 3, sizeof(Int64)))
                    {
                        rightElement = accessor.ReadInt64(0);

                    }
                }

                if (indexLeftElement <= indexRightElement)
                {
                    using (var accessor = memoryMap.CreateViewAccessor((indexLeftElement) << 3, sizeof(Int64)))
                    {
                        accessor.Write(0, rightElement);
                    }

                    using (var accessor = memoryMap.CreateViewAccessor(indexRightElement << 3, sizeof(Int64)))
                    {
                        accessor.Write(0, leftElement);
                    }

                    ++indexLeftElement;
                    --indexRightElement;
                }

            }

            if (indexLeftElement < right)
            {
                QuickSortInt64(memoryMap, indexLeftElement, right);
            }

            if (left < indexRightElement)
            {
                QuickSortInt64(memoryMap, left, indexRightElement);
            }

        }

        static public void BubbleSortInt64(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            using (var memoryMap = MemoryMappedFile.CreateFromFile(fileName, FileMode.Open))
            {
                for (Int64 i = 0; i < fileInfo.Length; i += sizeof(Int64))
                {
                    for (Int64 j = 0; j < fileInfo.Length - i - sizeof(Int64); j += sizeof(Int64))
                    {
                        Int64 tempLeftElement = 0, tempRightElement = 0;

                        using (var accessor = memoryMap.CreateViewAccessor(j, sizeof(Int64)))
                        {
                            tempLeftElement = accessor.ReadInt64(0);
                        }


                        using (var accessor = memoryMap.CreateViewAccessor(j + sizeof(Int64), sizeof(Int64)))
                        {
                            tempRightElement = accessor.ReadInt64(0);
                        }

                        if (tempLeftElement > tempRightElement)
                        {
                            using (var accessor = memoryMap.CreateViewAccessor(j, sizeof(Int64)))
                            {

                                accessor.Write(0, tempRightElement);
                            }


                            using (var accessor = memoryMap.CreateViewAccessor(j + sizeof(Int64), sizeof(Int64)))
                            {
                                accessor.Write(0, tempLeftElement);
                            }
                        }
                    }
                }
            }
        }

        static public List<Int64> GetRangeOfFile(string fileName, Int32 size, Int64 leftIndex = 0, Int64 rightIndex = 0)
        {
            List<Int64> rangeOfElements = new List<Int64>();
            FileInfo fileInfo = new FileInfo(fileName);
            rightIndex *= size;
            if (rightIndex == 0 || rightIndex > fileInfo.Length)
            {
                rightIndex = fileInfo.Length / size * size;                
            }
            using (var memoryMap = MemoryMappedFile.CreateFromFile(fileName, FileMode.Open))
            {
                for (leftIndex *= size; leftIndex < rightIndex; leftIndex += size)
                {
                    using (var accessor = memoryMap.CreateViewAccessor(leftIndex, size))
                    {
                        switch (size)
                        {
                            case sizeof(Byte):
                                rangeOfElements.Add(accessor.ReadByte(0));
                                break;
                            case sizeof(Int16):
                                rangeOfElements.Add(accessor.ReadInt16(0));
                                break;
                            case sizeof(Int32):
                                rangeOfElements.Add(accessor.ReadInt32(0));
                                break;
                            case sizeof(Int64):
                                rangeOfElements.Add(accessor.ReadInt64(0));
                                break;
                        }
                    }
                }
            }
            return rangeOfElements;
        }

        static public void WriteBinaryFileToNumericFile(string fileName, string writeFileName, Int32 size)
        {
            FileInfo fileInfo = new FileInfo(fileName);

            using (var sw = new StreamWriter(writeFileName))
            {
                using (var memoryMap = MemoryMappedFile.CreateFromFile(fileName, FileMode.Open))
                {
                    for (Int64 offset = 0; offset < fileInfo.Length; offset += size)
                    {
                        using (var accessor = memoryMap.CreateViewAccessor(offset, size))
                        {
                            switch (size)
                            {
                                case sizeof(Byte):
                                    sw.WriteLine(accessor.ReadByte(0));
                                    break;
                                case sizeof(Int16):
                                    sw.WriteLine(accessor.ReadInt16(0));
                                    break;
                                case sizeof(Int32):
                                    sw.WriteLine(accessor.ReadInt32(0));
                                    break;
                                case sizeof(Int64):
                                    sw.WriteLine(accessor.ReadInt64(0));
                                    break;

                            }
                        }
                    }
                }
            }

        }
    }

}
