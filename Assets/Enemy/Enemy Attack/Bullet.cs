    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _destoryTime;
    public EnemyAttacks _attacks;
    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player" || LayerMask.LayerToName(other.gameObject.layer) == "House")
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, _destoryTime);
        }
    }

}
