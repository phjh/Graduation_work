using UnityEngine;

public class ClearPortal : MonoBehaviour
{
	[Header("PortalValue")]
	[SerializeField] private Animator PortalAnim;
	[SerializeField] private Collider PortalCollider;
	[SerializeField] private LayerMask WhatIsPlayer;

	private void Start()
    {
        PortalCollider.enabled = true;

        PortalAnim.SetTrigger("Open");
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.layer == 10)
		{
			Managers.instance.TimeMng.SetTimer(false);
			Managers.instance.FlowMng.isGameClear = true;
			Managers.instance.FlowMng.ChangeSceneInFlow();
            PortalAnim.SetTrigger("Open");
        }
	}
}