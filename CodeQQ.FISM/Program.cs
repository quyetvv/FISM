using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CodeQQ.FISM;
using CodeQQ.FISM.Data;

namespace CodeQQ.FISM
{
    class Program
    {
        static void Main(string[] args)
        {
            int KMin, LChosen, sizeArray;            

            do
            {
                Console.Clear();
                Console.WriteLine("------------------------- ************-------------------------");
                Console.WriteLine("------------------------- FISM PROGRAM-------------------------");
                Console.WriteLine("------------------------- ************-------------------------");
                Console.WriteLine();
                Console.WriteLine("Enter size of array init (Press enter to use default value as 5):");

                var fismInstance = new FISM();

                if (!int.TryParse(Console.ReadLine(), out sizeArray) && sizeArray < 5)
                {
                    sizeArray = 5;
                }
                var inputArrayN = InitArrayInput(sizeArray);

                Console.WriteLine("Input arrays:");
                ShowList(inputArrayN, "input.txt");

                Console.WriteLine("Enter K-min value (Press enter to use default value as 2):");
                if (!int.TryParse(Console.ReadLine(), out KMin) || KMin < 2 || KMin > sizeArray)
                {                
                    KMin = 2;
                }
                
                Console.WriteLine("Enter number of elements for a combination (Press enter to use default value as 2):");
                if (!int.TryParse(Console.ReadLine(), out LChosen) || LChosen < 2 || LChosen > sizeArray)
                {
                    LChosen = 2;
                }

                // Only option for print log to screen if Size Array < 20 (default of LogServices) and user want to show it with confirmation Yes
                if (sizeArray < LogFactory.GetLogger().GetKMinMaxPrint())
                {
                    Console.WriteLine("Do you want to print log to screen (Press Y to chose Yes, other for No):");
                    if (Console.ReadKey().Key == ConsoleKey.Y)
                    {
                        LogFactory.GetLogger().ActivaShowLog(true);
                        LogFactory.GetLogger().SetCurrentKMin(KMin);
                        Console.WriteLine();
                    }
                }

                DateTime startTime = DateTime.Now;                
                int[][] result = fismInstance.Process(inputArrayN, KMin, LChosen);
                DateTime endTime = DateTime.Now;

                Console.WriteLine("Output arrays for (N_SiZE ARRAY,K_MIN,L_ELEMENT) = ({0},{1},{2})",sizeArray,KMin,LChosen);
                ShowList(result, "output.txt");
                Console.WriteLine(string.Format("Found {0} combinations in {1} s",result.Count(),(endTime - startTime).TotalSeconds));
                Console.WriteLine("Total combinations checked: {0}", fismInstance.TotalCombinations);
                Console.WriteLine("Production: {0} milions of combinations /s", fismInstance.TotalCombinations / ((endTime - startTime).TotalSeconds * 1000000) );

                Console.WriteLine("Press Escape(Esc)  to escape!");                
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }

        /// <summary>
        /// Initialize input array. With 5 we select default input as specified.
        /// Other options, we random create a new input array with random values but size as specified from user
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private static int[][] InitArrayInput(int size = 5)
        {
            List<int[]> inputArrayN;
            Random randomizer = new Random(DateTime.Now.Millisecond);
            if (size > 5)
            {
                inputArrayN = new List<int[]>(size);
                for (int i = 0; i < size; i++)
                {
                    int subArraySize = randomizer.Next(3, 30);
                    List<int> subArray = new List<int>(subArraySize);
                    for (int j = 0; j < subArraySize; j++)
                    {
                        int ranValue = randomizer.Next(0, size);           
                        if (!subArray.Contains(ranValue))
                        {
                            subArray.Add(ranValue);
                        }
                    }
                    inputArrayN.Add(subArray.ToArray());
                }
            }
            else
            {
                inputArrayN = new List<int[]>() { new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 3, 2 }, new int[] { 1, 4, 3, 2, 9 }, new int[] { 2, 3, 4, 7, 9 }, new int[] { 3, 4, 5, 9, 10 } };
            }
            return inputArrayN.ToArray();
        }

        private static void ShowList(int[][] result,string fileName = "")
        {
            string textContent = "";
            for (int i = 0; i < result.Count();i++)
            {
                string line = string.Format("L[{0}]:",i) + string.Join(",", result[i]);
                Console.WriteLine(line);
                textContent += line + "\r\n";
            }
            if (!string.IsNullOrEmpty(fileName))
            {
                File.WriteAllText(fileName, textContent);
            }
        }
    }
}
