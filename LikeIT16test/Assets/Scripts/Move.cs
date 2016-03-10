using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	public float speed;
	Vector3 move;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		transform.position += move * speed * Time.deltaTime;
	}
}
