using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Imprescindible para cargar escenas

public class MenuPrincipal : MonoBehaviour
{
    [Header("Paneles del Menú")]
    public GameObject panelMenu;
    public GameObject subMenuOpciones;
    public GameObject panelNiveles;

    [Header("Configuración Volumen")]
    public TextMeshProUGUI textoVolumen;
    public Slider sliderVolumen;

    // 1. Abre el menú de opciones
    public void IrAOpciones() {
        panelMenu.SetActive(false);
        subMenuOpciones.SetActive(true);
    }

    // 2. Abre el selector de niveles
    public void IrANiveles() {
        panelMenu.SetActive(false);
        panelNiveles.SetActive(true);
    }

    // 3. Vuelve al menú principal desde cualquier panel
    public void VolverAlMenu() {
        if(subMenuOpciones != null) subMenuOpciones.SetActive(false);
        if(panelNiveles != null) panelNiveles.SetActive(false);
        panelMenu.SetActive(true);
    }

    // 4. Actualiza el texto del porcentaje del slider
    public void ActualizarTextoVolumen() {
        if (sliderVolumen != null && textoVolumen != null) {
            float valor = sliderVolumen.value * 100;
            textoVolumen.text = valor.ToString("F0") + "%";
        }
    }

    // 5. Carga el nivel según el nombre que escribas en el botón
    public void CargarNivel(string nombreEscena) {
        SceneManager.LoadScene(nombreEscena);
    }

    // 6. Cierra el juego
    public void Salir() {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}