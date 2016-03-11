using UnityEngine;
using System.Collections;

public class Base : MonoBehaviour 
{
	public LevelSettings[] levels;
	public SkillSettings[] skills;
}

[System.Serializable]
public class LevelSettings
{
	public string name;
	public int scoreBronze, scoreSilver, scoreGold;
}

public enum SkillType {Curtain = 1, Hammer = 2, Guitar = 3, None = 0};
[System.Serializable]
public class SkillSettings
{
	public SkillType skillType;
	public int[] upgradeCost;
	public float[] powerValue;
}
