using UnityEngine;
using UnityEngine.SceneManagement;

public class respawn : MonoBehaviour
{
    [Header("Altura del vacío")]
    public float deathY = -10f;

    void Update()
    {
        // Si el personaje cae al vacío
        if (transform.position.y <= deathY)
        {
            ReiniciarNivel();
        }
    }

    void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
