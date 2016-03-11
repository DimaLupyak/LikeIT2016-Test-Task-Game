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

    private Joystick joystick;
    public GameObject body;
    private EnemyController[] enemies;
    private MainController mainController;
    private Animator animator;
    private Animator hummerAnimator;
    private Animator showerAnimator;

    void Start()
    {
        mainController = GameObject.FindObjectOfType<MainController>();
        joystick = GameObject.FindObjectOfType<Joystick>();
        animator = GetComponent<Animator>();
        hummerAnimator = transform.Find("Body").GetComponent<Animator>();
        showerAnimator = transform.Find("Shower").GetComponent<Animator>();
        RefreshSkills();
    }

    void Update()
    {
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
        switch (usingSkillType)
        {
            case SkillType.Curtain:
                showerAnimator.SetBool("Shower", !showerAnimator.GetBool("Shower"));
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
                hummerAnimator.SetBool("Hummer", true);
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