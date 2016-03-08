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

public enum SkillType {Curtain, Guitar, Hummer};
[System.Serializable]
public class SkillSettings
{
	public SkillType skillType;
	public int[] upgradeCost;
	public float[] powerValue;
}
