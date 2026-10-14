using UnityEngine;

public class RecolectableChupeta1_Controlador : MonoBehaviour
{
    public AudioSource SonidoOcio;
    public AudioSource SonidoRecolectado;
    public ContadorChupetas1 contador;      // contador de la interfaz que sube +1 cuando se recoge esta chupeta

    [Header("Particulas al recoger")]
    public GameObject particulas;           // prefab de particulas que explota al recoger la chupeta
    public Color colorParticulas = new Color(1f, 0.55f, 0.15f);

    private bool recolectada = false;       // evita que una misma chupeta sume dos veces

    private void Awake()
    {
        if (SonidoOcio != null)
            SonidoOcio.Play();              // reproducir sonido de ocio
    }

    private void Start()
    {
        ControlPartida.RegistrarChupeta();  // la partida cuenta cuantas chupetas hay en el nivel
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (recolectada) return;

        if (collision.CompareTag("Personaje"))
        {
            recolectada = true;

            if (contador != null)
                contador.AumentarContador();    // +1 en la interfaz, el contador emite su sonido
            else
                Debug.LogWarning("La chupeta " + name + " no tiene un contador asignado");

            SoltarParticulas();
            ControlPartida.AvisarChupetaRecogida();

            Destruir(); //>> Ejecutar destruccion
        }
    }

    private void SoltarParticulas()
    {
        if (particulas == null) return;

        GameObject efecto = Instantiate(particulas, transform.position, Quaternion.identity);
        ParticleSystem ps = efecto.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            var main = ps.main;
            main.startColor = new ParticleSystem.MinMaxGradient(colorParticulas, Color.white);
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

    // el +1 ya no se hace en OnDestroy porque OnDestroy tambien se ejecuta al cambiar de escena
    // o al salir del modo Play, y eso sumaria chupetas que nadie recogio
}
