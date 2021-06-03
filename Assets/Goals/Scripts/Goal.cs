using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public class Goal : MonoBehaviour
{
    public enum NewsCategory { HarbourGoal, BossAppearing, BossDefeated }
    
    [SerializeField] protected string goalName = "[Default Name]";
    [SerializeField] protected string goalDescription = "[Default Text]";
    [SerializeField] private NewsCategory newsCategory;
    [SerializeField] private string newsTitle;

    protected bool _completed = false;


    public string GoalDescription
    {
        get => goalDescription;
        private set => goalDescription = value;
    }

    public string GoalName
    {
        get => goalName;
        private set => goalName = value;
    }

    public virtual void GoalInitialization()
    {
        Debug.Log("Goal Initialized");
    }
    
    public virtual void GoalCompleted()
    {
        string category = GetCategoryName(newsCategory);
        
        // Will override current news banner
        if (newsTitle == "")
            UIManager.Instance.ActivateBanner(category, true); 
        else
            UIManager.Instance.ActivateBanner(category, true, newsTitle);
        
        GetComponentInParent<GoalManager>().GoalCompleted();
    }

    public string GetCategoryName(NewsCategory category)
    {
        return category switch
        {
            NewsCategory.HarbourGoal => "Harbour Goal",
            NewsCategory.BossAppearing => "Boss Goal",
            NewsCategory.BossDefeated => "Victory",
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
    }
}