using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour 
{
	//----------STATS---------
	private float health = 100;
	private List<Skill> skills;
	public float speed = 100;
    //-------------------------

	private EnemyController[] enemies;
	private MainController mainController;
    private Animator animator;

    void Start()
	{
		mainController = GameObject.FindObjectOfType<MainController>();
        animator = GetComponent<Animator>();
		RefreshSkills();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

		if (!(transform.position.y + moveVertical * speed < mainController.upBound && transform.position.y + moveVertical * speed > mainController.downBound))
			moveVertical = 0;
		transform.Translate(moveHorizontal * speed, moveVertical * speed, 0);
		animator.SetFloat("hSpeed", moveHorizontal  + moveVertical == 0 ? 0  : 1);
		Vector3 theScale = transform.localScale;
		theScale.x = moveHorizontal > 0 ? Mathf.Abs(this.transform.localScale.x) * -1 : moveHorizontal < 0 ? Mathf.Abs(this.transform.localScale.x) * 1 : theScale.x;
		transform.localScale = theScale;
		if (Input.GetButtonDown("Jump"))
			UseGuitar();
		if (Input.GetKeyDown(KeyCode.H))
			UseHammer();
    }

	private void RefreshSkills()
	{
		skills = new List<Skill>();
		foreach (var sk in mainController._base.skills)
			skills.Add(new Skill(sk.skillType, sk.powerValue[SaveManager.Instance.GetSkillLevel(sk.skillType)]));
	}
	private void UseGuitar()
	{
		animator.SetBool("GuitarPlaying", !animator.GetBool("GuitarPlaying"));
		foreach (var enemy in mainController.enemies)
			enemy.GetGuitarDamage(GetSkill(SkillType.Guitar).power);
	}
	private void UseHammer()
	{
		//play hammer animation
		EnemyController enemy = mainController.FindNearEnemy();
		if (enemy != null)
			enemy.GetHammerDamage((int)GetSkill(SkillType.Hummer).power);
		//minus hammer energy
	}

	private Skill GetSkill(SkillType skillType)
	{
		foreach (var sk in skills)
			if (sk.skillType == skillType)
				return sk;
		return new Skill();
	}
}

[System.Serializable]
public class Skill
{
	public SkillType skillType;
	public float power;
	public Skill(){}
	public Skill(SkillType _skillType, float _power)
	{
		skillType = _skillType;
		power = _power;
	}
}