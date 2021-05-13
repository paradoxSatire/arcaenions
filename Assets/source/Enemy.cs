using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, iDamageable
{
    public float healthPool = 10f;
    public float speed = 5f;
    
    private float currentHealth = 10f;

    private Transform healthBar;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = healthPool;
        healthBar = transform.Find("HealthBar/bar");
        Debug.Log(healthBar);
    }

    // Update is called once per frame
    void Update()
    {
        if(healthBar.localScale.x > currentHealth/healthPool){
            healthBar.localScale = new Vector2(healthBar.localScale.x - (.005f), 1f);
        }
        if (healthBar.localScale.x <= 0){
            Die();
        }
    }

    public virtual void ApplyDamage(float amount){
        currentHealth -= amount;
        //if (currentHealth <= 0){
            //Die();
        //}
    }

    private void Die(){
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    
}
