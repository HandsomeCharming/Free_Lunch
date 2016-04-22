using UnityEngine;
using System.Collections;

public class SkillSlot : MonoBehaviour {
	public GameObject[] slots;

	public static SkillSlot current;
	// Use this for initialization
	void Start () {
		current = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
