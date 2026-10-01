using UnityEngine;

public class RecolectableChupeta1_Controlador : MonoBehaviour
{
    public AudioSource SonidoOcio;
    public AudioSource SonidoRecolectado;
    private void Awake()
    {
        SonidoOcio.Play();                  // reproducir sonido de ocio
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Personaje"))
        {
            Destruir(); //>> Ejecutar destruccion
        }
    }

    [ContextMenu("Destruir Chupeta")]
    public void Destruir()
    {
        Destroy(gameObject);                // destruir la chupeta1
    }

    /*
    // la interfaz está emitiendo este sonido
    [ContextMenu("Reproducir Sonido Recolectado")]
    public void ReproducirSonidoRecolectado()
    {
        SonidoRecolectado.Play();           // reproducir - sonido de recoleccion de la chupeta
    }
    */

    private void OnDestroy()
    {
        print("Subir +1 en la interfaz");   // informar la interfaz que sume +1 chupeta
        
    }
}
