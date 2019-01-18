using UnityEngine;
using TMPro;
public class scrEffect : MonoBehaviour {
	TMP_Text tt;
	float timmer;
	Color transparent;
	// Use this for initialization
	void Start () 
	{
		transparent = new Color (0, 0, 0, 0);
		tt = GetComponent<TMP_Text> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		timmer += Time.deltaTime;
		tt.color = Color.Lerp (Color.green, transparent, timmer);
	}
}
