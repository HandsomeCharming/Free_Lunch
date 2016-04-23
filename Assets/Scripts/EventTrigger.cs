using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EventTrigger : MonoBehaviour {

	public UnityEvent mEvent;

	public int eventType = 0;

	void OnTriggerEnter(Collider coll) {
		if(coll.tag == "Player") {
			mEvent.Invoke();
		}
	}
}
