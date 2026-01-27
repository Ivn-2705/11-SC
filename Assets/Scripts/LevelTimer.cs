using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelTimer : MonoBehaviour
{
    public float startTime = 11f;

    private float currentTime;
    private TextMeshProUGUI timerText;

    void Awake()
    {
        // Usa el propio texto donde está montado
        timerText = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        currentTime = startTime;
        UpdateText();
    }

    void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime < 0f)
            currentTime = 0f;

        UpdateText();

        if (currentTime <= 0f)
        {
            // Reinicia el nivel actual
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void UpdateText()
    {
        timerText.text = Mathf.CeilToInt(currentTime).ToString();
    }
}
