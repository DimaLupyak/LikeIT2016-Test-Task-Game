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

	//public Skill curtainSkill, hammerSkill, guitarSkill;
	public bool isFacingRight = true;
	private Rigidbody2D rigi;
	private EnemyController[] enemies;
	private MainController mainController;
    private Animator animator;

    void Start()
	{
        rigi = GetComponent<Rigidbody2D> ();
		mainController = GameObject.FindObjectOfType<MainController>();
        animator = GetComponent<Animator>();
    }

	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");        
		float moveVertical = Input.GetAxis ("Vertical");
        rigi.AddForce(new Vector2(moveHorizontal * speed, moveVertical * speed));
        if(rigi.velocity.x!=0|| rigi.velocity.y != 0) rigi.AddForce(new Vector2(-rigi.velocity.x*10, -rigi.velocity.y*10));

        animator.SetFloat("hSpeed", Mathf.Abs(rigi.velocity.x));

        if (moveHorizontal > 0 && !isFacingRight)
            Flip();
        else if (moveHorizontal < 0 && isFacingRight)
            Flip();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

	private void RefreshSkills()
	{
		skills = new List<Skill>();
		foreach (var sk in mainController._base.skills)
			skills.Add(new Skill(sk.skillType, sk.powerValue[SaveManager.Instance.GetSkillLevel(sk.skillType)]));
	}
	private void UseHammer()
	{
		//play hammer animation
		EnemyController enemy = mainController.FindNearEnemy();
		if (enemy != null)
			enemy.GetDamage((int)GetSkill(SkillType.Hummer).power);
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