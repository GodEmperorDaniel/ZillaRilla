using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCamera : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera _currentCamera;
	[SerializeField] private CinemachineVirtualCamera _changeToThisCamera;
	private bool _notSwitched = true;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player") && _notSwitched)
		{
			SwitchCamera();
		}
	}
	public void SwitchCamera()
	{
		_currentCamera.Priority = 0;
		_changeToThisCamera.Priority = 10;
		_notSwitched = !_notSwitched;
	}
}
