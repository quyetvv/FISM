using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeQQ.FISM
{
    public interface IFISM
    {        
        /// Processes the specified input to identify the frequent individual elements.
        /// The input array. 
        /// The frequent number which result must be appeared in the input array's sub-arrays. 
        /// The number elements of result. 
        /// The array of all possible results. 
        int[][] Process(int[][] input, int kMin, int lChosen);
    }
}
