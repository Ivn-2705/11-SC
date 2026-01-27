using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMenu : MonoBehaviour
{
    // Carga la escena del juego
    public void Jugar()
    {
        SceneManager.LoadScene(1); // El número 1 será tu nivel
    }

    // Cierra el juego
    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliendo...");
    }

    // Ajusta el volumen
    public void Volumen(float v)
    {
        AudioListener.volume = v;
    }
}