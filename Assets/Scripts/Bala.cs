using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public int damage;

    float suma = 0.001f; // se suma en cada frame, si llega a 1 la bala es destruida (para que no se vaya infinitamente)
    float cont = 0;

    Rigidbody rig;
    [SerializeField] SpriteRenderer ren;
    [SerializeField] GameObject visible; // hace referencia a los gameobjects que contienen el trail y el sprite renderer
    [SerializeField] GameObject particula;


    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        visible.SetActive(false);
        particula.SetActive(false);
        Invoke("ActivarRen", 0.1f);

    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Enemy"))
        {
            Enemigo en = other.gameObject.GetComponent<Enemigo>();
            Destruir();
            en.RecibirAtaque(damage);
        }
        else
        {
            if (other.CompareTag("Muro"))
            {
                Destruir();
            }
        }

        
    }

    void ActivarRen()
    {
        visible.SetActive(true);
    }


    public void CambiarColor(Color color)
    {
        this.ren.color = color;
    }

    // destruye el objeto (creado para invocar)
    void DestruirGO()
    {
        Destroy(this.gameObject);
    }

    // comienza a destruirlo
    public void Destruir()
    {
        rig.velocity = Vector3.zero; // lo frena 
        visible.SetActive(false);

        particula.SetActive(true);
        Invoke("DestruirGO", 0.5f);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (cont >= 1f)
            Destruir();

        cont += suma;
    }
}
