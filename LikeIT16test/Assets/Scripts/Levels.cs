using UnityEngine;
using System.Collections;

public class Levels : MonoBehaviour 
{
	public Base _base;
	public GameObject levelPrefab;



	void Start () 
	{
		for (int i = 0; i < _base.levels.Length; i++)
		{
			var go = Instantiate(levelPrefab, new Vector3(0, 2 - i * 2f, 0), Quaternion.identity) as GameObject;
			if (i > 0 && !SaveManager.Instance.IsLevelOpen(i + 1))
				go.GetComponent<Button>().Disable();
			go.name = "Level_" + (i + 1);
			go.transform.GetChild(0).GetComponent<TextMesh>().text = "Level " + (i + 1);
		}	
	}
}