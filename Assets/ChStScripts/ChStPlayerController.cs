using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChStPlayerController : MonoBehaviour {

    private Rigidbody2D rb2d;
    private Animator anim;
    private int count;
    private bool isAlive;
    private float timer;
    private int wholetime;

    public float speed;
    public float jumpForce;
    public Text countText;
    public Text winText;
    public Text loseText;
    public AudioSource[] sounds;
    public AudioSource jumpSound;
    public AudioSource coinSound;
    public AudioSource loseSound;


    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        count = 0;
        isAlive = true;
        winText.text = "";
        SetCountText();
        loseText.text = "";
        sounds = GetComponents<AudioSource>();
        jumpSound = sounds[0];
        coinSound = sounds[1];
        loseSound = sounds[2];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetBool("ChStisWalking", true);
        }
        else
        {
            anim.SetBool("ChStisWalking", false);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            anim.SetTrigger("ChStJump");
        }

        transform.position = new Vector3
            (
                Mathf.Clamp(transform.position.x, -8.3f, 8.3f),
                transform.position.y, 
                transform.position.z
            );
    }

    void FixedUpdate()
    {
        if (isAlive)
        { 
            float moveHorizontal = Input.GetAxis("Horizontal");
            Vector2 movement = new Vector2(moveHorizontal, 0);
            rb2d.AddForce(movement * speed);
        }

        timer = timer + Time.deltaTime;
        if (timer >= 10)
        {
            loseText.text = "You Lose! :(";
            StartCoroutine(ByeAfterDelay(2));

        }
    }


    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "ChStGroundandRocks" && isAlive == true)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                jumpSound.Play();
            }
        }

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "ChStRiver")
        {
            anim.SetBool("ChStDrowning", true);
            isAlive = false;
            loseSound.Play();
            loseText.text = "You Lose! :(";
            StartCoroutine(ByeAfterDelay(2));
        }
    }

        void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ChStPickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 2;
            GameLoader.AddScore(2);
            SetCountText();
            coinSound.Play();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 10)
        {
            winText.text = "Yeah!";
            StartCoroutine(ByeAfterDelay(2));
        }
    }

    IEnumerator ByeAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        GameLoader.gameOn = false;
    }
}
