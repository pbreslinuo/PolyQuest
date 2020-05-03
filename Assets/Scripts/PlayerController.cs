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

    Vector3 m_Movement;
    CharacterController m_CharController;
    Transform m_Transform;
    Collider m_Collider;
    Renderer m_Renderer;

    private int currentJumps;
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
        m_Renderer = GetComponent<Renderer>();
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
            HorizontalMovement();
            currentJumps = numJumps;
            verticalSpeed = -selfGravity * Time.deltaTime;
            if (Input.GetKeyDown("space"))
            {
                verticalSpeed = jumpPower;
            }
        }
        else // is in air
        {
            verticalSpeed -= selfGravity * Time.deltaTime;
            if (Input.GetKeyDown("space") && currentJumps > 0)
            {
                HorizontalMovement();
                verticalSpeed = jumpPower;
                currentJumps--;
            }
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
        }
        else
        {
            horizSpeed = 0;
        }
    }
}
