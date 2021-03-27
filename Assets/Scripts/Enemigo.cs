using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour
{
    int vidaMax = 100;
    int vida;
    
    int damage = 10;



    NavMeshAgent nav;
    Transform target;
    Quaternion Inicial;

    Player player;

    GameObject playerGO;

    float contAtaque = 0f;
    float tiempoEntreAtaques = 1f;

    [SerializeField]SpriteRenderer spriteRen;

    bool puedeMoverse = false;
    //[SerializeField] ParticleSystem particula;

    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player") && contAtaque >= tiempoEntreAtaques)
        {
            try
            {
                player.RecibirAtaque(damage);
            }
            catch (System.Exception)
            {

                Debug.Log("No se encuentra al player");
            }
            contAtaque = 0;
        }
        
    }

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        playerGO = GameObject.FindGameObjectWithTag("Player");
        target = playerGO.transform;
        player = playerGO.GetComponent<Player>();
        Inicial = transform.rotation;
        vida = vidaMax;

        Invoke("ActivarMov", 1.5f);
    }

    void ActivarMov()
    {
        puedeMoverse = true;
    }

    void RestablecerColor()
    {
        spriteRen.color = Color.white;
    }

    public void RecibirAtaque(int damage)
    {
        vida -= damage;

        if (vida <= 0)
            Destroy(this.gameObject);

        spriteRen.color = Color.red;
        Invoke("RestablecerColor", 0.2f);
        //particula.Emit(50);

        // encoje al enemigo seg�n su vida
        float vidaEscalada = (float)vida / vidaMax; // escala la vida en un rango entre 0 y 1 
        float escala = vidaEscalada * vidaEscalada + 0.5f;
        if (escala > 1)
            escala = 1;

        transform.localScale = new Vector3(escala, 1, escala);

    }

    private void Update()
    {
        spriteRen.flipX = (target.position.x < transform.position.x);

        

        if (puedeMoverse)
        {
            

            nav.SetDestination(target.position);
            nav.transform.rotation = Inicial;

            if (contAtaque < tiempoEntreAtaques)
                contAtaque += Time.deltaTime;
        }
        
        
    }
}
