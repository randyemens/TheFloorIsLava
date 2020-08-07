/***********************************************************************************
 * TFIS: Player Movement
 * Created by Randel Emens
 * Description: Handles player functionality
 **********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    private Animator animator;
    private bool refreshJump = true;
    private float jumpTimer = .5f;
    public LayerMask Ground;
    public BoxCollider2D groundCheck;
    public BoxCollider2D lavaCheck;
    public LayerMask Lava;
    float horizontalMove;
    float startHeight;
    private GameController controllerScript;
    private CameraShake cameraScript;
    public GameObject deathAnimation;

    void Start()
    {
        //Initialize control components
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        animator = GetComponent<Animator>();
        startHeight = this.transform.position.y;
        GameObject GameController = GameObject.Find("GameController");
        controllerScript = (GameController)GameController.GetComponent(typeof(GameController));
        cameraScript = (CameraShake)Camera.main.gameObject.GetComponent(typeof(CameraShake));
    }


    //NEEDS REFACTORING: Separate movement and animation functionality

    void Update()
    {
        Move();

        //Create uniform vertical moment
        Vector3 targetVelocity = new Vector2(horizontalMove, m_Rigidbody2D.velocity.y);
        if (m_Rigidbody2D.velocity.y < -5.5f)
            targetVelocity = new Vector2(horizontalMove, -5.5f);
        else if (m_Rigidbody2D.velocity.y < -1f && m_Rigidbody2D.velocity.y > -5.5f)
            targetVelocity = new Vector2(horizontalMove, -5.5f);
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, .05f);

        //Update animator
        animator.SetFloat("Vertical", m_Rigidbody2D.velocity.y);
        animator.SetBool("Grounded", Physics2D.IsTouchingLayers(groundCheck, Ground));

        //Handle New Jump
        if (jumpTimer > 0f && Input.GetKey("space"))
        {
            jumpTimer -= Time.deltaTime;
            m_Rigidbody2D.velocity = new Vector3(m_Rigidbody2D.velocity.x, 5.5f, 0f);
            animator.SetBool("Jumping", true);
            if (this.transform.position.y - startHeight > 1.22f) StopJump();
        }
        //Cancel jump if conditions met
        else if ((Input.GetKeyUp("space") && jumpTimer > 0f) || jumpTimer < 0f)
            StopJump();

        //Stabilize variable height jump so player doesn't drift upwards
        if (Input.GetKeyDown("space"))
            refreshJump = false;

        if (Input.GetKeyUp("space"))
            refreshJump = true;

        //Refresh jump and set animator to grounded
        if (Physics2D.IsTouchingLayers(groundCheck, Ground))
        {
            if (refreshJump)
                jumpTimer = .5f;
            animator.SetBool("Jumping", false);
            startHeight = this.transform.position.y;
        }

        //Check for death
        if (Physics2D.IsTouchingLayers(lavaCheck, Lava))
        {
            Instantiate(deathAnimation, new Vector3(transform.position.x, transform.position.y, 0), new Quaternion(0f, 0f, 0f, 0f));
            controllerScript.EndGame(false);
            Destroy(this.gameObject);
        }

        //Set jump timer to 0 so player cannot continue to jump in air
        if (m_Rigidbody2D.velocity.y < -0.01f && !Physics2D.IsTouchingLayers(groundCheck, Ground))
            jumpTimer = 0f;

        //Cause slight camera shake for jumps
        if (Physics2D.IsTouchingLayers(groundCheck, Ground) && jumpTimer > 0f && Input.GetKeyDown("space"))
            cameraScript.shakeDuration = .2f;
    }

    private void Move()
    {
        if (controllerScript.paused) return;

        //Handle Left/Right Movement
        if (Input.GetKey(KeyCode.LeftArrow)) horizontalMove = -4f;
        else if (Input.GetKey(KeyCode.RightArrow)) horizontalMove = 4f;
        else horizontalMove = 0f;

        if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            transform.eulerAngles = new Vector3(0, 180.0f, 0);
        else if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            transform.eulerAngles = new Vector3(0, 0.0f, 0);

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
            animator.SetBool("Move", true);
        else
            animator.SetBool("Move", false);
    }

    //Cancel upward momentum of jump and update animator
    public void StopJump()
    {
        m_Rigidbody2D.velocity = new Vector3(m_Rigidbody2D.velocity.x, 1f, 0f);
        jumpTimer = 0f;
        animator.SetBool("Jumping", false);
    }
}
