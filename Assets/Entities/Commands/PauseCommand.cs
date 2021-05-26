using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCommand : Command
{
    public override void Execute()
    {
        GameManager.Instance.TogglePause();
    }
}
