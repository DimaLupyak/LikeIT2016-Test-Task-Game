using UnityEngine;
using System.Collections;

public class SmoothFollow2 : MonoBehaviour {
	public Transform target;
	public float distance = 3.0f;
	public float height = 3.0f;
	public float damping = 5.0f;
	public bool smoothRotation = true;
	public float rotationDamping = 10.0f;
	public float deltaX = 0;

	void Update () 
	{
		Vector3 wantedPosition = target.TransformPoint(0, height, -distance) + new Vector3(deltaX, 0, 0);
		transform.position = Vector3.Lerp (transform.position , wantedPosition, Time.deltaTime * damping);
	}
}