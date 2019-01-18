using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class mapp : MonoBehaviour {
	public string level = "";
	public GameObject[] forestPrefabs,crystalPrefabs;
	public GameObject helpPannel,mainMenuPanel,loadingPanel;
	bool helpActive = false;
	public TMP_Text debugLoad;
	public Slider loadProgress;

	GameObject a,b,c;
	// Use this for initialization
	void Start () 
	{
		//DontDestroyOnLoad (gameObject);
	//	SceneManager.sceneLoaded += OnSceneLoaded;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (helpActive) 
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				mainMenuPanel.SetActive (true);
				helpPannel.SetActive (false);
				helpActive = false;
			}
		}
	}
	public void onPlayButtonClicked()
	{
	  //  SceneManager.LoadScene (1);
		loadingPanel.SetActive(true);
		StartCoroutine(laodGameLevel());
		//Debug.LogError ("a " + a.progress);
	}
	public void onHelpButtonClicked()
	{
		mainMenuPanel.SetActive (false);
		helpPannel.SetActive (true);
		helpActive = true;
	}
	IEnumerator laodGameLevel()
	{
		AsyncOperation op = SceneManager.LoadSceneAsync (1);
		while (!op.isDone) 
		{
			float progress = Mathf.Clamp01 (op.progress / .9f);
			debugLoad.SetText ("Loading  "+(progress*100).ToString ("n0")+"%");
			loadProgress.value = progress;
			yield return null;
		}
	}
	public void onExitButtonCLicked()
	{
		Application.Quit ();
	}
}