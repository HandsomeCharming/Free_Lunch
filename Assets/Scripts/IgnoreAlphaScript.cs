using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IgnoreAlphaScript : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		GetComponent<Image>().eventAlphaThreshold = 0.01f;
	}
}