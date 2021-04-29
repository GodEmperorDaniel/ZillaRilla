using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CutsceneCamera : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera _currentCamera;
	[SerializeField] private CinemachineVirtualCamera _changeToThisCamera;
	[SerializeField] private float _cutsceneTime;
	private bool _notSwitched = true;
	private Coroutine c_cutscene;
	private bool _triggered;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !_triggered)
		{
			_triggered = true;
			SwitchCamera();
		}
	}
	public void SwitchCamera()
	{
		switch (_notSwitched)
		{
			case true:
				_currentCamera.Priority = 0;
				_changeToThisCamera.Priority = 10;
				c_cutscene = StartCoroutine(Cutscene());
				break;
			case false:
				_currentCamera.Priority = 10;
				_changeToThisCamera.Priority = 0;
				break;
		}
	}
	private IEnumerator Cutscene()
	{
		yield return new WaitForSeconds(_cutsceneTime);
		_notSwitched = false;
		SwitchCamera();
		c_cutscene = null;
	}
}
