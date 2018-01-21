using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float health = 10;
    public float armor = 1;
    public float oxygen = 20;


    void Start () {
        
    }
	
	void Update () {
		if(health <= 0)
        {
            Destroy(this.gameObject, .5f);
        }
	}
    
    public void OnCollisionEnter(Collider col)
    {
        Debug.Log("LOL");
        if(col.gameObject.GetComponent<Astronaut>() != null)
        {
            if(ControllerObject.instance.height > armor)
            {
                TakeDamage();
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
    }
}
