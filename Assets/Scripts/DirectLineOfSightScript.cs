using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectLineOfSightScript : MonoBehaviour
{
    public GameObject Player;
    public bool InSight;

    // Update is called once per frame
    void Update()
    {

        var layerMask = ~((1 << gameObject.layer) | (1 << Player.layer));
        InSight = !Physics.Linecast(transform.position, Player.transform.position, layerMask);
    }
}
