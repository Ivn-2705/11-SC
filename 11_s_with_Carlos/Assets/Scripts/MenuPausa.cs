using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem; // <--- NUEVA LIBRERÍA PARA UNITY 6

public class ControladorPausa : MonoBehaviour
{
    public GameObject panelPausa;
    private bool estaPausado = false;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        if (panelPausa.TryGetComponent<Image>(out Image img))
        {
            img.color = new Color(0, 0, 0, 0.7f);
        }
        panelPausa.SetActive(false); // Asegura que empiece oculto
    }

    void Update()
    {
        // Nueva forma de detectar la tecla ESC en Unity 6
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (estaPausado) Reanudar();
            else Pausar();
        }
    }

    public void Pausar()
    {
        panelPausa.SetActive(true);
        Time.timeScale = 0f;
        estaPausado = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Reanudar()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1f;
        estaPausado = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}