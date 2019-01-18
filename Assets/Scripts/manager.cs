using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class manager : MonoBehaviour {
	public GameObject permanent;
	public GameObject slab;
	float trainSpeed = 9f;
	public bool gameOver = false;
	public GameObject loadingPannel;
	public TMP_Text debugLoad;
	public Slider loadProgress;
	// Use this for initialization
	void Start () 
	{
		
		float ze = 7.29f;
		for (int i = 1; i <= 50; i++) 
		{
			ze += 0.59f;
			Instantiate (slab, new Vector3 (13.365f, 1.615f, ze), Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!gameOver) 
		{
			sequence ();
		}
	}
	void sequence()
	{
		trainSpeed += 0.004f;
		Debug.Log (trainSpeed);
		permanent.transform.Translate (new Vector3 (0, 0, trainSpeed * Time.deltaTime  ));
	}
	public void onRetrybuttonclicked()
	{
		SceneManager.LoadScene (1);
	}
	public void onGameoverMenuButtonClicked()
	{
		//SceneManager.LoadScene (0);
		loadingPannel.SetActive(true);
		StartCoroutine(laodGameLevel());
	}
	IEnumerator laodGameLevel()
	{
		AsyncOperation op = SceneManager.LoadSceneAsync (0);
		while (!op.isDone) 
		{
			float progress = Mathf.Clamp01 (op.progress / .9f);
			debugLoad.SetText ("Loading  "+(progress*100).ToString ("n0")+"%");
			loadProgress.value = progress;
			yield return null;
		}
	}
}







