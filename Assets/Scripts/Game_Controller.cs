using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Game_Controller : MonoBehaviour
{
    [SerializeField] AudioClip intro;
    [SerializeField] AudioClip loop;
    [SerializeField] GameObject enemigo;
    [SerializeField] Vector2 RangoSpawn;
    [SerializeField] GameObject player;
    [SerializeField] GameObject pocion;
    AudioSource audioSource;
    NavMeshSurface surfaceMesh;


    float timeToNextWave = 1f; // cuando el contador llega a este tiempo, se instancia la siguiente oleada
    int wave = 0; // el numero de oleada
    float cont = 0f;



    [SerializeField]Text textoOleada;
    float contCartelOleada = 0f; // para mostrar la transparencia
    readonly float velDesvanecimiento = 0.3f; // se multiplica al tiempo, si es 1 tarda un seg, si es 2 tarda 0.5 segundos etc
    bool mostrarCartel = false;
    bool desapareciendo = false;


    private void Awake()
    {
        Debug.Log("Comenzando");
        surfaceMesh = GetComponentInChildren<NavMeshSurface>();
        surfaceMesh.BuildNavMesh();
        audioSource = GetComponent<AudioSource>();

        //hago el texto transparente
        Color textoOleadaColor = textoOleada.color;
        textoOleadaColor.a = 0;
        textoOleada.color = textoOleadaColor;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        audioSource.clip = intro;
        audioSource.Play();
        
    }
   

    void MostrarCartelOleada()
    {
        textoOleada.text = "WAVE " + wave;
        if (!desapareciendo)
        {
            contCartelOleada += Time.deltaTime * velDesvanecimiento;
            if (contCartelOleada >= 1)
            {
                contCartelOleada = 1;
                desapareciendo = true;
            }
                
                
        }
        else
        {
            contCartelOleada -= Time.deltaTime * velDesvanecimiento;

            if (contCartelOleada <= 0)
            {
                contCartelOleada = 0;
                desapareciendo = false;
                mostrarCartel = false;
            }
                
        }

        Color aux = textoOleada.color;
        aux.a = contCartelOleada;
        textoOleada.color = aux;
    }


    Vector3 RandomSpawn()
    {
        return new Vector3(Random.Range(-RangoSpawn.x, RangoSpawn.x), 0.1f, Random.Range(-RangoSpawn.y, RangoSpawn.y));
    }
    // instancia la oleada
    void InstanciarOleada()
    {
        int cantEnemigos = (int)Mathf.Log(wave, 2) + 1;
        
        

        for (int i = 0; i < cantEnemigos; i++)
        {
            Instantiate(enemigo, RandomSpawn(), enemigo.transform.rotation);
        }


        if(wave % 2 == 0) // en las oleadas pares
        {
           Instantiate(pocion, RandomSpawn(), pocion.transform.rotation);
        }
    }
    
    private void FixedUpdate()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = loop;
            audioSource.Play();
        }

        cont += Time.deltaTime;
        if(cont >= timeToNextWave)
        {
            mostrarCartel = true;

            cont = 0;
            wave++;

            timeToNextWave = 5f * ((int)Mathf.Log(wave, 2) + 1);
            if (timeToNextWave > 15f)
                timeToNextWave = 15f;

            Invoke("InstanciarOleada", 1f);
        }

        if (mostrarCartel)
        {
            MostrarCartelOleada();
        }
    }

}
