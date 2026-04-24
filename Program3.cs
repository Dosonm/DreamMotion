using System;
using System.Collections.Generic;


/* [문제]
 * 테트리스 게임을 구현 중입니다.
 * 
 * TetrisState 클래스는 테트리스 판에 배치된 블록들의 상태를 나타냅니다.
 * Get, Set, CheckCompleteLine 함수를 구현하기 전에
 * TetrisState 클래스의 내부 변수를 1안과 2안 중 어떤 방식으로 할지 먼저 결정해야 합니다.
 * 
 * 코드의 가독성을 우선시 한다면 1안과 2안 중 어떤 방식을 선택하겠습니까? 그리고 그 이유는 무엇입니까?
 */

//여기 아래에 답변을 적어주세요.
/*
	1안 (bool[,] blocks)을 선택할 거 같습니다.                                                                                                                                            
                                                                                                                                                                                
	이유는 다음과 같습니다.                                                                                                                                                       
                  
	직관성: blocks[x, y]라는 표현 자체가 "x, y 좌표에 블록이 있다"는 의미를 그대로 담고 있어서, 코드를 처음 보는 사람도 자료구조의 구조를 바로 이해할 수 있습니다.

	함수 구현의 명확성: Get(int x, int y)와 Set(int x, int y) 함수의 구현이 return blocks[x, y]와 blocks[x, y] = true 처럼 한 줄로 명확하게 표현됩니다. 반면 2안은 lines[y]의 특정
    비트를 읽고 쓰기 위해 비트 연산(>>, &, |)이 필요해 가독성이 떨어집니다.

	2안의 단점: List<ushort>에서 각 ushort가 한 줄의 10칸을 비트로 표현하는 방식인데, 이 의도가 코드만 봐서는 즉시 파악되지 않습니다. CheckCompleteLine에서도 비트가 모두 1인지
	(0x3FF) 확인하는 방식은 가독성을 해칩니다.

	결론: 가독성을 우선시한다면 1안이 적합합니다. 2안은 메모리 절약 측면에서 유리하지만, 테트리스 판(10×20 = 200칸)은 크기가 매우 작아 최적화가 불필요하므로 가독성 이점이 더
	큽니다.
*/

class TetrisState
{
	const int TETRIS_SIZE_X = 10;
	const int TETRIS_SIZE_Y = 20;

	bool Get(int x, int y) { throw new NotImplementedException(); }         // x, y 지점에 블록이 있는지 체크하는 함수
	void Set(int x, int y) { throw new NotImplementedException(); }         // x, y 지점에 블록이 있다고 설정하는 함수
	void CheckCompleteLine() { throw new NotImplementedException(); }       // 완성된 줄을 체크하고 제거하는 함수

	//1안)
	bool[,] blocks = new bool[TETRIS_SIZE_X, TETRIS_SIZE_Y];

	//2안)
	List<ushort> lines = new List<ushort>();
}

