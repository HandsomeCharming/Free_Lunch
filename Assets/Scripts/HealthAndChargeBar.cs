using UnityEngine;
using System.Collections;

public class HealthAndChargeBar : MonoBehaviour {

	public Character character;

	public Texture2D chargeBar;

	public ArrayList hpBarPart;
	public float hpBarRadius;

	public int cursorSize = 63;
	public int hpChangeFrame = 3;
	int hpChangeFrameRemain = 0;
	int sizeX;
	int sizeY;
	float hp;

	ColorState colorState = ColorState.Yellow;

	GameObject cursorCharge;

	enum ColorState {
		Yellow,
		Orange,
		Red
	}

	void Awake() {
		sizeX = cursorSize;
		sizeY = cursorSize;
	}

	void Start () {
		cursorCharge = (GameObject)Instantiate (Resources.Load ("Prefabs/CursorCharge"));
		hpBarPart = new ArrayList();
		for(int a=0;a!=20;++a) {
			/*float degree = 17f*a+194f;
			float rad = degree*Mathf.Deg2Rad;*/
			float degree = 18f*a + 9;
			float rad = degree*Mathf.Deg2Rad;
			Vector3 pos = new Vector3(hpBarRadius*Mathf.Sin(rad), 0.5f,  hpBarRadius*Mathf.Cos(rad));
			Vector3 rot = new Vector3(90f, 90f+degree, 0);
			GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/HPBarPart"), pos, Quaternion.Euler(rot));
			obj.transform.parent = gameObject.transform;
			obj.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
			SpriteRenderer rend = obj.GetComponent<SpriteRenderer>();
			rend.color = new Color(240,255, 0);
			hpBarPart.Add(obj);
		}
		hp = character.status.hp;
	}

	void Update () {
		if(character == null) {
			Destroy(this.gameObject);
			return;
		}

		if(InputHandler.current.chargingTime > 0) {
			cursorCharge.SetActive(true);
			showChargeBar();
		} else {
			cursorCharge.SetActive(false);
		}

		if(hp != character.status.hp) {
			if(hpChangeFrameRemain > 0) {
				hpChangeFrameRemain --;
			} else {
				if(hp > character.status.hp) {
					hp -= 5;
				} else {
					hp += 5;
				}
				hpChangeFrameRemain = hpChangeFrame;

				for(int a=19;a!=0;--a) {
					GameObject obj =  (GameObject)hpBarPart[a];
					if(hp <= 5*a) {
						obj.SetActive(false);
					} else {
						obj.SetActive(true);
					}
				}
				if(hp/character.status.maxhp <= 0.3f && colorState != ColorState.Red) {
					foreach (GameObject obj in hpBarPart) {
						SpriteRenderer rend = obj.GetComponent<SpriteRenderer>();
						rend.color = new Color(255f/255f,50f/255f, 0);
					}
					colorState = ColorState.Red;
				} else if(hp/character.status.maxhp <= 0.6f && colorState == ColorState.Yellow) {
					for(int a=0; a!=20;++a) {
						GameObject obj =  (GameObject)hpBarPart[a];
						SpriteRenderer rend = obj.GetComponent<SpriteRenderer>();
						rend.color = new Color(255/255f,164f/255f, 0);
					}
					colorState = ColorState.Orange;
				} 
			}
		}

		transform.position = character.transform.position;

	}

	void showChargeBar() {
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = MainCamera.current.transform.position.y;
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

		float lerp = (InputHandler.current.chargingTime)/character.chargedAttackModifier.chargeTime;
		float scale = Mathf.Lerp(cursorSize, 10, lerp);

		cursorCharge.transform.position = mousePosition;
		cursorCharge.transform.localScale = new Vector3(scale, scale, scale);
	}

	void OnGUI()
	{
		/*if(InputHandler.current.chargingTime > 0) {
			
			float lerp = (InputHandler.current.chargingTime)/character.chargedAttackModifier.chargeTime;
			float scale = Mathf.Lerp(cursorSize, 20, lerp);

			GUI.DrawTexture(new Rect(Event.current.mousePosition.x - (scale/2), Event.current.mousePosition.y - (scale/2), scale, scale), chargeBar);
		}*/
	}
}

