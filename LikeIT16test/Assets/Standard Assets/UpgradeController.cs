using UnityEngine;
using System.Collections;

public class UpgradeController : MonoBehaviour {


	// Use this for initialization
	void Start (){
		
		if (PlayerPrefs.HasKey ("hummerLVL")) {
			
		} else {
			PlayerPrefs.SetInt ("hummerLVL", 0);
		}

		if (PlayerPrefs.HasKey ("guitarLVL")) {
			
		} else {
			PlayerPrefs.SetInt ("guitarLVL", 0);
		}

		if (PlayerPrefs.HasKey ("showerLVL")) {
			
		} else {
			PlayerPrefs.SetInt ("showerLVL", 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
