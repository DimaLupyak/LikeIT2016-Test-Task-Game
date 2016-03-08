using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainController : MonoBehaviour 
{
	private List<EnemyController> enemies;
	private PlayerController player;
	private int direction;

	public Base _base;

	const float damageRadius = 2f;

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
		
	}
}
