using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Controla la partida: cronometro, cuantas chupetas faltan, y el final.
// Si recoges todas las chupetas antes de que se acabe el tiempo: pantalla de victoria.
// Si se acaba el tiempo: Game Over y el personaje hace su animacion de morir.
// En los dos casos vuelve a la escena de inicio despues de unos segundos.
public class ControlPartida : MonoBehaviour
{
    public static ControlPartida Instancia { get; private set; }

    [Header("Tiempo")]
    public float tiempoLimite = 90f;            // segundos para recoger todas las chupetas
    public float segundosAntesDeVolver = 5f;    // espera en la pantalla final antes de volver al inicio
    public string escenaInicio = "inicio";

    [Header("Interfaz")]
    public TextMeshProUGUI textoTiempo;
    public GameObject panelFinal;
    public TextMeshProUGUI textoTitulo;
    public TextMeshProUGUI textoTituloSombra;   // copia del titulo detras, en morado, como en la pantalla de inicio
    public TextMeshProUGUI textoMensaje;
    public TextMeshProUGUI textoCuentaRegresiva;

    [Header("Jugador")]
    public MovimientoJugador jugador;

    [Header("Sonidos (opcional)")]
    public AudioSource sonidoGanar;
    public AudioSource sonidoPerder;

    private float tiempoRestante;
    private int chupetasTotales;
    private int chupetasRecogidas;
    private bool terminada;
    private Color colorTiempoNormal = Color.white;

    // ---------------------------------------------------------------- llamados desde las chupetas

    public static void RegistrarChupeta()
    {
        if (Instancia != null) Instancia.chupetasTotales++;
    }

    public static void AvisarChupetaRecogida()
    {
        if (Instancia != null) Instancia.ChupetaRecogida();
    }

    // ---------------------------------------------------------------- ciclo de vida

    private void Awake()
    {
        Instancia = this;
        if (panelFinal != null) panelFinal.SetActive(false);
        if (textoTiempo != null) colorTiempoNormal = textoTiempo.color;
    }

    private void OnDestroy()
    {
        if (Instancia == this) Instancia = null;
    }

    private void Start()
    {
        tiempoRestante = tiempoLimite;
        ActualizarTextoTiempo();
    }

    private void Update()
    {
        if (terminada) return;

        tiempoRestante -= Time.deltaTime;
        if (tiempoRestante <= 0f)
        {
            tiempoRestante = 0f;
            ActualizarTextoTiempo();
            Terminar(false);
            return;
        }
        ActualizarTextoTiempo();
    }

    // ---------------------------------------------------------------- logica

    private void ChupetaRecogida()
    {
        if (terminada) return;
        chupetasRecogidas++;
        if (chupetasTotales > 0 && chupetasRecogidas >= chupetasTotales)
            Terminar(true);
    }

    private void ActualizarTextoTiempo()
    {
        if (textoTiempo == null) return;

        int segundos = Mathf.CeilToInt(tiempoRestante);
        textoTiempo.text = (segundos / 60).ToString("00") + ":" + (segundos % 60).ToString("00");

        // los ultimos 10 segundos el reloj se pone rojo y late
        if (!terminada && tiempoRestante <= 10f)
        {
            textoTiempo.color = new Color(1f, 0.3f, 0.3f);
            float latido = 1f + 0.15f * Mathf.Abs(Mathf.Sin(Time.time * 6f));
            textoTiempo.transform.localScale = Vector3.one * latido;
        }
        else
        {
            textoTiempo.color = colorTiempoNormal;
            textoTiempo.transform.localScale = Vector3.one;
        }
    }

    private void Terminar(bool gano)
    {
        terminada = true;
        if (textoTiempo != null) textoTiempo.transform.localScale = Vector3.one;

        // congelar al jugador
        if (jugador != null)
        {
            jugador.puedeMoverse = false;
            if (!gano) ActivarTrigger(jugador.GetComponent<Animator>(), "Morir");
        }

        if (gano && sonidoGanar != null) sonidoGanar.Play();
        if (!gano && sonidoPerder != null) sonidoPerder.Play();

        StartCoroutine(MostrarFinal(gano));
    }

    private IEnumerator MostrarFinal(bool gano)
    {
        // si perdio, primero se ve al personaje caer con su animacion de morir
        if (!gano) yield return new WaitForSecondsRealtime(1.2f);

        if (panelFinal != null)
        {
            panelFinal.SetActive(true);
            string titulo = gano ? "¡GANASTE!" : "GAME OVER";
            if (textoTitulo != null)
            {
                textoTitulo.text = titulo;
                textoTitulo.color = gano ? new Color32(0xFF, 0xD5, 0x4A, 0xFF) : new Color32(0xFF, 0x7A, 0xB6, 0xFF);
            }
            if (textoTituloSombra != null) textoTituloSombra.text = titulo;
            if (textoMensaje != null)
            {
                textoMensaje.text = gano
                    ? "Recogiste las " + chupetasTotales + " chupetas con " + Mathf.CeilToInt(tiempoRestante) + " segundos de sobra"
                    : "Se acabó el tiempo. Recogiste " + chupetasRecogidas + " de " + chupetasTotales + " chupetas";
            }
            StartCoroutine(AnimarPanel());
        }

        yield return VolverAlInicio();
    }

    private IEnumerator AnimarPanel()
    {
        // el panel aparece creciendo un poquito, se ve mas vivo que aparecer de golpe
        Transform t = panelFinal.transform;
        float duracion = 0.35f;
        for (float x = 0f; x < duracion; x += Time.unscaledDeltaTime)
        {
            float k = x / duracion;
            float escala = Mathf.LerpUnclamped(0.6f, 1f, 1f - Mathf.Pow(1f - k, 3f));
            t.localScale = Vector3.one * escala;
            yield return null;
        }
        t.localScale = Vector3.one;
    }

    private IEnumerator VolverAlInicio()
    {
        for (int s = Mathf.CeilToInt(segundosAntesDeVolver); s > 0; s--)
        {
            if (textoCuentaRegresiva != null) textoCuentaRegresiva.text = "Volviendo al inicio en " + s + "...";
            yield return new WaitForSecondsRealtime(1f);
        }
        SceneManager.LoadScene(escenaInicio);
    }

    private static void ActivarTrigger(Animator animator, string nombre)
    {
        if (animator == null || animator.runtimeAnimatorController == null) return;
        foreach (AnimatorControllerParameter p in animator.parameters)
        {
            if (p.name == nombre && p.type == AnimatorControllerParameterType.Trigger)
            {
                animator.SetTrigger(nombre);
                return;
            }
        }
    }
}
