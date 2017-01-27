using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour {

	public GameObject buildMenuPanel;

	// Use this for initialization
	void Start () {
		buildMenuPanel.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnBuildButtonClick() {
		buildMenuPanel.SetActive (!buildMenuPanel.activeSelf);
	}
}
