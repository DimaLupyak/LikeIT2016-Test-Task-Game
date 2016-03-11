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

	public GameObject body, healthBar;

    private Joystick joystick;
    private EnemyController[] enemies;
    private MainController mainController;
    private Animator animator;
	private float stratHealBarX;

	public bool isDie = false;

    void Start()
    {
		stratHealBarX = healthBar.transform.localScale.x;
        mainController = GameObject.FindObjectOfType<MainController>();
        joystick = GameObject.FindObjectOfType<Joystick>();
        animator = GetComponent<Animator>();
        RefreshSkills();
    }

    void Update()
    {
		if (isDie)
			return;
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (moveHorizontal == 0 && moveVertical == 0)
        {
            moveHorizontal = joystick.GetAxis().x;
            moveVertical = joystick.GetAxis().y;
        }

        if (!(transform.position.y + moveVertical * speed < mainController.upBound && transform.position.y + moveVertical * speed > mainController.downBound))
            moveVertical = 0;
        transform.Translate(moveHorizontal * speed, moveVertical * speed, 0);
        animator.SetFloat("hSpeed", moveHorizontal + moveVertical == 0 ? 0 : 1);
        Vector3 theScale = transform.localScale;
        theScale.x = moveHorizontal > 0 ? Mathf.Abs(this.transform.localScale.x) * -1 : moveHorizontal < 0 ? Mathf.Abs(this.transform.localScale.x) * 1 : theScale.x;
        transform.localScale = theScale;
        if (Input.GetButtonDown("Jump"))
            UseSkill(SkillType.Guitar);
        if (Input.GetKeyDown(KeyCode.H))
            UseSkill(SkillType.Hammer);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hummer"))
        {
            animator.SetBool("Hummer", false);
        }
    }

    private void RefreshSkills()
    {
        skills = new List<Skill>();
        foreach (var sk in mainController._base.skills)
            skills.Add(new Skill(sk.skillType, sk.powerValue[SaveManager.Instance.GetSkillLevel(sk.skillType)]));
    }

    public void UseSkill(SkillType usingSkillType)
    {
        switch (usingSkillType)
        {
            case SkillType.Curtain:
                animator.SetBool("Shower", !animator.GetBool("Shower"));
                /*if (body.activeInHierarchy)
                {
                    body.SetActive (false);
                    animator.SetBool("Shower", !animator.GetBool("Shower"));
                }
                else
                {
                    body.SetActive (true);
                    animator.SetBool("Shower", !animator.GetBool("Shower"));
                }*/
                break;
            case SkillType.Hammer:
                animator.SetBool("Hummer", true);
                EnemyController enemy = mainController.FindNearEnemy();  
                if (enemy != null)
                    enemy.GetHammerDamage((int)GetSkill(SkillType.Hammer).power);
                break;
            case SkillType.Guitar:
                animator.SetBool("GuitarPlaying", !animator.GetBool("GuitarPlaying"));
                foreach (var en in mainController.enemies)
                    en.GetGuitarDamage(GetSkill(SkillType.Guitar).power);
                break;
        }
    }

    private Skill GetSkill(SkillType skillType)
    {
        foreach (var sk in skills)
            if (sk.skillType == skillType)
                return sk;
        return new Skill();
    }

	public void GetDamage(float damage)
	{
		health -= damage;
		StartCoroutine(ShowDamage());
		UpdateHealthBar();
	}

	void UpdateHealthBar()
	{
		if (health <= 0)
		{
			StartCoroutine(Die());
			healthBar.transform.localScale = Vector3.zero;
		}
		else healthBar.transform.localScale = new Vector3(stratHealBarX * (health / 100f), healthBar.transform.localScale.y, 1);
	}

	IEnumerator ShowDamage()
	{
		for (int i = 0; i < 3; i ++)
		{
			yield return new WaitForSeconds(0.02f);	
			body.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f, 1);
			yield return new WaitForSeconds(0.02f);	
			body.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
		}
	}

	IEnumerator Die()
	{
		//TODO : ADD DEAD ANIMATION
		isDie = true;
		body.transform.Translate(0, -1f, 0);
		body.transform.Rotate(0, 0, -90);
		yield return new WaitForSeconds(0);
	}
}

[System.Serializable]
public class Skill
{
    public SkillType skillType;
    public float power;
    public Skill() { }
    public Skill(SkillType _skillType, float _power)
    {
        skillType = _skillType;
        power = _power;
    }
}