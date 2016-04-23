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
		float hp = boss.status.hp;
		if(hp < 0)hp = 0f;
		Vector3 scale = hpBar.transform.localScale;
		scale.x = (hp/boss.status.maxhp)*10f;
		hpBar.transform.localScale = scale;
		Vector3 pos = hpBar.transform.localPosition;
		pos.x =-(1f-(hp/boss.status.maxhp))*370f;
		hpBar.transform.localPosition = pos;
	}
}
