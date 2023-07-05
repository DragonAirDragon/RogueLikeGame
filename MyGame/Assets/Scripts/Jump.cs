using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
   
    //Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerController.instance.dashCounter <= 0f)
        {
            GetComponent<Collider2D>().isTrigger = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {    
        if (other.gameObject.tag == "Player" && PlayerController.instance.dashCounter > 0)
        {
            GetComponent<Collider2D>().isTrigger = true;
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && PlayerController.instance.dashCounter > 0)
        {
            GetComponent<Collider2D>().isTrigger = true;
        }
    }
}
