using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* [문제]
 * 정렬된 두 배열을 합치기
 *    - MergeSortedArray 함수를 구현하시면 됩니다.
 *    - array1과 array2는 이미 오름차순으로 정렬되어 있습니다.
 *    - 합쳐진 배열도 오름차순으로 정렬되어야 합니다.
 */

class Program
{
	static void MergeSortedArray(int[] array1, int[] array2, int[] destArray)
	{
		int i = 0, j = 0, k = 0;	//i는 array1의 인덱스, j는 array2,의 인덱스, k는 destArray의 인덱스

		while (i < array1.Length && j < array2.Length)
		{
			//더 작은 값을 비교
			if (array1[i] <= array2[j])
				destArray[k++] = array1[i++];	//대입과 인덱스 증가를 같이 처리했습니다.
			else
				destArray[k++] = array2[j++];
		}

		//array1의 배열만 남았을 경우 (array2의 배열이 먼저 끝났을 경우)
		while (i < array1.Length)
			destArray[k++] = array1[i++];

		//array2의 배열만 남았을 경우 (array1의 배열이 먼저 끝났을 경우)
		while (j < array2.Length)
			destArray[k++] = array2[j++];
	}
}
