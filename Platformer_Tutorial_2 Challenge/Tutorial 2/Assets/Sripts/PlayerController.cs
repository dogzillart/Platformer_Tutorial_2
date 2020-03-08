using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rd2d;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    public float speed;
    private int Count;
    public Text winText;
    public Text countText;
    public Text livesText;
    public Text loseText;
    public GameObject UFO;
    private string EnemyUFO;
    private int Lives;
    public AudioSource musicSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    private bool facingRight = true;
    Animator anim;



 //Start is called before the first frame update
void Start()
{
    rd2d = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    musicSource.clip = musicClipOne;
    musicSource.Play();
    musicSource.loop = true;
    Count = 0;
    Lives = 3;
    winText.text = "";
    loseText.text = "";
    SetCountText();
    SetLivesText();
    isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
}



     //Update is called once per frame
    void FixedUpdate()
 {
        {
            float hozMovement = Input.GetAxis("Horizontal");
            float vertMovement = Input.GetAxis("Vertical");
            rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
            if (facingRight == false && hozMovement > 0)
            {
                Flip();
            }
            else if (facingRight == true && hozMovement < 0)
            {
                Flip();
            }
        }
    
        if (Lives <= 0)
    {
        rd2d.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    if (Count >= 8 && Lives >= 1)
    {
        rd2d.constraints = RigidbodyConstraints2D.FreezeAll;
        UFO.GetComponent<EnemyUFO>().enabled = false;
        }

    if (Input.GetButton("Horizontal"))
    {
        anim.SetInteger("State", 1);
    }
    if (Input.GetButtonUp("Horizontal"))
    {
        anim.SetInteger("State", 0);
    }
    if (Input.GetButton("Vertical"))
    {
        anim.SetInteger("State", 2);
    }
    if (Input.GetButtonUp("Vertical"))
    {
        anim.SetInteger("State", 0);
    }
 }

private void OnTriggerEnter2D(Collider2D other)
{
    if (other.gameObject.CompareTag("Coin"))
    {
        Count = Count += 1;
        SetCountText();
        other.gameObject.SetActive(false);
            
            if (other.gameObject.CompareTag("Coin") && Count == 4)
            {
                transform.position = new Vector2(235.71f, -0.8f);
                Lives = 3;
                SetLivesText();
            }
    }
    
        if (other.gameObject.CompareTag("Enemy"))
        {
           Lives = Lives - 1;
           SetLivesText();
           other.gameObject.SetActive(false);
        }
   
}

void SetCountText()
{
    countText.text = "Count: " + Count.ToString();

    if (Count >= 8 && Lives >= 1)
    {
        musicSource.Stop();
        musicSource.clip = musicClipTwo;
        musicSource.Play();
        winText.text = "You win! Game created by Kyle London";
        anim.enabled = false;
    }

}

void SetLivesText()
{
    livesText.text = "Lives: " + Lives.ToString();

    if (Lives <= 0)
    {
        loseText.text = "You have died. (hit Esc and reopen to try again)";
        musicSource.Stop();
        anim.enabled = false;
        }
}
    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
       

    private void OnCollisionStay2D(Collision2D collision)
    {
    if (collision.collider.tag == "Ground" && isOnGround)
    {
        if (Input.GetButton("Vertical"))
        {
            rd2d.AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
            anim.SetInteger("State", 2);
                     
        }
    }
  }
}

