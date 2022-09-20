using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int moveSpeed = 10;
    [SerializeField] int jumpSpeed = 10;
    [SerializeField] float leanAngle = 5f;
    [SerializeField] Transform feet;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Animator myAnimator;
    private Rigidbody2D myRB;

    private bool movable = true;

    //Score 
    private PlayerScore playerScore;
    private int p_score = 0;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        playerScore = FindObjectOfType<PlayerScore>();
    }

    void Update()
    {
        if(movable) HandleMovement();
    }

    public bool isMovable
    {
        get
        {
            return movable;
        }
        set
        {
            movable = value;
        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        myRB.velocity = new Vector2(horizontal * moveSpeed, myRB.velocity.y);
        myRB.transform.rotation = Quaternion.Euler(myRB.transform.rotation.x, myRB.transform.rotation.y, -horizontal * leanAngle);

        if (Input.GetButtonDown("Jump") && touchGround())
        {
            myAnimator.SetBool("isJumping", true);
            myRB.velocity = new Vector2(myRB.velocity.x, jumpSpeed);
        }
        else if (touchGround()){
            myAnimator.SetBool("isJumping", false);
        }
    }

    public int Score
    {
        get
        {
            return p_score;
        }
        set{
            p_score = value;
        }
    }

    private bool touchGround()
    {
        return Physics2D.OverlapCircle(feet.position, 0.2f, groundLayer);
    }

}
