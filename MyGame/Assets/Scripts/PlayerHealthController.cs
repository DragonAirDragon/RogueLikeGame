using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;
    // Start is called before the first frame update
    public int currentHealth;
    public int maxHealth;
    // Для временной неуязвимости
    public float damageInvincLenght = 1f;
    private float invincCount;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        maxHealth = CharacterTracker.instance.maxHealth;
        currentHealth = CharacterTracker.instance.currentHealth;
        //currentHealth = maxHealth;
        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //счетчик неуязвимости после выстрела
        if (invincCount > 0)
        {
            invincCount -= Time.deltaTime;
            if (invincCount <= 0)
            {
                PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, 1f);
            }
        }
    }
    public void DamagePlayer()
    {
        if (invincCount <= 0)
        {
            currentHealth--;
            AudioManager.instance.PlaySFX(11);//Звук удара по игроку так как в единственном экземпляре поэтому без фикс значений
            invincCount = damageInvincLenght;
            PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, .5f);
            if (currentHealth <= 0)
            {
                PlayerController.instance.gameObject.SetActive(false);
                AudioManager.instance.PlaySFX(10);// звук смерти игрока
                UIController.instance.deathScreen.SetActive(true);
                AudioManager.instance.PlayGameOver();
            }


            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }
    }
    public void MakeInvincible(float length)
    {
        invincCount = length;
        PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, .5f);
    }
    public void HealPlayer(int healAmount)
    { 
        currentHealth += healAmount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;
        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
}
