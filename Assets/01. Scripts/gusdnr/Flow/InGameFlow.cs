using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameFlow : SceneFlowBase
{
	[Header("Player Spawn Position")]
	[SerializeField] private Vector3[] SpawnPositions = new Vector3[4];
	public int SelectedSpawnPoint = -1;

	private WeaponInfomation weaponInfo;
	private MiniChuckMapUI minimap;
	private EnemySpawn enemySpawn;

	public override void ActiveFlowBase()
	{
		weaponInfo = FindAnyObjectByType<WeaponInfomation>();
		weaponInfo.SetWeaponData(mngs.PlayerMng.SelectedWeaponData);
		minimap = FindAnyObjectByType<MiniChuckMapUI>();
		minimap.ChunkMiniMapUIInit(mngs.PlayerMng.Player.transform, mngs.PlayerMng.Player.inputReader);
		SelectRandomStartPostion();

		enemySpawn = FindAnyObjectByType<EnemySpawn>();
		enemySpawn.ActiveEnemySpawn();

		TimeManager.Instance.StartTimer();
	}

	private void SelectRandomStartPostion()
	{
		SelectedSpawnPoint = Random.Range(0, SpawnPositions.Length - 1);

		mngs.PlayerMng.Player.transform.position = SpawnPositions[2];
	}
}
