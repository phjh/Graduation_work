using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawn : MonoBehaviour
{
    [Header("Spawn Values")]
    [Range(0.1f, 10f)]public float MinSpawnTick = 3f;
    [Range(0.1f, 10f)]public float MaxSpawnTick = 3f;
	[Range(1, 10)]public int MinSpawnOnce = 3;
    [Range(1, 10)]public int MaxSpawnOnce = 5;
	[Range(1f, 25f)] public float SpawnDistance = 10f;
	public LayerMask WhatIsGround;
	public LayerMask WhatIsWall;

    [Header("Restricted Zone Range")]
	[SerializeField] private Vector2 MapOutLine;
	[SerializeField] private Vector2 BossArea;


    [Header("Enemy Setting")]
    public List<string> SpawnableMonsters;
	
	public bool IsSpanwing = false;
	public bool OnRaid = false;

    private Managers mngs;
    private Transform PlayerTrm;
    private Vector3 LastEnemySpawnPosition = Vector3.zero;

	private Coroutine EnemySpawnCoroutine = null;

    public void ActiveEnemySpawn()
    {
		if (mngs == null) mngs = Managers.GetInstance();
		if (PlayerTrm == null) PlayerTrm = mngs?.PlayerMng?.Player?.transform;

		IsSpanwing = true;
		OnRaid = false;

		EnemySpawnCoroutine = StartCoroutine(EnemySpawning());
	}

	public void InActiveEnemySpawn()
	{
		if(EnemySpawnCoroutine != null) StopCoroutine(EnemySpawnCoroutine);

		IsSpanwing = false ;
	}

    private Vector3 CalculateSpawnPos()
    {
		Vector3 RandomDirection = Random.insideUnitSphere * SpawnDistance;
		RandomDirection.y = 0; // Set Y value to 0

		Vector3 SpawnPosition = PlayerTrm.position + RandomDirection; // Create Random Position by PlayerPosition

		bool isInBossAreaX = SpawnPosition.x >= BossArea.x && SpawnPosition.x <= BossArea.y && SpawnPosition.x <= MapOutLine.x && SpawnPosition.x >= MapOutLine.y;
		bool isInBossAreaZ = SpawnPosition.z >= BossArea.x && SpawnPosition.z <= BossArea.y && SpawnPosition.z <= MapOutLine.x && SpawnPosition.z >= MapOutLine.y;

		if (isInBossAreaX || isInBossAreaZ) // If meet the Condition one of both, Explore again
		{
			return CalculateSpawnPos();
		}


		NavMeshHit SampleSpawnPosition;
		// Find Right Position in NavMesh
		if (NavMesh.SamplePosition(SpawnPosition, out SampleSpawnPosition, 1.0f, NavMesh.AllAreas))
		{
			// Checking Ground
			if (Physics.CheckSphere(SampleSpawnPosition.position, 0.5f, WhatIsGround))
			{
				// Check Wall is not this Position (using Layer)
				if (!Physics.CheckSphere(SampleSpawnPosition.position, 0.5f, WhatIsWall))
				{
					LastEnemySpawnPosition.x = SampleSpawnPosition.position.x;
					LastEnemySpawnPosition.z = SampleSpawnPosition.position.z;
					return LastEnemySpawnPosition; // If meet the conditions, return Vector
				}
			}
		}

		// Explore again if don't meet the conditions
		return CalculateSpawnPos();
	}

	private IEnumerator EnemySpawning()
	{
		while (IsSpanwing)
		{
			if(OnRaid == true) SpawnEnemy(CalculateSpawnPos(), Random.Range(MinSpawnOnce, MaxSpawnOnce));
			if(OnRaid == false) SpawnEnemy(CalculateSpawnPos(), Random.Range(MaxSpawnOnce, MaxSpawnOnce * 3));

			yield return new WaitForSeconds(Random.Range(MinSpawnTick, MaxSpawnTick));
		}

		InActiveEnemySpawn();
	}

    public void SpawnEnemy(Vector3 SpawnPos, int SpawnCount)
    {
        int SpawnMonster = Random.Range(0, SpawnableMonsters.Count - 1);

		for (int count = 0; count < SpawnCount; count++)
        {
            mngs.PoolMng.Pop(SpawnableMonsters[SpawnMonster], SpawnPos);
        }
    }


}
