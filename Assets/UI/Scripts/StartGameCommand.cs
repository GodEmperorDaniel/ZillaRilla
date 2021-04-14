using System;
using UnityEngine;

public class StartGameCommand : Command
{
    public override void Execute()
    {
        Debug.Log("StartGame called");
        GameManager.Instance.LoadLevel("TestScene1");
    }
}