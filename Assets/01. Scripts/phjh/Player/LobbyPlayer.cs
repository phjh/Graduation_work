using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
    private PlayerHand _playerHand;

    private void Start()
    {
        PlayerManager.Instance.player = this;
    }

    private void Update()
    {
        PlayerManager.Instance.player = this;
    }

    public void SetSpineIK(Transform left, Transform right)
    {
        _playerHand = GetComponentInChildren<PlayerHand>();

        if (_playerHand == null)
        {
            Logger.LogError("playerHand is null");
            return;
        }

        _playerHand.Init(left, right);
    }

}