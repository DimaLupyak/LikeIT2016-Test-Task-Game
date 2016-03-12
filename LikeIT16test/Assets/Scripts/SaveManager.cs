using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour 
{
	const string CURRENT_LEVEL = "CurrentLevel";
	const string LEVEL_SCORE = "ScoreLevel{0}";
	const string LEVEL_STARS = "StarsLevel{0}";
	const string SKILL_LEVEL = "Skill{0}level";
	const string SKILL_POINTS = "Points";

	public bool clearPrefs;

	public static SaveManager Instance;
	public void Start()
	{
		if (SaveManager.Instance != null)
			Destroy(this.gameObject);
		else
		{
			SaveManager.Instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		if (clearPrefs)
			PlayerPrefs.DeleteAll();
	}

	public int GetPointCount()
	{
		return PlayerPrefs.GetInt(SKILL_POINTS);
	}
	public void AddPoints(int addCount)
	{
		Debug.LogWarning(GetPointCount() + " + " + addCount);
		PlayerPrefs.SetInt(SKILL_POINTS, GetPointCount() + addCount);
	}
	#region LEVELS

	public void SaveLevelScore(int score)
	{
		PlayerPrefs.SetInt(string.Format(LEVEL_SCORE, GetCurrentLevel()), score);
	}

	public int GetLevelScore()
	{
		return PlayerPrefs.GetInt(string.Format(LEVEL_SCORE, GetCurrentLevel()));
	}

	public void SaveLevelStars(int stars)
	{
		PlayerPrefs.SetInt(string.Format(LEVEL_STARS, GetCurrentLevel()), stars);
	}

	public int GetLevelStars(int level)
	{
		return PlayerPrefs.GetInt(string.Format(LEVEL_STARS, level));
	}

	public void SetCurrentLevel(int currentLevel)
	{
		PlayerPrefs.SetInt(CURRENT_LEVEL, currentLevel);
	}

	public int GetCurrentLevel()
	{
		return PlayerPrefs.GetInt(CURRENT_LEVEL);
	}
	
	public bool IsLevelOpen(int level)
	{
		return GetLevelStars(level - 1) > 0;
	}

	#endregion

	#region SKILLS

	public void SaveSkillLevel(SkillType skillType, int skillLevel)
	{
		PlayerPrefs.SetInt(string.Format(SKILL_LEVEL, skillType.ToString()), skillLevel);
	}
	public int GetSkillLevel(SkillType skillType)
	{
		return PlayerPrefs.GetInt(string.Format(SKILL_LEVEL, skillType.ToString()));
	}

	#endregion

	void Update()
	{
		//if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex > 0)
		//		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}
}
