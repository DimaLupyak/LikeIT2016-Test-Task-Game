using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour 
{

	public HouseProp[] houses;

	private PlayerController player;
	private int direction;

	const float damageRadius = 3f;

	public Base _base;
	public GameObject batPrefab, panteraPrefab;
	public Transform upBoundTransform;
	public Transform downBoundTransform;
	public Transform leftBoundTransform;
	public Transform rightBoundTransform;

	[HideInInspector]
	public List<EnemyController> enemies;
	[HideInInspector]
	public float upBound, downBound;
	[HideInInspector]
	public float leftBound, rightBound;

	private PuzzlesManager puzzlesManager;
	public static MainController Instance;
	public bool gamePause = false;

	float enemyTimer = 0;
	void CheckCreateEnemy()
	{
		enemyTimer += Time.deltaTime;
		if (enemyTimer > 6 && enemies.Count < 8)
		{
			if ((enemies.Count + 1) % 3 == 0)
				CreateNewEnemy(EnemyType.Bat);
			CreateNewEnemy(EnemyType.Pantera);
			enemyTimer = 0;
		}
		//CreateNewEnemy(EnemyType.Bat);
	}

	void Awake()
	{
		Instance = this;
		if (SaveManager.Instance == null)
			Application.LoadLevel(0);
	}

	void Start()
	{
		Time.timeScale = 1;
		player = GameObject.FindObjectOfType<PlayerController>();
		enemies = new List<EnemyController>();

		upBound = upBoundTransform.position.y;
		downBound = downBoundTransform.position.y;

		leftBound = leftBoundTransform.position.x;
		rightBound = rightBoundTransform.position.x;

        puzzlesManager = new PuzzlesManager(3);


    }

	void Update()
	{
		if (gamePause)
			return;
		if (Input.GetKeyDown(KeyCode.K))
			CreateNewEnemy(EnemyType.Bat);
		if (Input.GetKeyDown(KeyCode.L))
			CreateNewEnemy(EnemyType.Pantera);
		if (Input.GetKeyDown(KeyCode.Q))
			ShowNewHint();
		CheckHouseTouch();
		CheckCreateEnemy();
	}

	void CheckHouseTouch()
	{
		for ( int i = 0; i < houses.Length; i++)
		{
			if (Mathf.Abs((player.transform.position - houses[i].position.transform.position).x) < 0.5f && Mathf.Abs((player.transform.position - houses[i].position.transform.position).y) < 0.5f)
			{
				gamePause = true;
				if (puzzlesManager.GetIndex(houses[i].color) == puzzlesManager.GetIndex(puzzlesManager.Target))
				{
					PopUpManager.Instance.OpenPage(PageType.Win);
					SaveManager.Instance.SaveLevelStars(Timer.Instance.currentStars);
					SaveManager.Instance.AddPoints(Timer.Instance.currentStars);
				}
				else
				{
					PopUpManager.Instance.SetGameOverText("WRONG HOUSE!");
					PopUpManager.Instance.OpenPage(PageType.GameOver);
				}
			}
		}
	}

	public List<string> GetHints()
	{
		return puzzlesManager.GeneratedHints;
	}
	public EnemyController FindNearEnemy()
	{
		EnemyController tmpEnemy = new EnemyController();
		float tmpDistance = 100;
		foreach (var enemy in enemies)
		{
			float distance = Mathf.Abs(player.transform.position.x - enemy.transform.position.x);
			if (distance < damageRadius && distance < tmpDistance)
			{
				tmpDistance = distance;
				tmpEnemy = enemy;
			}
		}
		if (tmpDistance == 100)
			return null;
		return tmpEnemy;
	}

	public void UseSkill(SkillType skillType)
	{
		player.UseSkill(skillType);
	}
	public void CreateNewEnemy(EnemyType createEnemyType)
	{
		var randomPos = new Vector3(player.transform.position.x + Random.Range(17, 30), Random.Range(downBound * 100f, upBound * 100f) / 100f, -0.1f); 
		randomPos.y = createEnemyType == EnemyType.Pantera ? randomPos.y : Random.Range(-250, 0) / 100f;
		var enemyPrefab = createEnemyType == EnemyType.Bat ? batPrefab : panteraPrefab;
		GameObject tmpEnemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity) as GameObject;
		enemies.Add(tmpEnemy.GetComponent<EnemyController>());
	}

	public void ShowNewHint()
	{
		PopUpManager.Instance.OpenPage(PageType.NewHint);
		PopUpManager.Instance.ShowNewHint(puzzlesManager.GetHint());
	}


}

[System.Serializable]
public class HouseProp
{
	public string color;
	public Transform position;
}
