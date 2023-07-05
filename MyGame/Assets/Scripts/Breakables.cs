using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    public GameObject[] brokenPieces;
    public int maxPieces = 5;
    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;
    public int breakeSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Smash()
    {
        Destroy(gameObject);
        AudioManager.instance.PlaySFX(breakeSound);
        //Появление обломков
        int piecesToDrop = Random.Range(2, maxPieces);
        for (int i = 0; i < piecesToDrop; i++)
        {
            int randomPiece = Random.Range(0, brokenPieces.Length);
            Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
        }
        //Появление предмета
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerBullet") //при выстреле уничтожение
        {
            Smash();
        }
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player") //при перекате уничтожение
        {
            if (PlayerController.instance.dashCounter > 0)
            {
                Smash();
            }
        }
    }
}
