using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenController : MonoBehaviour {

	public Button resumeButton;
	public Button createButton;
	public Button exitButton;
	private CanvasRenderer canvasRenderer;

	// Use this for initialization
	void Start () {
		canvasRenderer = GetComponent<CanvasRenderer> ();
		Debug.Assert (canvasRenderer != null);

		resumeButton.onClick.AddListener (() => {
			gameObject.SetActive (false);
		});

		createButton.onClick.AddListener (() => {

		});

		exitButton.onClick.AddListener (() => {

		});
	}
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			gameObject.SetActive (!gameObject.activeSelf);
		}
	}
}
