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

	public bool vShtorke = false;
	//public bool usingSkill = false;

	private SkillType currentUsingSkill = SkillType.None;
	private Animator hummerAnimator;
	private Animator showerAnimator;

	public Transform leftBorder;
	public Transform rightBorder;

	public int killedEnemyCount = 0;

    private Joystick joystick;
    private EnemyController[] enemies;
    private MainController mainController;
    private Animator animator;
	private float stratHealBarX;

	public bool isDie = false;

	public Transform[] skillBars;
	public float[] skillsEnergy = {100, 100, 100};
	float skillBarScale;

	void CheckHintShow()
	{
		if (killedEnemyCount >= 3)
		{
			mainController.ShowNewHint();
			killedEnemyCount = 0;	
		}
	}

    void Start()
    {
		stratHealBarX = healthBar.transform.localScale.x;
        mainController = GameObject.FindObjectOfType<MainController>();

        joystick = GameObject.FindObjectOfType<Joystick>();
        animator = GetComponent<Animator>();
        RefreshSkills();
		skillBarScale = skillBars[0].localScale.x;
		hummerAnimator = transform.Find("Body").GetComponent<Animator>();
		showerAnimator = transform.Find("Shower").GetComponent<Animator>();
    }


	void UpdateSkillBars()
	{
		for (int i = 0; i < 3 ; i++)
			skillBars[i].localScale = new Vector3(skillBarScale * (skillsEnergy[i] / 100f), 0.4f, 0);
	}
	void SkillFill()
	{
		for (int i = 0; i < 3; i++)
			skillsEnergy[i] += skillsEnergy[i] < 100 ? Time.deltaTime * 4 : 0;
	}
	void SkillDown()
	{
		if (currentUsingSkill == SkillType.Curtain || currentUsingSkill == SkillType.Guitar)
		{
			skillsEnergy[(int)currentUsingSkill - 1] -= skillsEnergy[(int)currentUsingSkill - 1] > 0 ? Time.deltaTime * 15 : 0;
			if (skillsEnergy[(int)currentUsingSkill - 1] <= 0)
				UseSkill(currentUsingSkill);
		}
	}
	void FixedUpdate()
	{
		if (currentUsingSkill == SkillType.Guitar)
			GuitarDamage();
	}
    void Update()
    {
		if (mainController.gamePause)
			return;


		SkillFill();
		SkillDown();
		UpdateSkillBars();
		CheckHintShow();
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

		if (!(transform.position.x + moveHorizontal * speed < mainController.rightBound && transform.position.x + moveHorizontal * speed > mainController.leftBound))
			moveHorizontal = 0;
		
			transform.Translate (moveHorizontal * speed, moveVertical * speed, 0);

        animator.SetFloat("hSpeed", moveHorizontal + moveVertical == 0 ? 0 : 1);
        Vector3 theScale = transform.localScale;
        theScale.x = moveHorizontal > 0 ? Mathf.Abs(this.transform.localScale.x) * -1 : moveHorizontal < 0 ? Mathf.Abs(this.transform.localScale.x) * 1 : theScale.x;
        transform.localScale = theScale;
       
		if (Input.GetButtonDown("Jump"))
            UseSkill(SkillType.Guitar);
        if (Input.GetKeyDown(KeyCode.H))
            UseSkill(SkillType.Hammer);
		if (hummerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Active"))
        {
			hummerAnimator.SetBool("Hummer", false);
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
		if (currentUsingSkill != SkillType.None && usingSkillType != currentUsingSkill)
			return;
		if (currentUsingSkill == SkillType.None && skillsEnergy[(int)usingSkillType - 1] < 10)
			return;
        switch (usingSkillType)
        {
            case SkillType.Curtain:
				vShtorke = !vShtorke;
				showerAnimator.SetBool("Shower", !showerAnimator.GetBool("Shower"));
                break;
            case SkillType.Guitar:
                animator.SetBool("GuitarPlaying", !animator.GetBool("GuitarPlaying"));
                break;
		case SkillType.Hammer:
			if (currentUsingSkill == SkillType.Hammer)
				return;
			skillsEnergy[(int)SkillType.Hammer - 1] -= 10;
			hummerAnimator.SetBool("Hummer", true);
			EnemyController enemy = mainController.FindNearEnemy();  
			if (enemy != null)
				enemy.GetHammerDamage((int)GetSkill(SkillType.Hammer).power);
			StartCoroutine(ResetSkillTypeUsing(0.3f));
			break;
        }
		currentUsingSkill = currentUsingSkill == SkillType.None ? usingSkillType : SkillType.None;
    }

	IEnumerator ResetSkillTypeUsing(float delay)
	{
		yield return new WaitForSeconds(delay);
		currentUsingSkill = SkillType.None;
	}

	void GuitarDamage()
	{
		foreach (var en in mainController.enemies)
		{
			if (Mathf.Abs(en.transform.position.x - this.transform.position.x) < 5 && !en.isSleepping)
				en.GetGuitarDamage(GetSkill(SkillType.Guitar).power);
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
        hummerAnimator.SetBool("Dead", true);
        animator.SetBool("Dead", true);
        body.transform.Translate(0, -1f, 0);
		body.transform.Rotate(0, 0, -90);
		PopUpManager.Instance.OpenPage(PageType.GameOver);
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