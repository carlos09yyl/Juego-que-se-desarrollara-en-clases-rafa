using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlEscenas : MonoBehaviour
{
    private void Awake()
    {
        print("Control de Escenas - Activado");
    }

    public void CargarEscenaInicio()
    {
        SceneManager.LoadScene("inicio");
    }

    public void CargarEscenaNivel()
    {
        SceneManager.LoadScene("nivel");
    }
}