using UnityEngine;

// Reproduce una lista de imagenes en bucle sobre un SpriteRenderer.
// Se usa para el personaje del menu, que solo necesita su animacion de quieto.
[RequireComponent(typeof(SpriteRenderer))]
public class AnimacionSimple : MonoBehaviour
{
    public Sprite[] cuadros;
    public float cuadrosPorSegundo = 12f;

    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (cuadros == null || cuadros.Length == 0) return;
        int i = (int)(Time.time * cuadrosPorSegundo) % cuadros.Length;
        sprite.sprite = cuadros[i];
    }
}
