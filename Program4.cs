using System;
using System.Collections.Generic;
using System.Linq;

/* [문제]
 * 다음은 2D 슈팅 게임의 스테이지를 관리하는 코드입니다.
 * 적들과 플레이어는 서로에게 총알을 쏴서 공격할 수 있습니다.
 * 총알은 한 스테이지에 수십만 개가 나올 정도로 많을 수 있습니다.
 *
 * 아래 코드는 pseudo code로 작성된 것으로, 실제로 컴파일이 되지는 않습니다.
 * 일부 코드는 생략되어 있으며, 일부 코드는 의도적으로 비효율적으로 작성되어 있습니다.
 *
 * 아래 코드를 보고 고쳐야 할 부분을 찾아 그 이유와 어떻게 고쳐야 할지 설명해 주세요.
 * 우선순위에 따라 세가지만 나열해 주시고 코드를 작성할 필요는 없습니다.
 */


//여기 아래에 답변을 적어주세요.
/*
	1번: 충돌 후 return 누락 (PlayerBullet.Update)
	RemoveStageObject(this)로 자신을 제거한 뒤에도 foreach가 계속 돌아서 이미 제거된 총알이 다른 Enemy에 중복 데미지를 줄 수 있습니다. 또한 순회 중 컬렉션을 수정하므로 예외가 발생할 수 있습니다.
	enemy.AddDamage() 호출 후 return을 추가해야 합니다.

	2번: 매 프레임 이중 foreach로 인한 성능 문제
	GameStage.Update()에서 모든 오브젝트를 순회하며 Update()를 호출하고, 그 안에서 PlayerBullet.Update()가 또 stageObjects 전체를 순회합니다. 총알이 K개, 오브젝트가 N개면 매 프레임 O(N×K)가 됩니다.
	stageObjects를 타입별로 분리해(List<Enemy> enemies 등) 총알이 전체 리스트가 아닌 Enemy 리스트만 순회하도록 해야 합니다.

	3번: player 프로퍼티의 매 호출마다 선형 탐색
	player => stageObjects.FirstOrDefault(x => x is Player)는 호출될 때마다 리스트 전체를 순회합니다. player 프로퍼티는 EnemyBullet.Update()에서 매 프레임 호출되므로, 
	Player 참조를 별도 필드로 캐싱하면 좋습니다.

	+추가로 bullet을 생성하고 파괴하는 대신 오브젝트 풀링을 사용하면 GC를 줄여 메모리 성능에 도움을 줄 수 있습니다.
*/

class GameStage
{
	public static GameStage currentStage;

	List<StageObject> stageObjects = new List<StageObject>();

	public Player player => stageObjects.FirstOrDefault(x=>x is Player) as Player;

	public void Update(float deltaTime)
	{
		foreach( var stageObject in stageObjects )
		{
			stageObject.Update(deltaTime);
		}
	}

	public void AddStageObject(StageObject stageObject)
	{
		stageObjects.Add(stageObject);
	}

	public void RemoveStageObject(StageObject stageObject)
	{
		stageObjects.Remove(stageObject);
	}
}

class StageObject
{
	public virtual void Update(float deltaTime)
	{
		// 스테이지 오브젝트의 상태를 업데이트하는 함수
	}
}

class Enemy : StageObject
{
	override public void Update(float deltaTime)
	{
		if( ShouldFireBullet() )
		{
			FireBullet();
		}
	}

	void FireBullet()
	{
		Bullet bullet = new EnemyBullet(this.pos, GameStage.currentStage.player.pos);
		GameStage.currentStage.AddStageObject(bullet);
	}
}

class Player : StageObject
{
	//...
}

class Bullet : StageObject
{
	protected Vector3 pos;
	protected Vector3 dir;
	protected float speed;
	protected float radius;
	protected float power;

	public override void Update(float deltaTime)
	{
		pos += dir * speed * deltaTime;
	}
}

class EnemyBullet : Bullet
{
	public EnemyBullet(Vector3 startPos, Vector3 targetPos)
	{
		//...
	}

	public override void Update(float deltaTime)
	{
		float distanceToPlayer = Vector3.length(pos - GameStage.currentStage.player.pos);

		if( distanceToPlayer < radius )
		{
			// 플레이어가 총알에 맞았을 때의 처리
			GameStage.currentStage.RemoveStageObject(this);

			GameStage.currentStage.player.AddDamage(this.power);
		}
	}
}

class PlayerBullet : Bullet
{
	public PlayerBullet(Vector3 startPos, Vector3 targetPos)
	{
		//...
	}

	public override void Update(float deltaTime)
	{
		foreach(var stageObject in GameStage.currentStage.stageObjects )
		{
			if( stageObject is Enemy enemy)
			{
				float distanceToEnemy = Vector3.length(pos - enemy.pos);
				if( distanceToEnemy < radius )
				{
					// 적이 총알에 맞았을 때의 처리
					GameStage.currentStage.RemoveStageObject(this);

					enemy.AddDamage(this.power);
				}
			}
		}
	}
}
