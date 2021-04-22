using System.Collections;
using System.Collections.Generic;
using GameStates;
using UnityEngine;

public class Unit : MonoBehaviour
{
    StateMachine stateMachine = new StateMachine();

    void Start()
    {
        stateMachine.ChangeState(new BootState());
    }

    void Update()
    {
        stateMachine.Update();
    }
}