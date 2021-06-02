using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : Manager<CutsceneManager>
{
    private VideoPlayer _videoPlayer;

    private void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.targetCamera = UIManager.Instance.DummyCamera;
        _videoPlayer.loopPointReached += ClipDonePlaying;

        GetComponentInChildren<Canvas>().worldCamera = UIManager.Instance.DummyCamera;
    }
    
    

    public void ClipDonePlaying(VideoPlayer videoPlayer)
    {
        GameManager.Instance.DisableAllControls();
        GameManager.Instance.LoadMainMenu();
    }
}
