using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ResultFlow : SceneFlowBase
{
	#region Structs

	[System.Serializable]
	struct GameResultPair
	{
		public string ResultText;
		public Sprite ResultImage;
	}

	[System.Serializable]
	struct OreResultPair
	{
		public StatType ThisOreType;
		public TextMeshProUGUI Text;
	}

	#endregion

	[Header("Game Result UI Elements")]
	[SerializeField] private GameResultPair[] ResultPairs;
	[SerializeField] private Image ResultImg;
	[SerializeField] private TextMeshProUGUI ResultText;
	[Range(0f, 3f)][SerializeField] private float ResultDuration = 1.0f;

	[Header("Ore Result UI Elements")]
	[SerializeField] private OreResultPair[] OrePairs;
	[SerializeField] private TextMeshProUGUI OreSum;
	[Range(0f, 3f)][SerializeField] private float OreDuration = 0.3f;

	[Header("Time Result UI Elements")]
	[SerializeField] private TextMeshProUGUI FloorText;
	[SerializeField] private TextMeshProUGUI TimeText;

	[Header("Buttons")]
	[SerializeField] private Button ReStartBtn;
	[SerializeField] private Button QuitBtn;

	private int OreCount = 0;
	private int OreCountSum = 0;

	public override void ActiveFlowBase()
	{
		ResultText.text = ResultPairs[mngs.FlowMng.isGameClear ? 1 : 0].ResultText;
		ResultImg.sprite = ResultPairs[mngs.FlowMng.isGameClear ? 1 : 0].ResultImage;
		TMPDOText(ResultText, ResultDuration);

		OreCount = OreCountSum = 0;

		foreach (OreResultPair pair in OrePairs)
		{
			OreCount = mngs.PlayerMng.RetrunOreCount(pair.ThisOreType);
			pair.Text.text = IntToString(OreCount);
			TMPDOText(pair.Text, OreDuration);
			OreCountSum = OreCountSum + OreCount;
		}

		OreSum.text = IntToString(OreCountSum);
		TMPDOText(OreSum, OreDuration);

		TimeText.text = SetRemainTime();

		ReStartBtn?.onClick.RemoveAllListeners();
		ReStartBtn?.onClick.AddListener(() => mngs.UIMng.SetSceneName(NextSceneName));

		QuitBtn?.onClick.RemoveAllListeners();
		QuitBtn?.onClick.AddListener(() => mngs.UIMng.QuitGame());
	}

	private string IntToString(int value)
	{
		return string.Format("{00}", value);
	}

	private void TMPDOText(TextMeshProUGUI tmp, float duration)
	{
		tmp.maxVisibleCharacters = 0;
		DOTween.To(x => tmp.maxVisibleCharacters = (int)x, 0f, tmp.text.Length, duration);
	}

	private string SetRemainTime()
	{
		int t0 = (int)mngs.TimeMng.timeLimit;
		int m = t0 / 60;
		int s = (t0 - m * 60);
		return $"{m:00}:{s:00}";
	}

}
