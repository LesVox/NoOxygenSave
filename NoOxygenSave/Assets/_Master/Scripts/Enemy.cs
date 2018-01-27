using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float health = 10;
    public float armor = 1;
    public float oxygen = 20;
    public float knockbackTimer;
    public const float knockbackTimerMax = 1f;
    public float knockbackSpeed;
    public const float knockbackSpeedMax = 3f;

    public bool knockback = false;

    private GameObject planet;
    private GameObject astro;

    private void Awake()
    {
        planet = FindObjectOfType<Planet>().gameObject;
    }

    void Start () {
        knockbackTimer = knockbackTimerMax;
        knockbackSpeed = knockbackSpeedMax;

    }
	
	void Update () {
		if(health <= 0)
        {
            Destroy(this.gameObject, .5f);
            this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = !this.gameObject.GetComponentInChildren<MeshRenderer>().isVisible;
            //ControllerObject.instance.Refill(oxygen);
            
        }

        if(knockback && knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
            this.gameObject.transform.RotateAround(planet.gameObject.transform.position, -(ControllerObject.instance.Tilt).normalized, knockbackSpeed);
            knockbackSpeed -= knockbackSpeed / 10;
        }
        else if(knockback && knockbackTimer < 0)
        {
            knockback = false;
            knockbackTimer = knockbackTimerMax;
            knockbackSpeed = knockbackSpeedMax;
        }

	}
    
    public void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.GetComponent<Astronaut>() != null)
        {
            astro = col.gameObject;
            if(ControllerObject.instance.height > armor)
            {
                TakeDamage();
                ControllerObject.instance.Bounce();
            }
            else
            {
                ControllerObject.instance.DealDamage(10);
            }
        }
    }

    private void TakeDamage()
    {
        health -= 5;
        ControllerObject.instance.Refill(oxygen);
        knockback = true;
    }

    private void OnDestroy()
    {
        ControllerObject.instance.SpawnEnemy();
    }
}
