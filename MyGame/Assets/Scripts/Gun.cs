using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletToFire; //ссылка на пулю
    public Transform firePoint; // Точка выстрела

    public float timeBetweenShots; //Частота выстрелов при нажатии
    private float shotCounter;
    public int shootSFX;
    public string weaponName;
    public Sprite gunUI;


    //нужно для магазина
    public int itemCost;
    public Sprite gunShopSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.canMove && !LevelManager.instance.isPaused) { 

            if (shotCounter > 0)
            {
                shotCounter-= Time.deltaTime;
            }
            else
            {
                if(Input.GetMouseButtonDown(0)||Input.GetMouseButton(0)) {
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                    shotCounter = timeBetweenShots;
                    AudioManager.instance.PlaySFX(shootSFX);
                }


                /*
                if (Input.GetMouseButtonDown(0))
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
            }
        }
    }
}
