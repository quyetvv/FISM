Please write program with input an array which contains N sub-array(s). Each sub-array is an array of integers which has M element(s). N and M can be very large (e.g. 1.000.000). But we assume that the arrays can be stored in physical memory. The program would output: All sets of integers which appear in at least K or more than K sub-array(s). Each set must have exactly L elements.

//Cho một mảng có N phần tử, mỗi phần tử là một mảng các số nguyên có số lượng khác nhau. Số lượng các phần tử trong từng mảng con và mảng ban đầu (N) có thể rất lớn. Giả sử toàn bộ các mảng đang được lưu trong bộ nhớ vật lý, hãy tìm toàn bộ tập hợp các số nguyên xuất hiện ít nhất trong K mảng con. Tập hợp số nguyên tìm được phải có chính xác L phần tử.

Input:
		N = 5
		{1, 2, 3, 4, 5}
		{1, 3, 2}
		{1, 4, 3, 2, 9}
		{2, 3, 4, 7, 9}
		{3, 4, 5, 9, 10}
    
Output:
		1. In case K = 3, L = 2
		{1, 2}
		{1, 3}
		{2, 3}
		{2, 4}
		{3, 4}
		{3, 9}
		{4, 9}

		2. In case K = 3, L = 3
		{1, 2, 3}
		{2, 3, 4}
		{3, 4, 9}
    
	public interface IFISM
	{
		 /// <summary>
		 /// Processes the specified input to identify the frequency appearing 
		 /// of individual elements.
		 /// </summary>
		 /// <param name="input">The input array.</param>
		 /// <param name="K">The frequency number which result must be appeared
		 /// in the input sub-arrays.
		 /// </param>
		 /// <param name="L">The number of elements of result set.</param>
		 /// <returns>The array of all possible result sets.</returns>
		 int[][] Process(int[][] input, int K, int L);
	}
