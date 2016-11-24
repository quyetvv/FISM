using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using CodeQQ.FISM.Data;

namespace CodeQQ.FISM.DataServices
{
    public class InputValueDictionaryServices
    {
        private static Dictionary<int, HashSet<int>> _valuesListCacheDictionary;
        public static Dictionary<int, HashSet<int>> ValuesListCacheDictionary
        {
            get
            {
                if (_valuesListCacheDictionary == null)
                    _valuesListCacheDictionary = new Dictionary<int, HashSet<int>>();
                return _valuesListCacheDictionary;
            }
        }


        /// <summary>
        /// Build a dictionary from input array
        /// Each key of dictionary will be a value exist in input and value will be all list contain this value
        /// </summary>
        /// <param name="input"></param>
        public static void BuildDictionaryCacheFromInput(int[][] input)
        {
            var elementDict = new Dictionary<int, HashSet<int>>();
            for (int i = 0; i < input.Count(); i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    int valueElement = input[i][j];
                    // valueElement is element in each sub array. It will be used as key of dictionary
                    if (elementDict.ContainsKey(valueElement))
                    {
                        elementDict[valueElement].Add(i);
                    }
                    else
                    {
                        elementDict.Add(valueElement, new HashSet<int>(){i});
                    }
                }
            }
            _valuesListCacheDictionary = elementDict;
            ShowDictionary();
        }

        private static void ShowDictionary()
        {
            if (LogFactory.GetLogger().IsShowLog())
            {
                LogFactory.GetLogger().ShowLog("Building dictionary completed:");
                var listKeys = _valuesListCacheDictionary.Keys.OrderBy(x => InputValueDictionaryServices.ValuesListCacheDictionary[x].Count);
                LogFactory.GetLogger().ShowLog("Key ordered:",listKeys.ToArray());
                foreach (int value in listKeys)
                {
                    LogFactory.GetLogger().ShowLog( " List contain values " + string.Format("[{0}] includes:", value) + string.Join(",", _valuesListCacheDictionary[value]));
                }
            }
        }
    }
}
