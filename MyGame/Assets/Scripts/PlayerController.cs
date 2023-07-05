using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;


    public float moveSpeed;

    private Vector2 moveInput;

    public Rigidbody2D theRB;

    public Transform gunArm;

    public Animator anim;

 /*   public GameObject bulletToFire; //ссылка на пулю
    public Transform firePoint; // Точка выстрела

    public float timeBetweenShots; //Частота выстрелов при нажатии
    private float shotCounter;*/

    public SpriteRenderer bodySR;
    //Для рывка 
    public float activeMoveSpeed;
    public float dashSpeed = 8f, dashLength = .5f, dashCooldown = 1f, dashInvincibility = .5f;
    [HideInInspector]
    public float dashCounter;
    private float dashCoolCounter;
    public int shootSFX;
    [HideInInspector]
    public bool canMove = true;

    public List<Gun> availableGuns = new List<Gun>();
    [HideInInspector]
    public int currentGun;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
      //  theCam = Camera.main;//оптимизация
        activeMoveSpeed = moveSpeed;
        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = availableGuns[currentGun].weaponName;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !LevelManager.instance.isPaused)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize(); // чтобы скорость передвежения по диагонали была такой же

            //transform.position += new Vector3(moveInput.x*Time.deltaTime*moveSpeed, moveInput.y*Time.deltaTime*moveSpeed, 0f);
            theRB.velocity = moveInput * activeMoveSpeed; // движение игрока через RigidBody



            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition); //оптимизирован
                                                                                      //поворот игрока
            if (mousePos.x < screenPoint.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                gunArm.localScale = new Vector3(-1f, -1f, 1f);
            }
            else
            {
                transform.localScale = Vector3.one;
                gunArm.localScale = Vector3.one;
            }



            //Rotate gun arm
            Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            gunArm.rotation = Quaternion.Euler(0, 0, angle);// quaternion для получение прямого доступа к вращению

            //Реализация выстрела
         /*   if (Input.GetMouseButtonDown(0))
            {
                AudioManager.instance.PlaySFX(shootSFX);
                Instantiate(bulletToFire, firePoint.position, firePoint.rotation); //Создание копии префаба пули
                shotCounter = timeBetweenShots;
            }
            //Частота выстрелов при нажатии
            if (Input.GetMouseButton(0))
            {
                shotCounter -= Time.deltaTime;
                if (shotCounter <= 0)
                {
                    AudioManager.instance.PlaySFX(shootSFX);
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation);

                    shotCounter = timeBetweenShots;
                }
            }*/
            //Дешинг 
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (availableGuns.Count > 0 )
                {
                    currentGun++;
                    if(currentGun >= availableGuns.Count)
                    {
                        currentGun = 0;
                    }
                    SwithGun();
                }
                else
                {
                    Debug.LogError("Player has no guns");
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;
                    anim.SetTrigger("dash");
                    PlayerHealthController.instance.MakeInvincible(dashInvincibility);
                    AudioManager.instance.PlaySFX(8);//Звук Деша
                }
            }
            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0)
                {
                    activeMoveSpeed = moveSpeed;
                    dashCoolCounter = dashCooldown;
                }
            }
            if (dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }


            if (moveInput != Vector2.zero)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }
        else
        {
            theRB.velocity = Vector2.zero;
            anim.SetBool("isMoving", false);
        }
    }
    public void SwithGun()
    {
        foreach (Gun theGun in availableGuns) { 
         theGun.gameObject.SetActive(false);
        }
        availableGuns[currentGun].gameObject.SetActive(true);
        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = availableGuns[currentGun].weaponName;
    }
}
