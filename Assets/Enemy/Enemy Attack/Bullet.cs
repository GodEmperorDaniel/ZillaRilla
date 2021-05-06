using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _destoryTime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, _destoryTime);
    }

}
