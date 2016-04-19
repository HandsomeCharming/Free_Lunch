using UnityEngine;
using System.Collections;

public class DragableUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnDrag() { 
		transform.position = Input.mousePosition; 
	}
}
