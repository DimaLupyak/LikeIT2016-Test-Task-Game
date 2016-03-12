using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class Button : MonoBehaviour 
{
	public enum ButtonAction {GoToScene, GoToLevel, UseSkill, CreateEnemy, PrevHint, NextHint, ClosePopUp, ShowHintList, Next}
	public ButtonAction buttonAction;
	public bool changeSprite;

	[HideInInspector]
	public bool attachToSide;
	[HideInInspector]
	public Vector2 normalResolution;
	[HideInInspector]
	public int parm;
	[HideInInspector]
	public Sprite buttonOff, buttonOn;
	[HideInInspector]
	public SkillType skillType;

	const float scaleValue = 0.9f;

	void Start()
	{
		if (attachToSide)
			AttachToSide();
	}
	void OnMouseDown()
	{
		if (changeSprite)
			this.GetComponent<SpriteRenderer>().sprite = buttonOn;
		else
			this.transform.localScale *= scaleValue;
	}

	void OnMouseUp()
	{
		if (changeSprite)
			this.GetComponent<SpriteRenderer>().sprite = buttonOff;
		else
			this.transform.localScale /= scaleValue;
		DoAction();
	}
		
	void DoAction()
	{
		switch (buttonAction)
		{
		case ButtonAction.GoToScene:
			SceneManager.LoadScene(parm);
			break;
		case ButtonAction.GoToLevel:
			SaveManager.Instance.SetCurrentLevel(parm);
			SceneManager.LoadScene(parm + 1);
			break;
		case ButtonAction.UseSkill:
			MainController.Instance.UseSkill(skillType);
			break;
		case ButtonAction.CreateEnemy:
			MainController.Instance.CreateNewEnemy(EnemyType.Bat);
			break;
		case ButtonAction.PrevHint:
			PopUpManager.Instance.ShowNextHint(false);
			break;
		case ButtonAction.NextHint:
			PopUpManager.Instance.ShowNextHint(true);
			break;
		case ButtonAction.ClosePopUp:
			PopUpManager.Instance.CloseWindow();
			break;
		case ButtonAction.ShowHintList:
			PopUpManager.Instance.OpenPage(PageType.HintList);
			PopUpManager.Instance.UpdateTips();
			break;
		case ButtonAction.Next:
			SceneManager.LoadScene(Application.loadedLevel + 1);
			break;
		default:
			Debug.LogWarning(buttonAction.ToString());
			break;
		}
	}

	public void Disable()
	{
		TextMesh txt = this.transform.GetChild(0).GetComponent<TextMesh>();
		txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0.5f);
		this.GetComponent<BoxCollider2D>().enabled = false;
	}
	public void Enable()
	{
		TextMesh txt = this.transform.GetChild(0).GetComponent<TextMesh>();
		txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1);
		this.GetComponent<BoxCollider2D>().enabled = true;
	}
	public void AttachToSide()
	{
		var tmpPos = this.transform.position;
		tmpPos.x *= ((float)Screen.width / (float)Screen.height) / (normalResolution.x / normalResolution.y);
		this.transform.position = tmpPos;
	}
	public void SetNormalResolution()
	{
		normalResolution = new Vector2(Screen.width, Screen.height);
		Debug.LogWarning("Set!");
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(Button))]
public class ButtonInspector : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		var button = target as Button;

		if (button.buttonAction == Button.ButtonAction.UseSkill)
		{
			GUILayout.Space(20);
			EditorGUILayout.LabelField("Skill:");
			button.skillType = (SkillType)EditorGUILayout.EnumPopup(button.skillType);
			GUILayout.Space(20);
		}
		if (button.changeSprite)
		{
			button.buttonOff = (Sprite)EditorGUILayout.ObjectField("OFF:", button.buttonOff, typeof(Sprite), false);
			button.buttonOn = (Sprite)EditorGUILayout.ObjectField("ON:", button.buttonOn, typeof(Sprite), false);
		}
		if (button.buttonAction == Button.ButtonAction.GoToScene)
		{
			GUILayout.Space(20);
			EditorGUILayout.LabelField("Go to scene " + button.parm);
			button.parm = EditorGUILayout.IntSlider(button.parm, 0, SceneManager.sceneCountInBuildSettings);
		}

		if (button.attachToSide = EditorGUILayout.Toggle("Присунути!", button.attachToSide))
		{
			if (GUILayout.Button("Зараз норм"))
				button.SetNormalResolution();
		}
	}
}
#endif
