using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour, iDamageable
{
    public float healthPool = 5f;
    public float speed = 5f;
    
    private float currentHealth = 1f;

    private Transform healthBar;
    public float attackRadiusMelee = 1.5f;
    public float meleeDamage = 1f;
    public float attackDelay = 1.5f;
    private float timeUntilMeleeReady = 1.5f;
    public LayerMask playerLayer = 3;
    public GameObject dropHealthPrefab;  
    public float dropHealthChance = 90f;
  

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
        HandleAttack();
    }

    public virtual void ApplyDamage(float amount){
        currentHealth -= amount;
        //if (currentHealth <= 0){
            //Die();
        //}
    }

    private void Die(){
        gameObject.SetActive(false);
        int dropChance = Random.Range(1,100);
        if (dropChance > dropHealthChance){
            Instantiate(dropHealthPrefab, transform.position, Quaternion.identity);
        }
        Debug.Log(dropChance);
        Destroy(gameObject);
    }

    public void HandleAttack(){
        if (timeUntilMeleeReady <= 0){
            Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(transform.position, attackRadiusMelee, playerLayer);
                for (int i = 0; i < overlappedColliders.Length; i++){
                    iDamageable enemyAttributes = overlappedColliders[i].GetComponent<iDamageable>();
                    if (enemyAttributes != null){
                        enemyAttributes.ApplyDamage(meleeDamage);
                    }
                }
                timeUntilMeleeReady = attackDelay;
            }
        else {
            timeUntilMeleeReady -= Time.deltaTime;
        }
    }
}
