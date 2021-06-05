using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixTerrainDrawDistance : MonoBehaviour
{
    [SerializeField] private Terrain _terrain;
    [SerializeField] private int _detailDistance;
    void Start()
    {
        _terrain.detailObjectDistance = _detailDistance;
    }
}
