using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour, iDamageable
{

    public KeyCode attackKey = KeyCode.Mouse0;
    public KeyCode jumpKey = KeyCode.Space;
    public string xMoveAxis = "Horizontal";
    public string yMoveAxis = "Vertical";

    public float speed = 5f;
    public float jumpForce = 6f;
    public float groundLeeway = 0.1f;

    private Rigidbody2D rb2D = null;
    private SpriteRenderer sr = null;
    private float moveintentionX = 0;
    private float moveintentionY = 0;
    private bool attemptAttack = false;

    public Transform attackOriginMelee = null;
    public float attackRadiusMelee = 0.6f;
    public float meleeDamage = 1f;
    public float attackDelay = 1.1f;
    public LayerMask enemyLayer = 8;

    private float timeUntilMeleeReady = 0;

    private float currentHealth = 10f;

    private Transform healthBar;
    public float healthPool = 20f;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = healthPool;
        healthBar = transform.Find("HealthBar/bar");
        if (GetComponent<Rigidbody2D>()){
            rb2D = GetComponent<Rigidbody2D>();
        }
        if (GetComponent<SpriteRenderer>()){
            sr = GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(healthBar.localScale.x > currentHealth/healthPool){
            healthBar.localScale = new Vector2(healthBar.localScale.x - (.002f), 1f);
        }
        if(healthBar.localScale.x < currentHealth/healthPool){
            healthBar.localScale = new Vector2(healthBar.localScale.x + (.002f), 1f);
        }
        if (healthBar.localScale.x <= 0){
            Die();
        }
        GetInput();
        HandleAttack();
    }

    void FixedUpdate(){
        HandleRun();
    }

    void OnDrawGizmosSelected(){
Gizmos.DrawWireSphere(attackOriginMelee.position, attackRadiusMelee);
}


    void OnTriggerEnter2D(Collider2D other) {
        if (other.name.Contains("Healer")) {
            if(currentHealth < healthPool){            
                currentHealth += 2;
                other.gameObject.GetComponent<ParticleSystem>().Play();
                StartCoroutine(destroyDelay(2f, other));
                other.isTrigger = false;
                if (currentHealth > healthPool){currentHealth = healthPool;}}
        }
    }

    public virtual void ApplyDamage(float amount){
        currentHealth -= amount;
        //if (currentHealth <= 0){
            //Die();
        //}
    }

    private void GetInput(){
        moveintentionX = Input.GetAxis(xMoveAxis);
        moveintentionY = Input.GetAxis(yMoveAxis);
        attemptAttack = Input.GetKeyDown(attackKey);
    }

    private void HandleRun(){
        if (moveintentionX > 0 && sr.flipX == false){
            sr.flipX = true;
            attackOriginMelee.localPosition = new Vector2(.5f, 0);
        }
        else if (moveintentionX < 0 && sr.flipX == true){
            sr.flipX = false;
            attackOriginMelee.localPosition = new Vector2(-.5f, 0);
        }    

        if(moveintentionX < 0 && moveintentionY < 0){
            attackOriginMelee.localPosition = new Vector2(-.5f, -.75f);
        } else if(moveintentionX < 0 && moveintentionY == 0){
            attackOriginMelee.localPosition = new Vector2(-.5f, 0.0f);
        } else if(moveintentionX < 0 && moveintentionY > 0){
            attackOriginMelee.localPosition = new Vector2(-.5f, .75f);
        } else if(moveintentionX > 0 && moveintentionY < 0){
            attackOriginMelee.localPosition = new Vector2(.5f, -.75f);
        } else if(moveintentionX > 0 && moveintentionY == 0){
            attackOriginMelee.localPosition = new Vector2(.5f, 0.0f);
        } else if(moveintentionX > 0 && moveintentionY > 0){
            attackOriginMelee.localPosition = new Vector2(.5f, .75f);
        } else if(moveintentionX == 0 && moveintentionY > 0){
            attackOriginMelee.localPosition = new Vector2(0.0f, .75f);
        } else if(moveintentionX == 0 && moveintentionY < 0){
            attackOriginMelee.localPosition = new Vector2(0.0f, -.75f);
        }

        rb2D.velocity = new Vector2(moveintentionX * speed, moveintentionY * speed);
    }

    private void Die(){
        transform.position = Vector2.zero;
        currentHealth = healthPool;
        healthBar.localScale = new Vector2(1f, 1f);
    }

    private void HandleAttack(){
        if (timeUntilMeleeReady <= 0){
            if(attemptAttack){
                Debug.Log("PlayerAttemptingAttack");
                Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(attackOriginMelee.position, attackRadiusMelee, enemyLayer);
                for (int i = 0; i < overlappedColliders.Length; i++){
                    iDamageable enemyAttributes = overlappedColliders[i].GetComponent<iDamageable>();
                    if (enemyAttributes != null){
                        enemyAttributes.ApplyDamage(meleeDamage);
                    }
                }
                timeUntilMeleeReady = attackDelay;
            }
        }
        else {
            timeUntilMeleeReady -= Time.deltaTime;
        }
    }

    public IEnumerator destroyDelay(float t, Collider2D c2D){
        yield return new WaitForSeconds(t);
        Destroy(c2D.gameObject);
    }

}
