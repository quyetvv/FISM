using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeQQ.FISM.Data;

namespace CodeQQ.FISM.DataServices
{
    public class JoinListServices
    {
        private static Dictionary<string, HashSet<int>> JoinedListCache = new Dictionary<string, HashSet<int>>();

        private static int MaxCache = 100000;
        /// <summary>
        /// To save memory, this method ensure that we only save list in cache if this list have number of item greater than KMin value
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="joinedList"></param>
        /// <param name="KMin"></param>
        private static void AddJoinedListToKey(string cacheKey, HashSet<int> joinedList, int KMin)
        {            
            if (joinedList == null || joinedList.Count < KMin)
            {
                LogFactory.GetLogger().ShowLog("Cache miss. Add null value with key:" + cacheKey);
                JoinedListCache.Add(cacheKey, null);
            }
            else
            {
                LogFactory.GetLogger().ShowLog("Cache miss and add new item to cache with key:" + cacheKey);
                JoinedListCache.Add(cacheKey, joinedList);
            }
        }

        /// <summary>
        /// Join all list base on a key of list
        /// We use a recursive method to join all list, start from last list ...
        /// A combination is set of l element chosen. We now join all list contain these element
        /// For example a combination includes:2 3 4      
        /// We will join three list contain 2 3 4, if list return from joining these list have more than K min. It mean that there are more than K list contain a combination L element
        /// We will try to break calculation (joining list) so much base on caching result from previous join
        /// Also base on feature of two combination adjacent we maybe only using previous result to calculate
        /// For example when we calculate join list for a combination of 2 3 4 6 we save result of joining list 2 3 4
        /// Then we need get joining list of (2 3 4 7) we just need get joining list (2 3 4) from cache and join with list contain 7
        /// With join list from lists contain (2 3 4) has number of element less than K we will do not join with list contain 7 but return immediately.  Base on this way,we save so much of time for process join list        
        /// </summary>
        /// <param name="combination"></param>
        /// <param name="maxJoinIndex"></param>
        /// <param name="KMin"></param>
        /// <param name="joinedList"></param>
        /// <returns></returns>
        public static bool GetJoinListInDictionary(int[] combination, int maxJoinIndex, int KMin, out HashSet<int> joinedList, string cachedKey)
        {
            LogFactory.GetLogger().ShowLog("Check join list result in cache with key:" + cachedKey);
            if (!JoinedListCache.ContainsKey(cachedKey))
            {
                // If we have more than 2 list need join then we need recursive
                // Only calculate join if we just list join for two list
                if (maxJoinIndex > 2)
                {                    
                    // get new cache key by removed last element out of key
                    // For example, first time call this function, a key will be 2_3_4_6_
                    // Then we call recursive here, it will be 2_3_4_ or 2_3_
                    int indexKeyRemoved = cachedKey.Length - (combination[maxJoinIndex - 1].ToString().Length + 1);
                    string newCachedKey = cachedKey.Remove(indexKeyRemoved);

                    // We get recursive here. If it return false mean that list join is null or not satify condition KMin
                    bool resultJoinToBeforeList = GetJoinListInDictionary(combination, maxJoinIndex - 1, KMin,out joinedList, newCachedKey);

                    // we not save this result to cache if it is a full join list\
                    // Example a combination of (2 3 4 6) joined, then just need cache result of (2 3 4) because (2 3 4 6) is not used by any more
                    if (combination.Count() - maxJoinIndex >= 1)
                    {
                        // save join list to cache                        
                        AddJoinedListToKey(cachedKey, joinedList, KMin);
                    }
                    if (resultJoinToBeforeList)
                    {                        
                        joinedList = JoinTwoListNoCache(joinedList, InputValueDictionaryServices.ValuesListCacheDictionary[combination[maxJoinIndex - 1]]);
                    }
                }
                else
                {
                    // here only two list joined, we call to join it and add list to cache
                    joinedList = JoinTwoListNoCache(InputValueDictionaryServices.ValuesListCacheDictionary[combination[0]], InputValueDictionaryServices.ValuesListCacheDictionary[combination[1]]);
                    AddJoinedListToKey(cachedKey, joinedList, KMin);
                }
            }
            else
            {
                LogFactory.GetLogger().ShowLog("Cache hit with key:" + cachedKey);
                joinedList = JoinedListCache[cachedKey];
            }
            return joinedList != null && joinedList.Count >= KMin;
        }

        /// <summary>
        /// We clear cache to ensure that programm not comsume so much of memory on a big combinations
        /// </summary>
        public static void ClearCache()
        {
            if (JoinedListCache.Count > MaxCache)
            {
                JoinedListCache.Clear();
                JoinedListCache = new Dictionary<string, HashSet<int>>();
            }
        }

        /// <summary>
        /// Join two list together
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static HashSet<int> JoinTwoListNoCache(HashSet<int> list1, HashSet<int> list2)
        {
            HashSet<int> joinList = new HashSet<int>();            
            foreach (int value in list1)
            {
                if (list2.Contains(value))
                {
                    joinList.Add(value);
                }
            }
            return joinList;
        }
    }
}
