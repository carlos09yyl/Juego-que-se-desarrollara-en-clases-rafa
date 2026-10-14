using UnityEngine;

// Hace que un objeto suba, baje y se balancee suavemente. Sirve para decoracion
// (chupetas del menu, titulo). Funciona igual con objetos del mundo y de la interfaz.
public class FlotarDecoracion : MonoBehaviour
{
    public float amplitud = 0.15f;      // cuanto sube y baja
    public float velocidad = 1.5f;      // que tan rapido
    public float giro = 6f;             // grados de balanceo

    private Vector3 posicionInicial;
    private Quaternion rotacionInicial;
    private float fase;

    private void Start()
    {
        posicionInicial = transform.localPosition;
        rotacionInicial = transform.localRotation;
        fase = Random.value * 10f;      // cada objeto arranca en un punto distinto, asi no se mueven todos igual
    }

    private void Update()
    {
        float t = Time.unscaledTime * velocidad + fase;
        transform.localPosition = posicionInicial + Vector3.up * Mathf.Sin(t) * amplitud;
        transform.localRotation = rotacionInicial * Quaternion.Euler(0f, 0f, Mathf.Sin(t * 0.8f) * giro);
    }
}
