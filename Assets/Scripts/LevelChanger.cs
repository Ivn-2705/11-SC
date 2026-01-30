using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelVictoria;

    [Header("Configuración")]
    public Transform personaje1;
    public Transform personaje2;
    public float distanciaParaPasar = 2.5f; // Prueba con 2.5 o 3 si 2 es muy poco

    private bool yaGano = false;

    void Update()
    {
        if (!yaGano && personaje1 != null && personaje2 != null)
        {
            float distancia = Vector2.Distance(personaje1.position, personaje2.position);

            // Esto te ayudará a ver la distancia real en la consola de Unity
            // Debug.Log("Distancia actual: " + distancia);

            if (distancia <= distanciaParaPasar)
            {
                MostrarMenuVictoria();
            }
        }
    }

    void MostrarMenuVictoria()
    {
        yaGano = true;
        if(panelVictoria != null)
        {
            panelVictoria.SetActive(true);
        }
        
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // --- FUNCIONES PARA LOS BOTONES ---
    public void SiguienteNivel(string nombreNivel)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreNivel);
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlMenu(string nombreMenu)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreMenu);
    }
}