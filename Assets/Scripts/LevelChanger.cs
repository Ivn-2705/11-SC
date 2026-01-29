using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de nivel

public class LevelChanger : MonoBehaviour
{
    [Header("Configuración")]
    public Transform personaje1;
    public Transform personaje2;
    public float distanciaParaPasar = 15f; // Qué tan cerca deben estar
    public string nombreSiguienteNivel;    // Nombre de la escena en Build Settings

    void Update()
    {
        if (personaje1 != null && personaje2 != null)
        {
            // Calculamos la distancia entre los dos personajes
            float distancia = Vector2.Distance(personaje1.position, personaje2.position);

            // Si la distancia es menor al umbral, pasamos de nivel
            if (distancia <= distanciaParaPasar)
            {
                PasarDeNivel();
            }
        }
    }

    void PasarDeNivel()
    {
        Debug.Log("¡Personajes juntos! Cambiando de nivel...");
        SceneManager.LoadScene(nombreSiguienteNivel);
    }
}