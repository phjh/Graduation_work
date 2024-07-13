using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DropedItem : PoolableMono
{
    [Header("Item UI")]
    [SerializeField] private Transform ItemInfoCanvas;
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private TextMeshProUGUI ItemDescription;

    [Header("UI Variables")]
    [SerializeField][Range(0.0f, 1.0f)] private float infoDuration = 0.3f;
    
    private Managers mngs;

    public ItemDataSO ItemData
    {
        get
        {
            if(ItemData == null)
            {   
                Logger.LogError($"{this.name}'s Data is Null. Can't Get Data");
                return null;
            }
            return ItemData;
        }
        set
        {
            ItemData = value;
        }
    }

    private SpriteRenderer ItemRenderer;

	private void Start()
	{
		ResetPoolableMono();
		EnablePoolableMono();
	}

	public void InitializeItemData(ItemDataSO InitData)
    {
        if (InitData.CheckingInitData() == false)
        {
            Logger.LogError($"{InitData.name}'s Data is null! Please checking Inspector");
            return;
        }

        ItemData = InitData;
        ItemData.SetRandomAddingValue();
	}

	private void SetItemObject()
    {
        ItemRenderer.sprite = ItemData.Image;

		ItemName.text = string.IsNullOrEmpty(ItemData.Name) ? "No Name" : ItemData.Name;
		ItemDescription.text = string.IsNullOrEmpty(ItemData.Description) ? "No Desc" : ItemData.Description;
	}

	private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ItemInfoCanvas.DOScale(Vector3.one, infoDuration).SetEase(Ease.OutBack);
        }
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            mngs.PlayerMng.AddOreInDictionary(ItemData.AddingStatType, ItemData.ItemAddingValue);
            mngs.PoolMng.Push(this, PoolName);
        }
    }

    private void OnTriggerExit(Collider other)
	{
        if (other.gameObject.CompareTag("Player"))
        {
			ItemInfoCanvas.DOScale(Vector3.zero, infoDuration).SetEase(Ease.OutExpo);
		}
	}

	public override void ResetPoolableMono()
	{
		if (TryGetComponent(out ItemRenderer) == false)
		{
			ItemRenderer = this.gameObject.AddComponent<SpriteRenderer>();
		}

		if (mngs == null) mngs = Managers.GetInstance();
		ItemInfoCanvas.localScale = Vector3.zero;
	}

	public override void EnablePoolableMono()
	{
		if (ItemInfoCanvas.TryGetComponent(out Canvas itemCanvas)) itemCanvas.worldCamera = Camera.main;
		SetItemObject();
	}
}
