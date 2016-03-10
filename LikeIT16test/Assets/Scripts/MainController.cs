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
	public GameObject enemyPrefab;
	public Transform upBoundTransform;
	public Transform downBoundTransform;

	[HideInInspector]
	public List<EnemyController> enemies;
	[HideInInspector]
	public float upBound, downBound;

    private PuzzlesManager puzzlesManager;

	void Awake()
	{
		//if (SaveManager.Instance == null)
		//	SceneManager.LoadScene(0);
	}
	void Start()
	{
		player = GameObject.FindObjectOfType<PlayerController>();
		enemies = new List<EnemyController>();
		upBound = upBoundTransform.position.y;
		downBound = downBoundTransform.position.y;
        puzzlesManager = new PuzzlesManager(3);
        puzzlesManager.LogTable();
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


	void CreateNewEnemy()
	{
		var randomPos = new Vector3(player.transform.position.x + Random.Range(5, 15), Random.Range(downBound * 100f, upBound * 100f) / 100f, -0.1f); 
		GameObject tmpEnemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity) as GameObject;
		enemies.Add(tmpEnemy.GetComponent<EnemyController>());
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
			CreateNewEnemy();
	}
}
