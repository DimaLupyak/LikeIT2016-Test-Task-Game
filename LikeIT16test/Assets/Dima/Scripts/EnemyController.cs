using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class EnemyController : MonoBehaviour 
{

    //------------Stats--------------
	public float speed = 50;
    public float attackRange = 50;
	public float health = 100;
	public float level = 1;
	//-------------------------------

	public SpriteRenderer sprite;
	public GameObject player;
	public bool isFacingRight = true;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
		player = GameObject.FindObjectOfType<PlayerController>().gameObject;
    }

    void Update()
    {
		float distance = Mathf.Sqrt(Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.y - transform.position.y, 2));
		if (distance < attackRange && distance > 2)
		{
			transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);
			animator.SetFloat("hSpeed", 1);
		}
		else animator.SetFloat("hSpeed", 0);

		Vector3 theScale = transform.localScale;
		theScale.x = player.transform.position.x > this.transform.position.x ? Mathf.Abs(this.transform.localScale.x) * -1 : Mathf.Abs(this.transform.localScale.x) * 1;
		transform.localScale = theScale;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

	public void GetDamage(int damage)
	{
		health -= damage;
		StartCoroutine(ShowDamage());
	}

	IEnumerator ShowDamage()
	{
		for (int i = 0; i < 5; i ++)
		{
			yield return new WaitForSeconds(0.05f);	
			sprite.color = new Color(1, 0.5f, 0.5f, 1);
			yield return new WaitForSeconds(0.05f);	
			sprite.color = new Color(1, 1, 1, 1);
		}
	}
}
