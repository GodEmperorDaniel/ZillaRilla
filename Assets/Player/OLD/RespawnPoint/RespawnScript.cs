using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    private Coroutine c_respawn;
    private List<PlayerData> _respawnTargets = new List<PlayerData>();
    private List<PlayerData> _currentlyRespawning;
    private bool _rightLeft;

    public void AddRespawnTarget(Attackable player, int respawnTime = 5)
    {
        if (_respawnTargets.Count < 2)
        {
            player.ResetHealth();
            PlayerData playerData = new PlayerData();
            playerData._playerObject = player;
            playerData._respawnTime = respawnTime;
            _respawnTargets.Add(playerData);
        }
    }
    private void Update()
    {
        if (_respawnTargets.Count > 0)
        {
            if (c_respawn == null)
            {
                c_respawn = StartCoroutine(Respawn(_respawnTargets[0]._playerObject, _respawnTargets[0]._respawnTime));
                _respawnTargets.RemoveAt(0);
            }
            else
            {
                Debug.Log("BOTH PLAYERS DEAD!");
            }
        }
    }
    private IEnumerator Respawn(Attackable player, int respawnTime)
    {
        if (_rightLeft)
        {
            player.transform.position = transform.position + _offset;
            _rightLeft = !_rightLeft;
        }
        else
        {
            player.transform.position = transform.position - _offset;
            _rightLeft = !_rightLeft;
        }
        yield return new WaitForSeconds(respawnTime);
        player.gameObject.SetActive(true);
        yield return null;
        c_respawn = null;
    }

}
public struct PlayerData
{
    public Attackable _playerObject;
    public int _respawnTime;
}