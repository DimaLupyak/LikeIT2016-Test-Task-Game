using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using Holoville.HOTween;

public enum EnemyType {Pantera, Bat}
public class EnemyController : MonoBehaviour 
{
	public EnemyType enemyType;

    //------------Stats--------------
	private float speed = 0.02f;
	private float attackRange = 20;
	private float health = 100;
	public float power = 10;
	//-------------------------------

	const float maxHealth = 100;

	public SpriteRenderer sprite, healthBar;
	public GameObject character;
	public bool isFacingRight = true;

	private PlayerController player;
	private MainController mainController;
    private Animator animator;
	private bool blockMove;
    private float minDistance, hitDelta, distance, hitTimer;

    void Start()
    {
		animator = character.GetComponent<Animator>();
		player = GameObject.FindObjectOfType<PlayerController>();
		mainController = GameObject.FindObjectOfType<MainController>();
		speed += Random.Range(0, 200) / 10000f;
        minDistance = 1 + Random.Range(0, 200) / 100f;
		hitDelta = 1 + Random.Range(0, 200) / 100f;
    }

    void Update()
    {
		if (blockMove)
		{
			if (enemyType == EnemyType.Pantera)
				animator.SetFloat("hSpeed", 0);
			return;
		}
		distance = enemyType == EnemyType.Bat ? Mathf.Abs(player.transform.position.x - transform.position.x) : Mathf.Sqrt(Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.y - transform.position.y, 2));
		if (distance < attackRange && distance > minDistance && !player.vShtorke)
		{
			var destination = enemyType == EnemyType.Bat ? new Vector3(player.transform.position.x, this.transform.position.y, this.transform.position.z) : player.transform.position;
			transform.position = Vector3.MoveTowards(transform.position,  destination, speed);
			if (enemyType == EnemyType.Pantera)
				animator.SetFloat("hSpeed", 1);
		}
		else if (enemyType == EnemyType.Pantera) 
			animator.SetFloat("hSpeed", 0);
		if (distance < minDistance && !player.isDie && !player.vShtorke)
			CheckHit();
		FlipCheck();
    }

	void CheckHit()
	{
		hitTimer += Time.deltaTime;
		if (hitTimer >= hitDelta)
			Hit();
	}

	IEnumerator BatHit()
	{
		blockMove = true;
		var startsPos = this.transform.position;
		HOTween.To(this.transform, 0.3f, new TweenParms().Prop("position", player.transform.position).Ease(EaseType.EaseInBack));
		yield return new WaitForSeconds(0.4f);
		if (!player.isDie)
			player.GetDamage(power);
		HOTween.To(this.transform, 0.3f, new TweenParms().Prop("position", startsPos).Ease(EaseType.EaseOutBack));
		yield return new WaitForSeconds(0.4f);
		blockMove = false;
	}
	private void Hit()
	{
		hitTimer = 0;
		if (enemyType == EnemyType.Bat)
			StartCoroutine(BatHit());
	}

    private void FlipCheck()
    {
		Vector3 theScale = character.transform.localScale;
		theScale.x = player.transform.position.x > this.transform.position.x ? Mathf.Abs(character.transform.localScale.x) * 1 : Mathf.Abs(character.transform.localScale.x) * -1;
		character.transform.localScale = theScale;
    }

	public void GetHammerDamage(int damage)
	{
		health -= damage;
		StartCoroutine(ShowDamage());
		UpdateHealthBar();

	}
	void UpdateHealthBar()
	{
		if (health <= 0)
			StartCoroutine(Die());
		else healthBar.transform.localScale = new Vector3(2 * (health / maxHealth), healthBar.transform.localScale.y, 1);
	}

	public void GetGuitarDamage(float delay)
	{
		//TODO : SLEEP ANIMATION
		StartCoroutine(Sleep(delay));
	}

	IEnumerator Sleep(float delay)
	{
		blockMove = true;
        animator.SetBool("Sleep", true);
        yield return new WaitForSeconds(delay);
		blockMove = false;
        animator.SetBool("Sleep", false);
    }

	IEnumerator ShowDamage()
	{
		for (int i = 0; i < 3; i ++)
		{
			yield return new WaitForSeconds(0.02f);	
			sprite.color = new Color(1, 0.5f, 0.5f, 1);
			yield return new WaitForSeconds(0.02f);	
			sprite.color = new Color(1, 1, 1, 1);
		}
	}

	IEnumerator Die()
	{
		mainController.enemies.Remove(this);
		healthBar.enabled = false;
		blockMove = true;
		character.transform.Rotate(0, 0, 90f * Mathf.Sign(character.transform.localScale.x));
		yield return new WaitForSeconds(3);
		while (character.transform.localScale.y > 0.1f)
		{
			character.transform.localScale *= 0.9f;
			yield return new WaitForSeconds(0.01f);
		}
		Destroy(this.gameObject);
	}
}
