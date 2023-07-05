using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public bool dashingObject=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dashingObject)
        {
            if (PlayerController.instance.dashCounter <= 0)
            {
                GetComponent<Collider2D>().isTrigger = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer();
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer();
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer();
        }
        if (dashingObject)
        {
            if (other.gameObject.tag == "Player" && PlayerController.instance.dashCounter > 0)
            {
                GetComponent<Collider2D>().isTrigger = true;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer();
        }
        if (dashingObject)
        {
            if (other.gameObject.tag == "Player" && PlayerController.instance.dashCounter > 0)
            {
                GetComponent<Collider2D>().isTrigger = true;
            }
        }
    }
}
