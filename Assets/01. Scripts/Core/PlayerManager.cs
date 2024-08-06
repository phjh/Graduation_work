using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : ManagerBase<PlayerManager>
{
	[Header("Player Data")]
	public WeaponDataSO SelectedWeaponData;
	public bool ActioveXRay = false;
	public Player Player;

	public Dictionary<StatType, int> OreDictionary = new Dictionary<StatType, int>();

	private GameObject nowWeapon;

	//only at lobby
	public LobbyPlayer LobbyPlayer;

	public override void InitManager()
	{
		base.InitManager();
		//SelectedWeaponData = Resources.Load("Resources/WeaponDatas/PickaxeData") as WeaponDataSO;
		SetUpOreDictionary();

        Logger.Log("Complete Active Player Manager");
	}

	public void SetUpOreDictionary()
	{
		OreDictionary = new Dictionary<StatType, int>();

		for (int count = 0; count < (int)StatType.End; count++)
		{
			if ((StatType)count == StatType.None || (StatType)count == StatType.End) continue;
			OreDictionary.Add((StatType)count, 0);
		}
	}

	public void SetPlayerWeapon(WeaponDataSO SettedData)
	{
		if(SettedData.weapon == WeaponEnum.None || SettedData.weapon == WeaponEnum.End) return;
		SelectedWeaponData = SettedData;
        SetPlayerWeaponVisible();
	}

	private void SetPlayerWeaponVisible()
	{
		if (nowWeapon != null)
			Destroy(nowWeapon);
		nowWeapon = Instantiate(SelectedWeaponData.playerWeapon.gameObject, new Vector2(0.1f,-0.62f), Quaternion.identity);
		if(SelectedWeaponData.weapon == WeaponEnum.Pickaxe)
		{
			nowWeapon.transform.rotation = Quaternion.Euler(0, 180, 45);
            Vector3 v = nowWeapon.transform.lossyScale;
            nowWeapon.transform.localScale = new Vector3(v.x * -1, v.y, v.z);
        }
		if(SelectedWeaponData.weapon == WeaponEnum.Drill)
		{
			Vector3 v = nowWeapon.transform.lossyScale;
			nowWeapon.transform.localScale = new Vector3(v.x * -1, v.y, v.z);
		}
        nowWeapon.SetActive(true);
		PlayerWeapon weapon = nowWeapon.GetComponent<PlayerWeapon>();
		LobbyPlayer.SetSpineIK(weapon.leftHandIK, weapon.rightHandIK);
	}

	public void SetActiveXRay(bool active)
	{
		ActioveXRay = active;

		if(ActioveXRay)
		{

		}
	}

	public WeaponDataSO SentWeaponData()
	{
		if (SelectedWeaponData.weapon == WeaponEnum.None || SelectedWeaponData.weapon == WeaponEnum.End) return null;
		return SelectedWeaponData;
	}

	public void AddOreInDictionary(StatType type, float addValue)
	{
		if (!OreDictionary.ContainsKey(type)) return; //If Non value in Dictionary to same type, return

		OreDictionary[type] = OreDictionary[type]++;
		Player?.playerStat?.AddModifierStat(type, addValue, false);
	}

	public int RetrunOreCount(StatType type)
	{
		return OreDictionary[type];
	}
}
