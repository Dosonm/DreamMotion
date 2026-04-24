using System;

/* [문제]
 * 비트 다루기
 *    - MakeBitRange 함수를 구현하시면 됩니다.
 *    - 출력값은 from 번째 비트부터 to 번째 비트까지 1로 세팅된 값이어야 합니다.
 *    - 예) MakeBitRange(1,4) = 00011110 (0x0000001E)
 *    - from, to는 0~31까지 값이며, to >= from 입니다.
 */

class Program
{
	//"<<(비트 시프트 연산자)"를 활용하여 함수를 작성해봤습니다.
	//이진수에서 상위 비트의 값 1을 감산하면 하위 비트들이 모두 1로 변하는 특성을 활용하였습니다.
	static string MakeBitRange(uint from, uint to)
	{
		uint mask = (to - from == 31) ? uint.MaxValue : (1u << (int)(to - from + 1)) - 1; // to가 31, from이 0일 때 발생하는 오버플로우를 방지
		uint value = mask << (int)from;

		//출력
		string raw = Convert.ToString(value, 2);	//이진수로 변환                                                                                                             
        int padLen = (raw.Length + 3) / 4 * 4; 		//길이는 4의 배수                                                                                                                           
        string binary = raw.PadLeft(padLen, '0'); 
		return $"{binary} (0x{value:X8})"
		;
	}
}

