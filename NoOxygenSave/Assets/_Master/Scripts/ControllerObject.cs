using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerObject : MonoBehaviour {

    public static ControllerObject instance = null;

    public Planet planet;
    public Astronaut astro;

    private Vector3 tilt;
    private Vector3 groundPos;

    private float jumpSpeed;
    private float jumpSpeedMax = 2f;
    private float jumpTimer = 0f;
    private float jumpTimerMax = 1f;
    private float fallSpeed = 2f;
    private float knockbackSpeed;
    private float knockbackSpeedMax = 1.7f;
    private float knockbackDuration;
    private const float knockbackDurationMax = .3f;
    private float bounceTimer = 0f;
    private float bounceTimerMax = .5f;
    private float bounceSpeed = 6f;


    public float oxygen;
    public float height;

    private bool isJumping;
    private bool isGrounded;
    private bool isBouncing;
    private bool takingDamage;


    private float rotationSpeed;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }
          
        DontDestroyOnLoad(gameObject);
    }

    void Start () {
        knockbackDuration = knockbackDurationMax;
        oxygen = 200;
        rotationSpeed = .2f;
        groundPos = astro.transform.position;
        takingDamage = false;
        isBouncing = false;
        jumpSpeed = jumpSpeedMax;
        knockbackSpeed = knockbackSpeedMax;

    }
	
	void Update () {

        height = (jumpTimer * 5) - 1;

        if (isBouncing)
        {
            astro.gameObject.transform.Translate(Vector3.back * bounceSpeed * Time.smoothDeltaTime);
            bounceSpeed -= bounceSpeed / 20;

            bounceTimer += Time.deltaTime;
            if (bounceTimer > bounceTimerMax)
            {
                bounceTimer = 0;
                isBouncing = false;
            }
        }

        if (Input.touchCount >= 1 || Input.GetKey(KeyCode.Space))
        {
            if (!isJumping && isGrounded)
            {
                isJumping = true;
                isGrounded = false;
            }

        }
        else
        {
            isJumping = false;            
        }
        
        if (isJumping)
        {
            if(jumpTimer < jumpTimerMax)
            {
                astro.gameObject.transform.Translate(Vector3.back * jumpSpeed * Time.smoothDeltaTime);
                jumpSpeed -= jumpSpeed / 60;
                jumpTimer += Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        else if (!isGrounded && !isBouncing) //Means you're falling
        {
            
            astro.gameObject.transform.Translate(Vector3.back * -fallSpeed * Time.smoothDeltaTime);

        }

        if (astro.transform.position.z >= groundPos.z)
        {
            astro.gameObject.transform.position = groundPos;
            isGrounded = true;
            jumpTimer = 0f;
            jumpSpeed = jumpSpeedMax;
        }

        //tilt = new Vector3(-1f, Input.acceleration.y, 0f);
        tilt = new Vector3(-1f, Input.GetAxis("Horizontal"), 0f);

        if (astro.gameObject != null && !takingDamage && !isBouncing)
        {
            planet.gameObject.transform.RotateAround(planet.gameObject.transform.position, tilt.normalized, rotationSpeed);

            astro.gameObject.transform.up = -tilt;
        }
        else if (takingDamage)
        {
            takingDamage = Knockback(knockbackSpeed);
        }
        else if (isBouncing)
        {
            isBouncing = Knockback(knockbackSpeed / 2f);
        }
        else
        {
            //countdown, restart game
        }

        oxygen -= Time.deltaTime;
        if(isBouncing)
            oxygen -= (height) * 0.05f;

        Debug.Log(oxygen);
	}


    public void DealDamage(float m_damage)
    {
        oxygen -= m_damage;
        takingDamage = true;
    }

    public void Refill(float m_refillOxygen)
    {
        oxygen += m_refillOxygen;
    }

    public void Bounce()
    {
        isBouncing = true;
    }

    public bool Knockback(float m_knockbackSpeed)
    {
        if (knockbackDuration > 0)
        {
            planet.gameObject.transform.RotateAround(planet.gameObject.transform.position, -tilt.normalized, m_knockbackSpeed);
            knockbackDuration -= Time.deltaTime;
            knockbackSpeed -= knockbackSpeed / 20;
            return true;
        }
        else
        {
            knockbackDuration = knockbackDurationMax;
            knockbackSpeed = knockbackSpeedMax;
            return false;
        }
    }
}
