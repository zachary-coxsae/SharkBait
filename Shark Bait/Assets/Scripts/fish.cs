using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.OnFishHit += onFishHit;
    }
    private void OnDisable()
    {

        GameEvents.OnFishHit -= onFishHit;
    }

    public void onFishHit(RaycastHit hitinfo)
    {
        GameObject.Destroy(this.gameObject);
    }
}
