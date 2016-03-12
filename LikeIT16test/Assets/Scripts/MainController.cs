using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour 
{
	private PlayerController player;
	private int direction;

	const float damageRadius = 2f;

	public Base _base;
	public GameObject batPrefab, panteraPrefab;
	public Transform upBoundTransform;
	public Transform downBoundTransform;

	[HideInInspector]
	public List<EnemyController> enemies;
	[HideInInspector]
	public float upBound, downBound;

    private PuzzlesManager puzzlesManager;
	public static MainController Instance;
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
        puzzlesManager = new PuzzlesManager(3);
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
		var randomPos = new Vector3(player.transform.position.x + Random.Range(5, 15), Random.Range(downBound * 100f, upBound * 100f) / 100f, -0.1f); 
		randomPos.y = createEnemyType == EnemyType.Pantera ? randomPos.y : Random.Range(-250, 50) / 100f;
		var enemyPrefab = createEnemyType == EnemyType.Bat ? batPrefab : panteraPrefab;
		GameObject tmpEnemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity) as GameObject;
		enemies.Add(tmpEnemy.GetComponent<EnemyController>());
	}

	public void ShowNewHint()
	{
		PopUpManager.Instance.OpenPage(PageType.NewHint);
		PopUpManager.Instance.ShowNewHint(puzzlesManager.GetHint());
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
			CreateNewEnemy(EnemyType.Bat);
        if (Input.GetKeyDown(KeyCode.L))
            CreateNewEnemy(EnemyType.Pantera);
        if (Input.GetKeyDown(KeyCode.Q))
			ShowNewHint();
    }
}
