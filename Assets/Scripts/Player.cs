
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] int vidaMax = 100;
    int ataqueBasica = 20;
    float velocidadDeAtaqueBasica = 0.8f;
    float velBalaBasica = 8f;
    float velocidadBasica = 3f;

    int vida;
    int ataque;
    float velocidadDeAtaque;
    float velBala;
    float velocidad;


    [SerializeField] GameObject bala;
    [SerializeField] Slider sliderVida;
    [SerializeField] AudioClip disparo;
    [SerializeField] AudioClip getHit;
    AudioSource audioSource;


    [SerializeField]Animator ac;
    

    Camera cam;
    Rigidbody rig;
    SpriteRenderer spriteRen;
    Color colorPorDefecto;

    public float range = 100f;
    float timer;


    [SerializeField]bool[] mejorasAdquiridas = new bool[4];
    // Start is called before the first frame update


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pocion"))
        {
            Pocion pocionScript = other.GetComponent<Pocion>();
            pocionScript.Destruir();
            Mejora(pocionScript.id);
            
        }
    }
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        spriteRen = GetComponent<SpriteRenderer>();

        colorPorDefecto = new Color(spriteRen.color.r, spriteRen.color.g, spriteRen.color.b, 1);

        cam = Camera.main;

        vida = vidaMax;
        ataque = ataqueBasica;
        velocidadDeAtaque = velocidadDeAtaqueBasica;
        velBala = velBalaBasica;
        velocidad = velocidadBasica;
    }




    void Morir()
    {
        Debug.Log("MORIR");
    }


    void VolverAColorNormal()
    {
        spriteRen.color = colorPorDefecto;
    }

    public void RecibirAtaque(int damage)
    {
        audioSource.clip = getHit;
        audioSource.Play();

        // animacion de recibir ataque
        spriteRen.color = Color.red * 0.8f;
        Invoke("VolverAColorNormal", 0.2f);

        

        vida -= damage;
        if(vida <= 0)
        {
            Morir();
        }

        // cambia el slider
        sliderVida.value = (float)vida / (float)vidaMax;
    }






    void Disparar()
    {
        timer = 0; // situo el tiempo entre disparos a 0

        // reproduzco el sonido
        audioSource.clip = disparo;
        audioSource.Play();

        // instancio la bala
        GameObject balaGO = Instantiate(bala, transform.position, bala.transform.rotation); // obtengo el GO de la bala+

        Rigidbody rigBala = balaGO.GetComponent<Rigidbody>(); // obtengo el rigidvody de la bala 
        Bala balaScript = bala.GetComponent<Bala>(); // obtengo el script
        balaScript.damage = ataque;// le asigno el da�o que tendr�

        // calculo su direcci�n 
        Vector3 posMouse = cam.ScreenToWorldPoint(Input.mousePosition);
        posMouse.y = balaGO.transform.position.y; // nunca ira en el 3 eje

        Vector3 fuerzaVala = (transform.position - posMouse);
        rigBala.velocity = -(fuerzaVala.normalized * velBala);


        balaGO.transform.LookAt(posMouse);

        if (mejorasAdquiridas[3]) // si tengo más daño
        {
            balaScript.CambiarColor(Color.red * 0.8f);
        }

    }

    void CheckeaDisparo()
    {
        timer += Time.deltaTime;

        if (Input.GetButton("Fire1") && timer >= velocidadDeAtaque && Time.timeScale != 0)
            Disparar();
    }

    void Movimiento()
    {
        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = (int)Input.GetAxisRaw("Vertical");

        if(horizontal != 0)
            spriteRen.flipX = (!(horizontal == 1));


        if (horizontal != 0 || vertical != 0)
            ac.SetBool("Walking", true);
        else
            ac.SetBool("Walking", false);

        Vector3 aux = transform.position;

        aux.x += horizontal * velocidad * Time.deltaTime;
        aux.z += vertical * velocidad * Time.deltaTime;

        rig.MovePosition(aux);    
    }



    void Curar(int cant)
    {
        vida += cant;
        if (vida > vidaMax)
            vida = vidaMax;
    }
    public void Mejora(int tipoDeMejora)
    {
        if (tipoDeMejora == -1 || !mejorasAdquiridas[tipoDeMejora]) // si el tipo de mejora es curacion o no tengo la mejora
        {
            mejorasAdquiridas[tipoDeMejora] = true; // obtengo la mejora

            switch (tipoDeMejora)
            {
                case -1:// curacion
                    Curar(50);
                    break;
                case 0: // velocidad
                    velocidad *= 2;
                    break;
                case 1: // velocidad bala
                    velBala *= 2;
                    break;
                case 2:
                    velocidadDeAtaque /= 2;
                    break;
                case 3: // Mejora de daño 
                    ataque = (int)(ataque * 1.5f);
                    break;
                default:
                    Debug.Log("ESA MEJORA NO ESTA EN LA LISTA: " + tipoDeMejora);
                    break;
            }
        }
        else
        {
            Curar(50);
        }
        
    }



    public void setVida(int vida)
    {
        this.vida = vida;
    }

    public void setVelocidad(float velocidad)
    {
        this.velocidad = velocidad;
    }

    public void setVelBala(float velBala)
    {
        this.velBala = velBala;
    }

    public void setVelAtaque(float velAtaque)
    {
        this.velocidadDeAtaque = velAtaque;
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
        CheckeaDisparo();
    }
}
