using UnityEngine;
using System.Collections;

public class CubeExplosion : MonoBehaviour {
	public Vector2 dir;
	public int particles;
	// Use this for initialization
	void Start () {
		for(int a=0;a!=particles;++a) {
			float angle = (((float)a)/particles)*2*Mathf.PI;
			GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/CubeAfterExplosion"), transform.position + new Vector3(Mathf.Cos(angle)*0.1f, 1f,Mathf.Sin(angle)*0.1f), Quaternion.identity ); // new Vector3(Mathf.Cos(angle)*0.1f, 1f,Mathf.Sin(angle)*0.1f)
			obj.transform.SetParent(this.transform);
			obj.GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Cos(angle)*50f, 0f,Mathf.Sin(angle)*50f);

			StartCoroutine( disableRenderer(0.3f));
		}
	}

	IEnumerator disableRenderer(float time) {
		yield return new WaitForSeconds(0.2f);
		for(int a=0;a!=particles;++a) {
			GameObject obj = (GameObject)transform.GetChild(a).gameObject;
			obj.GetComponent<MeshRenderer>().enabled = false;
			Color color =  obj.GetComponent<MeshRenderer>().material.color;
			color.a = 0.0f;
			obj.GetComponent<MeshRenderer>().material.color = color;
			//obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
			Destroy(obj, 0.3f);
		}

		Destroy(this.gameObject, 0.6f);
	}

	void Update() {
		Vector3 pos = this.transform.position;
		pos.x += dir.x*1.0f;
		pos.z += dir.y*1.0f;
		this.transform.position = pos;
	}
}
