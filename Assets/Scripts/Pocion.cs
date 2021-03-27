using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocion : MonoBehaviour
{
    public int id;

    SpriteRenderer spriteRen;
    private void Awake()
    {
        spriteRen = GetComponent<SpriteRenderer>();
        id = Random.Range(-1, 4);
        switch (id)
        {
            case -1:
                spriteRen.color = Color.red;
                break;
            case 0:
                spriteRen.color = Color.blue;
                break;
            case 1:
                spriteRen.color = Color.yellow;
                break;
            case 2:
                spriteRen.color = Color.black;
                break;
            case 3:
                spriteRen.color = Color.green;
                break;
        }
    }

    public void Destruir()
    {
        Destroy(this.gameObject);
    }
}
