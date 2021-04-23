using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    //[SerializeField] private string _objectiveName;
    [SerializeField] private string _objectiveDescription;


    /*
     public string ObjectiveName
    {
        get => _objectiveName;
        private set => _objectiveName = value;
    }
    */
    
    public string ObjectiveDescription
    {
        get => _objectiveDescription;
        private set => _objectiveDescription = value;
    }

    
}
