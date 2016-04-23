using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public enum SkillDiv {
	Devour,
	Division,
	Computation
}

public class SkillButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	public SkillDiv skilldiv;

	public int skillType = 0;
	public int skillLevel = 0;  //Level in the skill tree
	public int skillCount = 0;  //Level of the skill
	public int skillCountMax = 1;
	public string skillDescription;
	public string skillTitle;
	public int prereSkillType = -1;

	public int pressed {get; set;}

	public Image baseImage;
	public bool selected = false;
	public bool canClick = true;

	public Text description;

	SkillTreeHandler skillTreeHandler;

	Transform oriTrans;


	bool pickedUp = false;

	void Start() {
		oriTrans = transform;
		skillTreeHandler = SkillTreeHandler.skillTreeHandler;//transform.parent.gameObject.GetComponent<SkillTreeHandler>();
	}

	void Awake() {
		oriTrans = transform;
	}

	public void OnPointerClick(PointerEventData eventData) {
		Image image = this.gameObject.GetComponent<Image>();
		if(skillTreeHandler == null) {
			skillTreeHandler = SkillTreeHandler.skillTreeHandler;
		}
		/*if (eventData.button == PointerEventData.InputButton.Left) {
			if(skillTreeHandler.clickSkill(this)) {
				if(pressed == 0) {
					pressed = 1;
					image.color = Color.red;
				}
			}
		}
		else if (eventData.button == PointerEventData.InputButton.Right) {
			if(skillTreeHandler.rightClickSkill(this)) {
				if(pressed == 1) {
					pressed = 0;
					image.color = Color.white;
				}
			}
		}*/
	}


	public void disableClick() {
		canClick = false;
		GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Skilltree/SkillForbiddenBase");
	}

	public void enableClick() {
		canClick = true;
		GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Skilltree/SkillTreeDBase");
	}

	public void OnDrag() { 
		if(!canClick)return;
		pickedUp = true;
		transform.position = Input.mousePosition; 
		if(Input.mousePosition.y > Screen.height*0.65) {
			GameObject obj = (GameObject)SkillSlot.current.slots[skillLevel];
			Image image = obj.GetComponent<Image>();
			if(image.sprite.name != "SkillSlotHighlighted")
				image.sprite = Resources.Load<Sprite>("Sprites/Skilltree/SkillSlotHighlighted");
			if(baseImage.sprite.name != "SkillSlot")
				baseImage.sprite = Resources.Load<Sprite>("Sprites/Skilltree/SkillSlot");
			
		} 
		else {
			GameObject obj = (GameObject)SkillSlot.current.slots[skillLevel];
			Image image = obj.GetComponent<Image>();
			if(image.sprite.name != "SkillSlot")
				image.sprite = Resources.Load<Sprite>("Sprites/Skilltree/SkillSlot");
			if(baseImage.sprite.name != "SkillSlotHighlighted")
				baseImage.sprite = Resources.Load<Sprite>("Sprites/Skilltree/SkillSlotHighlighted");
		}
	}

	public void OnDrop() {
		if(!canClick)return;
		if(Input.mousePosition.y > Screen.height*0.65) {
			transform.position = ((GameObject)SkillSlot.current.slots[skillLevel]).transform.position;
			selected = true;
			SkillTreeHandler.skillTreeHandler.clickSkill(this);
		} else {
			SkillTreeHandler.skillTreeHandler.removeSkill(this);
			selected = false;
			transform.position =  baseImage.transform.position;
		}

	}

	//Triggered when loaded save data.
	public void LoadedClick() {
		if(pressed == 0) {
			Image image = this.gameObject.GetComponent<Image>();
			pressed = 1;
			image.color = Color.red;
		}
	}

	public void OnPointerEnter(PointerEventData eventData) {
		//if(description != null) return;
		//description = ((GameObject)Instantiate(Resources.Load("Prefabs/SkillDescriptionText"))).GetComponent<Text>();
		//description.gameObject.transform.parent = this.transform;
		//description.gameObject.transform.SetParent(this.transform);
		//description.text = "amigo";
		//description.rectTransform.position = this.GetComponent<Image>().rectTransform.position;
		if(SkillDescription.current == null)return;
		description.enabled = true;
		SkillDescription.current.baseImage.enabled = true;
	}

	public void OnPointerExit(PointerEventData eventData) {
		/*if(description != null) {
			Destroy(description.gameObject);
		}*/
		if(SkillDescription.current == null)return;
		description.enabled = false;
		SkillDescription.current.baseImage.enabled = false;
		//Debug.Log("Exit");
	}
}
