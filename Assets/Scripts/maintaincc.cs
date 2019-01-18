using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class maintaincc : MonoBehaviour 
{
	public Toggle showTutorial;
	public GameObject tutorialWindows;
	int displayTutoial = 0;
	public AudioSource pickUpSound;
	bool preventInteraction = false,gamePaused = false,startResuming = false;
	float resumeTimer = 4f;

	public GameObject inGameScreen,pauseScreen;
	public GameObject[] forestPrefabs,crystalPrefabs;
	public manager mgr;
	public GameObject gameOverPannel, scoreEffect, canvass, pauseButton;
	public GameObject[] gifts;
	Camera cm;
	int target1,target2;
	public TMP_Text scrr,gameOverScr,resumeText;int score;

	float zz = 19.1f;
	float grndZ = 206f;
	float ll = 9f,lr = 11f,rl = 20f,rr = 24f,zforTarget = 14f;
	//public GameObject ground1targets,ground2tragets,ground3targets;
	// Use this for initialization
	void Start ()
	{
		//PlayerPrefs.SetInt ("showtutorial", 0);
		displayTutoial = PlayerPrefs.GetInt ("showtutorial");
		cm = Camera.main;
		Instantiate (returnRandomLand(), new Vector3 (13.272f, 1.63f, 5.34f), Quaternion.identity);
		Instantiate (returnRandomLand(), new Vector3 (13.272f, 1.63f, 105.2f), Quaternion.identity);
		Instantiate (returnRandomLand(), new Vector3 (13.272f, 1.63f, 205.2f), Quaternion.identity);
	}
	GameObject returnRandomLand()
	{
		if (Random.Range (0, 2) == 0) 
		{
			return forestPrefabs [Random.Range (0, 3)];
		} 
		else 
		{
			return crystalPrefabs [Random.Range (0, 3)];
		}
	}
	// Update is called once per frame
	void Update ()
	{
		if (displayTutoial == 0)
		{
			if ((int)transform.position.z == 7)
				startTutorial ();
		}
		if (!preventInteraction)
		    handleInput ();
		if (gamePaused)
			waitForResumeTap ();
		if (startResuming)
			resumeGame();
	}
	void startTutorial()
	{
		mgr.gameOver = true;
		preventInteraction = true;
		tutorialWindows.SetActive (true);
		displayTutoial = 1;
	}
	public void okButtonClicked()
	{
		mgr.gameOver = false;
		preventInteraction = false;
		tutorialWindows.SetActive (false);
		if (showTutorial.isOn)
			PlayerPrefs.SetInt ("showtutorial", 1);
	}
	void OnTriggerEnter(Collider cc)
	{
		//checkBg (cc.gameObject.tag);
		//Debug.Log (cc.gameObject.tag);
		if (cc.gameObject.tag == "target1" || cc.gameObject.tag == "target2") 
		{
			handleTarget (cc.gameObject.tag);
		}
		if (cc.gameObject.tag == "slab")
		{
			zz += 0.59f;
			cc.gameObject.transform.position = new Vector3 (13.365f, 1.615f, zz);
		}
	}
	void OnTriggerExit(Collider cdd)
	{
		string dt = cdd.gameObject.tag;
		if (dt == "target1" || dt == "target2") 
		{
			Debug.Log ("targt");
			resetTarget (cdd.gameObject);
		}
		if (dt == "grnd1" || dt == "grnd2" || dt == "grnd3") 
		{
			grndZ += 99.2f;
			cdd.gameObject.transform.position = new Vector3 (13.2722f, 1.63f, grndZ);
		}
	}
	void resetTarget(GameObject target)
	{
	   if (Random.Range (0, 2) == 1) //left half or right half
	  { 
			//left side
			zforTarget += Random.Range(15,30);
			target.transform.position = new Vector3(Random.Range(ll,lr),1.69f,zforTarget);
	 } 
		else
	{
		//right side
		zforTarget += Random.Range(15,30);
		target.transform.position = new Vector3(Random.Range(rl,rr),1.69f,zforTarget);
	}

	}
	void handleInput()
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) 
			{
				if (hit.transform.tag == "target1")
				{
					if (target1 == 0) 
					{
						target1 = 1;
						score++;
						pickUpSound.Play ();
						scrr.SetText (score.ToString ());
						giveReward (hit.transform.position);
					}
				} 
				else if (hit.transform.tag == "target2") 
				{
					if (target1 == 0) 
					{
						target2 = 1;
						score++;
						pickUpSound.Play ();
						scrr.SetText (score.ToString ());
						giveReward (hit.transform.position);
					}
				}
			}
		}
	}
	void handleTarget(string tg)
	{
		if (tg == "target1") 
		{
			if (target1 == 0) 
			{
				stopTheGame ();
			}
			else 
			{
				target1 = 0;
			}
				Debug.Log ("missed t1");
		} 
		else if (tg == "target2") 
		{
			if (target2 == 0) 
			{
				stopTheGame ();
			}
			else 
			{
				target2 = 0;
			}
				Debug.Log ("missed t2");
		}
	}
	void giveReward(Vector3 spawnPos)
	{
		spawnPos.x -= 5f;
		Instantiate (gifts [Random.Range (0, gifts.Length)], spawnPos, Quaternion.identity);
		//score effect
		GameObject t = Instantiate(scoreEffect,cm.WorldToScreenPoint(spawnPos),Quaternion.identity);
		t.transform.SetParent (canvass.transform);
	}
	void stopTheGame()
	{
		mgr.gameOver = true;
		preventInteraction = true;
		canvass.SetActive (true);
		inGameScreen.SetActive (false);
		gameOverPannel.SetActive (true);
		gameOverScr.SetText (checkAndDisplayHighScore());
	}
	string checkAndDisplayHighScore()
	{
		int oldHighScore = PlayerPrefs.GetInt ("score");
		Debug.LogError ("prev sc " + oldHighScore + " new " + score);
		if (oldHighScore < score) 
		{
			//player made the high score
			PlayerPrefs.SetInt ("score", score);
			return"New High Score : " + score.ToString ()+"\n Old High Score : "+oldHighScore.ToString();
		} 
		else 
		{
			return"Your Score : "+score.ToString()+"\nHigh Score : "+oldHighScore.ToString();
		}

	}
	public void onPauseButtonClicked()
	{
		mgr.gameOver = true;
		preventInteraction = true;
		gamePaused = true;
		pauseScreen.SetActive (true);
		resumeText.SetText ("TAP TO RESUME");
		pauseButton.SetActive (false);
	}
	void waitForResumeTap ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			startResuming = true;
		}
	}
	void resumeGame()
	{
		resumeTimer -= Time.deltaTime;
		int t = (int)resumeTimer;
		if (t == 0) 
		{
			resumeTimer = 3f;
			gamePaused = false;
			preventInteraction = false;
			mgr.gameOver = false;
			pauseButton.SetActive (true);
			pauseScreen.SetActive (false);
			startResuming = false;
		}
		else 
		{
			resumeText.SetText (t.ToString ());
		}
	}
}








