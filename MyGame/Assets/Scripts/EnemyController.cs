using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float moveSpeed;


    [Header("Преследование")]
    public bool shouldChasePlayer; //нужно ли преследовать
    public float rangeToChasePlayer;
    private Vector2 moveDirection;
    [Header("Побег")]
    // ДЛЯ ВОЗМОЖНОСТИ ДОБАВИТЬ ФУНКЦИЮ ПОБЕГА ОТ ВРАГА
    public bool shouldRunAway;
    public float runawayRange;
    [Header("Блуждание")]
    //ДЛЯ ТОГО ЧТОБЫ ВРАГ БЛУЖДАЛ рандомно
    public bool shouldWander;
    public float wanderLenght, pauseLenght;
    private float wanderCounter, pauseCounter;
    private Vector3 wanderDirection;
    [Header("Патруль")]
    // блуждание по маршруту
    public bool shouldPatrol;
    public Transform[] patrolPoints;
    public int currentPatrolPoint;


    
    [Header("Стрельба")]
    //Для стрельбы
    public bool shouldShoot;
    // сама пуля и точка откуда стреляем
    public GameObject bullet;
    public Transform firePoint;
    //для частоты выстрелова
    public float fireRate;
    private float fireCounter;

    public float shootRange;
    [Header("Анимация и хп")]
    public SpriteRenderer theBody; 
    public Animator anim;

    public int health = 150;
    // для эффектов урона и накаута
    public GameObject[] deathSplatters;
    public GameObject hitEffect;
    [Header("Звуковые Эффекты")]
    public int enemyHurtSFX;
    public int enemyDeadSFX;
    public int shootEnemySFX;
    [Header("Выпадение из врагов")]
    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;
    // Start is called before the first frame update
    void Start()
    {
        if(shouldWander)
        {
            pauseCounter = pauseCounter = Random.Range(pauseLenght * .75f, pauseLenght * 1.25f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (theBody.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            moveDirection = Vector2.zero;
            if ((Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer) && shouldChasePlayer)
            {

                moveDirection = PlayerController.instance.transform.position - transform.position;
            }
            else
            {
                if (shouldWander)
                {
                    if(wanderCounter > 0)
                    {
                        wanderCounter -= Time.deltaTime;

                        moveDirection = wanderDirection;
                        if (wanderCounter <= 0)
                        {
                            pauseCounter = Random.Range(pauseLenght * .75f, pauseLenght * 1.25f);
                        }
                    }
                    if (pauseCounter > 0)
                    {
                        pauseCounter -= Time.deltaTime;
                        if(pauseCounter <= 0)
                        {
                            wanderCounter = Random.Range(wanderLenght * .75f, wanderLenght * 1.25f);

                            wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                        }
                    }
                }
                if (shouldPatrol)
                {
                    moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;
                    if (Vector2.Distance(transform.position, patrolPoints[currentPatrolPoint].position)<.2f)
                    {
                        currentPatrolPoint++;
                        if (currentPatrolPoint >= patrolPoints.Length )
                        {
                            currentPatrolPoint = 0;
                        }
                        
                    }
                }
            }
            if(shouldRunAway&& (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < runawayRange))
            {
                moveDirection =  transform.position - PlayerController.instance.transform.position;
                
            }

            if (moveDirection.x > 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (moveDirection.x < 0)
            {
                transform.localScale = Vector3.one;
            }



            /*else
            {
                moveDirection = Vector2.zero;
            }*/

            theRB.velocity = moveDirection.normalized * moveSpeed;
            // условия выстрелов от врага и скорострельность
            if (shouldShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < shootRange)
            {
                fireCounter -= Time.deltaTime;
                if (fireCounter <= 0)
                {
                    fireCounter = fireRate;
                    Instantiate(bullet, firePoint.position, firePoint.rotation);
                    AudioManager.instance.PlaySFX(shootEnemySFX);
                }
            }
        }
        else
        {
            theRB.velocity = Vector2.zero;
        }

        if (moveDirection != Vector2.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }
    public void DamageEnemy(int damage,Transform kj)
    {
        AudioManager.instance.PlaySFX(enemyHurtSFX);
        health -= damage;

        Instantiate(hitEffect, transform.position,kj.rotation);
       
        if (health <= 0)
        {
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(enemyHurtSFX);
            int selectedDeath = Random.Range(0,deathSplatters.Length);
            Instantiate(deathSplatters[selectedDeath],transform.position,transform.rotation);

            if (shouldDropItem)
            {
                float dropChance = Random.Range(0f, 100f);
                if (dropChance <= itemDropPercent)
                {
                    int randomItem = Random.Range(0, itemsToDrop.Length);
                    Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
                }
            }
        }
    }
}
