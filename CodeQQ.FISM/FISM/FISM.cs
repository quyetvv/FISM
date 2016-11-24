using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CodeQQ.FISM.Data;
using CodeQQ.FISM.DataServices;

namespace CodeQQ.FISM
{
    public class FISM: IFISM
    {
        // use this variable as temp to save a joint list out
        private HashSet<int> _outJoinedList = null;
        private List<int[]> _subArrayRet = null;
        
        public double TotalCombinations;
        /// <summary>
        /// Algorithm to get a 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="kMin"></param>
        /// <param name="lChosen"></param>
        /// <returns></returns>
        public int[][] Process(int[][] input, int kMin, int lChosen)
        {
            _subArrayRet = new List<int[]>();
            //STEP 1: Build dictionary
            // Build dictionary, we using HashSet to storage list, this will make join two list later become more effective
            LogFactory.GetLogger().ShowLog("Building dictionary...");
            InputValueDictionaryServices.BuildDictionaryCacheFromInput(input);

            //STEP 2: Create combination generator
            // Base on list values from dictionary
            // We build a set of combinations genereate from this value with chose as lelemnt
            // It is same as chose k element in a set of n element
            // To increase speed we alway order elemnts in a combination base on count of list contain this element
            // For example 3 have three list contain: 1 2 5 but 4 have  only two list contain it: 2 4 so we will place 4 before 1 2 5 in a combination
            var listValues = InputValueDictionaryServices.ValuesListCacheDictionary.Keys.OrderBy(x => InputValueDictionaryServices.ValuesListCacheDictionary[x].Count);
            var combs = new Combinations<int>(listValues, new LengthListComparer(), lChosen);

            // STEP 3: Find in all combination satified combination will be added to out put array
            FindInAllCombinations(combs, kMin);
            
            // return output array
            return _subArrayRet.ToArray();
        }

        /// <summary>
        /// Find satified combination by loop for each combination, joining all list referenced by values in combination.
        /// If joined list return has at least KMIN element then it will be added to out put array
        /// </summary>
        /// <param name="combinations"></param>
        /// <param name="kMin"></param>
        private void FindInAllCombinations(Combinations<int> combinations,int kMin)
        {            
            foreach (var combination in combinations)
            {
                LogFactory.GetLogger().ShowLog("Checking combinations:", combination);
                if (JoinListServices.GetJoinListInDictionary(combination, combination.Length, kMin, out _outJoinedList, string.Join("_", combination) + "_"))
                {
                    LogFactory.GetLogger().ShowLog("Found one combination: ", combination);
                    _subArrayRet.Add(combination);
                }
                TotalCombinations += 1;
                JoinListServices.ClearCache();
            }
        }
    }

    /// <summary>
    /// Using this comparer to compare and sort elements in combinations
    /// We get a combination with elements who list contain less element will be precedeed
    /// </summary>
    public class LengthListComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            int diff = InputValueDictionaryServices.ValuesListCacheDictionary[x].Count - InputValueDictionaryServices.ValuesListCacheDictionary[y].Count;
            if (diff == 0)
            {
                diff = x - y;
            }
            return diff;
        }
    }
}
