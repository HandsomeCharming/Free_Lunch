using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillDescription : MonoBehaviour {

	public static SkillDescription current;

	public Image baseImage;
	public Text text;
	// Use this for initialization
	void Start () {
		current = this;
	}

	void Awake() {
		current = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
