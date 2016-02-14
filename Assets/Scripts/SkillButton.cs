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
	public int skillLevel = 0;
	public int skillCount = 0;
	public int skillCountMax = 1;
	public string skillDescription;
	public string skillTitle;

	public int pressed {get; set;}

	Text description;

	SkillTreeHandler skillTreeHandler;

	void Start() {
		skillTreeHandler = transform.parent.gameObject.GetComponent<SkillTreeHandler>();
	}

	public void OnPointerClick(PointerEventData eventData) {
		Image image = this.gameObject.GetComponent<Image>();
		if (eventData.button == PointerEventData.InputButton.Left) {
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
		}
	}

	public void OnPointerEnter(PointerEventData eventData) {
		if(description != null) return;
		description = ((GameObject)Instantiate(Resources.Load("Prefabs/SkillDescriptionText"))).GetComponent<Text>();
		//description.gameObject.transform.parent = this.transform;
		description.gameObject.transform.SetParent(this.transform);
		description.text = "amigo";
		description.rectTransform.position = this.GetComponent<Image>().rectTransform.position;
	}

	public void OnPointerExit(PointerEventData eventData) {
		if(description != null) {
			Destroy(description.gameObject);
		}
		//Debug.Log("Exit");
	}
}
