using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveDecal : MonoBehaviour
{
	[SerializeField] private float _despawnTime;

	private void Start()
	{
		StartCoroutine((ChangeColor()));
	}
	private IEnumerator ChangeColor()
	{
		Material mat = GetComponent<MeshRenderer>().material;
		float t = 0;
		Color matDefCol = mat.color;
		Debug.Log("Doing something");
		while (t < 1)
		{ 
			mat.color = Color.Lerp(matDefCol, new Color(matDefCol.r, matDefCol.g, matDefCol.b, 0), t);
			t += Time.deltaTime / _despawnTime;
			yield return null;
		}
		Destroy(gameObject);
	}
}
