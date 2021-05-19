using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningCubeTest : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        NudgeCube();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.zero;

        //_rotation = (_rotation + rotateSpeed) % 360;
        //transform.Rotate(Vector3.up, rotateSpeed * Time.timeScale);
    }

    private void NudgeCube()
    {
        rb.AddForceAtPosition(new Vector3(3, 0, 0), new Vector3(0, 0, 1));
    }


}