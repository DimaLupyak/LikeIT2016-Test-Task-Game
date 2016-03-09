using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainController : MonoBehaviour 
{
	private List<EnemyController> enemies;
	private PlayerController player;
	private int direction;

	public Base _base;
	public GameObject enemyPrefab;

	const float damageRadius = 2f;
	const float upBound = -1.5f;
	const float downBound = -5f;
	void Start()
	{
		player = GameObject.FindObjectOfType<PlayerController>();
		enemies = new List<EnemyController>();
	}

	void LoadSkills()
	{
		
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
		var randomPos = new Vector3(player.transform.position.x + Random.Range(5, 15), Random.Range(downBound, upBound), -0.1f); 
		GameObject tmpEnemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity) as GameObject;
		enemies.Add(tmpEnemy.GetComponent<EnemyController>());
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
			CreateNewEnemy();
	}
}
