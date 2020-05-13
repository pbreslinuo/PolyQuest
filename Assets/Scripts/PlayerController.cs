using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float jumpPower;
    public float selfGravity;
    public float moveSpeed;
    public int numJumps;
    public float projectileSpeed;
    public int numProjectiles;
    public GameObject projectile;

    Vector3 m_Movement;
    CharacterController m_CharController;
    Transform m_Transform;
    Collider m_Collider;
    SpriteRenderer m_SpriteRenderer;
    Animator m_Animator;

    private int currentJumps;
    private int currentProjectiles;
    private float verticalSpeed = 0;
    private float horizSpeed = 0;
    private bool facingRight = true;
    private bool prevFacingRight = true;
    private bool headBonked;
    private bool sideBonked;

    void Start()
    {
        m_Transform = GetComponent<Transform>();
        m_CharController = GetComponent<CharacterController>();
        m_Collider = GetComponent<BoxCollider>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();
        currentJumps = numJumps;
    }

    void Update()
    {
        // reset if the r key is pressed
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // first, check grounded things: reset jumps, movement, reset bonked
        if (m_CharController.isGrounded)
        {
            m_Animator.SetBool("Grounded", true);
            m_Animator.SetBool("Jumping", false);
            HorizontalMovement();
            currentJumps = numJumps;
            verticalSpeed = -selfGravity * Time.deltaTime;
            if (Input.GetKeyDown("space"))
            {
                verticalSpeed = jumpPower;
                m_Animator.SetBool("Jumping", true);
            }
        }
        else // is in air
        {
            m_Animator.SetBool("Jumping", false);
            m_Animator.SetBool("Grounded", false);
            verticalSpeed -= selfGravity * Time.deltaTime;
            // put this in here so we can turn around midair- for sprite flipping and shooting
            if (Input.GetKey(KeyCode.A)) facingRight = false;
            else if (Input.GetKey(KeyCode.D)) facingRight = true;
            if (Input.GetKeyDown("space") && currentJumps > 0)
            {
                m_Animator.SetBool("Jumping", true);
                HorizontalMovement();
                verticalSpeed = jumpPower;
                currentJumps--;
            }
        }

        // shoot projectile code
        if (Input.GetKeyDown(KeyCode.RightControl) && currentProjectiles < numProjectiles)
        {
            // create an arrow
            GameObject Arrow = Instantiate(projectile, transform.position, transform.rotation);
            // set it's direction for both movement and sprite facing
            Vector3 fireDirection;
            if (facingRight)
            {
                fireDirection = Vector3.right;
                Arrow.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                fireDirection = Vector3.left;
                Arrow.GetComponent<SpriteRenderer>().flipX = false;
            }
            // launch it and update number of onscreen projectiles
            Arrow.GetComponent<Rigidbody>().velocity = fireDirection * projectileSpeed;
            currentProjectiles++;
            // then call coroutine which checks if its colliding less frequently than unity updates, but enough to be responsive
            StartCoroutine(ProjectileCoroutine(Arrow));
        }

        // finally, check our two types of 'bonks'
        // hit head on ceiling bonk
        if (((m_CharController.collisionFlags & CollisionFlags.Above) != 0)
            && !headBonked && verticalSpeed > 0)
        {
            //start going down again
            headBonked = true;
            verticalSpeed = -selfGravity * Time.deltaTime;
        }
        // hit side on wall bonk
        if (((m_CharController.collisionFlags & CollisionFlags.Sides) != 0)
            && !sideBonked && !m_CharController.isGrounded)
        {
            sideBonked = true;
            horizSpeed = 0;
        }

        // finally set and apply the movement to the player object
        m_Movement.Set(horizSpeed, verticalSpeed, 0f);
        m_CharController.Move(m_Movement * Time.deltaTime);

        // flip the gameObject so the texture faces the other way if we changed direction
        if ((facingRight && !prevFacingRight) || (!facingRight && prevFacingRight))
        {
            //some sort of character flip code goes here
            m_SpriteRenderer.flipX = !m_SpriteRenderer.flipX;
        }
        // and set current facing to prev facing for next iteration
        prevFacingRight = facingRight;
    }

    private void HorizontalMovement()
    {
        headBonked = false;
        sideBonked = false;
        if (Input.GetKey(KeyCode.A) ^ Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.A))
            {
                horizSpeed = -moveSpeed;
                facingRight = false;
            }
            else
            {
                horizSpeed = moveSpeed;
                facingRight = true;
            }
            m_Animator.SetBool("Walking", true);
        }
        else
        {
            horizSpeed = 0;
            m_Animator.SetBool("Walking", false);
        }
    }

    IEnumerator ProjectileCoroutine(GameObject thisObject)
    {
        // check 10 times a second
        yield return new WaitForSeconds(0.1f);
        // if we're colliding with something or went offstage, destroy this and lower num of onscreen projectiles
        if (thisObject.GetComponent<ProjectileController>().isColliding() || thisObject.transform.position.x > 20 || thisObject.transform.position.x < -20)
        {
            currentProjectiles--;
            Destroy(thisObject);
        }
        // otherwise call this coroutine again
        else StartCoroutine(ProjectileCoroutine(thisObject));
    }
}
