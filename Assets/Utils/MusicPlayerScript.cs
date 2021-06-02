using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerScript : MonoBehaviour
{
    private void Awake()
    {
        GameManager.victoryOrLoseDelegate += this.OnCalledEvent;
    }

    private void OnCalledEvent()
    {
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        GameManager.victoryOrLoseDelegate -= this.OnCalledEvent;
    }
}
