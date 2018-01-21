using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerObject : MonoBehaviour {

    public static ControllerObject instance = null;

    public Planet planet;
    public Astronaut astro;

    private Vector3 tilt;
    private Vector3 groundPos;

    private float jumpSpeed = 1f;
    private float jumpTimer = 0f;
    private float jumpTimerMax = 1f;
    private float fallSpeed = 1f;
    private float knockbackSpeed = 3f;
    private float knockbackDuration = .5f;

    public float oxygen;
    public float height;

    private bool isJumping;
    private bool isGrounded;
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
        oxygen = 100;
        rotationSpeed = .2f;
        groundPos = astro.transform.position;
	}
	
	void Update () {

        height = Mathf.Abs(groundPos.z - astro.gameObject.transform.position.z);

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
        else if (!isGrounded) //Means you're falling
        {
            
            astro.gameObject.transform.Translate(Vector3.back * -fallSpeed * Time.smoothDeltaTime);

        }

        if (astro.transform.position.z >= groundPos.z)
        {
            astro.gameObject.transform.position = groundPos;
            isGrounded = true;
            jumpTimer = 0f;
            jumpSpeed = 1f;
        }

        //tilt = new Vector3(-1f, Input.acceleration.y, 0f);
        tilt = new Vector3(-1f, Input.GetAxis("Horizontal"), 0f);

        if (astro.gameObject != null && !takingDamage)
        {
            planet.gameObject.transform.RotateAround(planet.gameObject.transform.position, tilt.normalized, rotationSpeed);

            astro.gameObject.transform.up = -tilt;
        }
        else if (takingDamage)
        {
            if (knockbackDuration > 0)
            {
                planet.gameObject.transform.RotateAround(planet.gameObject.transform.position, -tilt.normalized, knockbackSpeed);
                knockbackDuration -= Time.deltaTime;
            }
            else
            {
                takingDamage = false;
            }
            
            
        }
        else
        {
            //countdown, restart game
        }

        oxygen -= Time.deltaTime + (height) * 0.1f;
        //Debug.Log(oxygen);
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
}
