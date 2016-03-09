using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class EnemyController : MonoBehaviour 
{

    //------------Stats--------------
	public float speed = 50;
    public float attackRange = 50;
	public float maxHealth = 100;
	public float health = 100;
	public float level = 1;
	//-------------------------------

	public SpriteRenderer sprite, healthBar;
	public GameObject character;
	public bool isFacingRight = true;

	private GameObject player;
	private MainController mainController;
    private Animator animator;
	private bool blockMove;

    void Start()
    {
		animator = character.GetComponent<Animator>();
		player = GameObject.FindObjectOfType<PlayerController>().gameObject;
		mainController = GameObject.FindObjectOfType<MainController>();
		speed += Random.Range(0, 200) / 10000f;
    }

    void Update()
    {
		if (blockMove)
		{
			animator.SetFloat("hSpeed", 0);
			return;
		}
		float distance = Mathf.Sqrt(Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.y - transform.position.y, 2));
		if (distance < attackRange && distance > 2)
		{
			transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);
			animator.SetFloat("hSpeed", 1);
		}
		else animator.SetFloat("hSpeed", 0);
		FlipCheck();
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
		yield return new WaitForSeconds(delay);
		blockMove = false;
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
