using UnityEngine;
using System.Collections;

public class HPBarCanvas : MonoBehaviour {

	public Character boss;

	public GameObject hpBar;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(boss == null) hpBar.SetActive(false);
		Vector3 scale = hpBar.transform.localScale;
		scale.x = (boss.status.hp/boss.status.maxhp)*10f;
		hpBar.transform.localScale = scale;
		Vector3 pos = hpBar.transform.localPosition;
		pos.x =-(1f-(boss.status.hp/boss.status.maxhp))*370f;
		hpBar.transform.localPosition = pos;
	}
}
