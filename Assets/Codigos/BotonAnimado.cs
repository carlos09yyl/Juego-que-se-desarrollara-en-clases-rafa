using UnityEngine;
using UnityEngine.EventSystems;

// Animacion de los botones del menu: crecen al pasar el mouse, se encogen al presionarlos
// y, si se activa "latido", laten suave para llamar la atencion.
public class BotonAnimado : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public float escalaEncima = 1.08f;
    public float escalaPresionado = 0.95f;
    public bool latido = false;

    private float objetivo = 1f;
    private bool encima;

    public void OnPointerEnter(PointerEventData e) { encima = true; objetivo = escalaEncima; }
    public void OnPointerExit(PointerEventData e)  { encima = false; objetivo = 1f; }
    public void OnPointerDown(PointerEventData e)  { objetivo = escalaPresionado; }
    public void OnPointerUp(PointerEventData e)    { objetivo = encima ? escalaEncima : 1f; }

    private void Update()
    {
        float meta = objetivo;
        if (latido && !encima) meta *= 1f + 0.03f * Mathf.Sin(Time.unscaledTime * 4f);

        float actual = transform.localScale.x;
        float nueva = Mathf.Lerp(actual, meta, 1f - Mathf.Exp(-14f * Time.unscaledDeltaTime));
        transform.localScale = Vector3.one * nueva;
    }
}
