using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{

    public KeyCode attackKey = KeyCode.Mouse0;
    public KeyCode jumpKey = KeyCode.Space;
    public string xMoveAxis = "Horizontal";
    public string yMoveAxis = "Vertical";

    public float speed = 5f;
    public float jumpForce = 6f;
    public float groundLeeway = 0.1f;

    private Rigidbody2D rb2D = null;
    private SpriteRenderer sr = null;
    private float moveintentionX = 0;
    private float moveintentionY = 0;
    private bool attemptJump = false;
    private bool attemptAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Rigidbody2D>()){
            rb2D = GetComponent<Rigidbody2D>();
        }
        if (GetComponent<SpriteRenderer>()){
            sr = GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        //HandleJump();
        //HandleAttack();
    }

    void FixedUpdate(){
        HandleRun();
    }

    private void GetInput(){
        moveintentionX = Input.GetAxis(xMoveAxis);
        moveintentionY = Input.GetAxis(yMoveAxis);
        attemptAttack = Input.GetKeyDown(attackKey);
        attemptJump = Input.GetKeyDown(jumpKey);
    }

    private void HandleRun(){
        if (moveintentionX > 0 && sr.flipX == false){
            sr.flipX = true;
        }
        else if (moveintentionX < 0 && sr.flipX == true){
            sr.flipX = false;
        }    

        rb2D.velocity = new Vector2(moveintentionX * speed, moveintentionY * speed);
    }
}
