using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpgradeController : MonoBehaviour {
	
	public Base _base;
	public Text upgradePoints;
	public Text curText;
	public Text hamText;
	public Text guitarText;
	// Use this for initialization
	void Start ()
	{
		upgradePoints.text = string.Format("Available: {0}",SaveManager.Instance.GetPointCount());
	}
	public void UpgradeSkill(string skillName)
	{
		switch (skillName) {
		case "curtain":
			if(SaveManager.Instance.GetSkillLevel(SkillType.Curtain) < 2 && SaveManager.Instance.GetPointCount() >= _base.skills[0].upgradeCost[SaveManager.Instance.GetSkillLevel (SkillType.Curtain)+1]){
			SaveManager.Instance.SaveSkillLevel (SkillType.Curtain, SaveManager.Instance.GetSkillLevel (SkillType.Curtain) + 1);
			SaveManager.Instance.AddPoints(-_base.skills[0].upgradeCost[SaveManager.Instance.GetSkillLevel (SkillType.Curtain)]);
			upgradePoints.text = string.Format("Available: {0}",SaveManager.Instance.GetPointCount());
				curText.text = string.Format("Робить Афанасія невидимим для ворогів на певний час. Зараз: {0} секунд, наступний рівень {1} секунд",_base.skills[0].powerValue[SaveManager.Instance.GetSkillLevel (SkillType.Curtain)],_base.skills[0].powerValue[SaveManager.Instance.GetSkillLevel (SkillType.Curtain)+1]);
			}
			break;
		case "guitar":
			if (SaveManager.Instance.GetSkillLevel (SkillType.Guitar) < 2 && SaveManager.Instance.GetPointCount() >= _base.skills [1].upgradeCost [SaveManager.Instance.GetSkillLevel (SkillType.Guitar)+1]) {
				SaveManager.Instance.SaveSkillLevel (SkillType.Guitar, SaveManager.Instance.GetSkillLevel (SkillType.Guitar)+1);
				SaveManager.Instance.AddPoints(-_base.skills[1].upgradeCost[SaveManager.Instance.GetSkillLevel (SkillType.Guitar)]);
				upgradePoints.text = string.Format ("Available: {0}", PlayerPrefs.GetInt ("points"));
				guitarText.text = string.Format("Дає можливість на певний час приспати ворогів. Зараз: {0} секунд, наступний рівень {1} секунд",_base.skills[1].powerValue[SaveManager.Instance.GetSkillLevel (SkillType.Guitar)],_base.skills[1].powerValue[SaveManager.Instance.GetSkillLevel (SkillType.Guitar)+1]);
			}
			break;
		case "hammer":
			if (SaveManager.Instance.GetSkillLevel (SkillType.Hammer) < 2 && SaveManager.Instance.GetPointCount() >= _base.skills [2].upgradeCost [SaveManager.Instance.GetSkillLevel (SkillType.Hammer)+1]) {
				SaveManager.Instance.SaveSkillLevel (SkillType.Hammer, SaveManager.Instance.GetSkillLevel (SkillType.Hammer)+1);
				SaveManager.Instance.AddPoints(-_base.skills[2].upgradeCost[SaveManager.Instance.GetSkillLevel (SkillType.Hammer)]);
				upgradePoints.text = string.Format ("Available: {0}", PlayerPrefs.GetInt ("points"));
				hamText.text =  string.Format("Наносить певну кількість шкоди ворогам. Зараз: {0} одиниць шкоди, наступний рівень {1} одиниць шкоди",_base.skills[2].powerValue[SaveManager.Instance.GetSkillLevel (SkillType.Hammer)],_base.skills[2].powerValue[SaveManager.Instance.GetSkillLevel (SkillType.Hammer)+1]);
			}
			break;
		default:
			break;
		}
	}
	public void BackToLevels()
	{
		SceneManager.LoadScene(1);
	}
}
