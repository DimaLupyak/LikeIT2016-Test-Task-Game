using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class EnemyController : MonoBehaviour {

    //------------Stats--------------
	public float speed = 50;
    public float rangeAttack = 50;
	public float health = 100;
	public float level = 1;
	//-------------------------------

	public SpriteRenderer sprite;
	public GameObject player;
	public bool isFacingRight = true;

    private Rigidbody2D rigi;

    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
		player = GameObject.FindObjectOfType<PlayerController>().gameObject;
    }

    void FixedUpdate()
    {
        float dX = player.transform.position.x - transform.position.x;
        float dY = player.transform.position.y - transform.position.y;
        float d = Mathf.Sqrt(Mathf.Pow(dX, 2) + Mathf.Pow(dY, 2));
        if (d < rangeAttack && d > 2)
        {
            rigi.AddForce(new Vector2(
                Mathf.Sign(dX)* speed* (1-Mathf.Abs(dX)/ rangeAttack), 
                Mathf.Sign(dY) * speed/2 * (1-Mathf.Abs(dY) / rangeAttack)));
        }
        if (rigi.velocity.x != 0 || rigi.velocity.y != 0) rigi.AddForce(new Vector2(-rigi.velocity.x * 10, -rigi.velocity.y * 10));
        if (rigi.velocity.x > 0 && !isFacingRight)
            Flip();
        else if (rigi.velocity.x < 0 && isFacingRight)
            Flip();
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

    void OnTriggerEnter2D(Collider2D other)
    {
    }
}
