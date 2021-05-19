using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Player.Scrips;
public class SwichToOtherPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    private PlayerInputManager _playerManager;
    private void Awake()
    {
        _playerManager = GetComponent<PlayerInputManager>();
    }

    private void OnEnable()
    {
        _playerManager.onPlayerJoined += SwichToOtherPrefab;
        
    }
    public void SwichToOtherPrefab(PlayerInput input)
    {
        if (_playerPrefab != null && _playerManager.playerCount <= 1)
        { 
            _playerManager.playerPrefab = _playerPrefab;
            //transform.position += new Vector3(4,0,0);
        }
    }
}
