using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI startText; // Texto "Pulsa espacio para empezar"
    public TextMeshProUGUI gameOverText; // Texto "GAME OVER"
    public TextMeshProUGUI scoreText; // Texto para mostrar el puntaje
    public Button restartButton; // Botón de reinicio
    public SnakeMov snake; // Referencia al script de movimiento de la serpiente
    public SpecialFruit specialFruit; // Prefab de la fruta especial
    public FoodAppear food; // Referencia al script de generación de fruta
    public AudioClip ThemeBackground; // Sonido de inicio
    public AudioClip SoundGameOver; // Sonido de Game Over
    private AudioSource audioSource;
    private int score; // Variable para almacenar el puntaje
    private bool isGameStarted = false;
    public bool specialFruitSpawned = false;
    private int nextSpecialFruitScore = 100; // Puntos requeridos para la próxima fruta especial

    private void Start()
    {
        // Configurar AudioSource
        audioSource = GetComponent<AudioSource>();
        // Mostrar el texto de inicio y ocultar el texto de GAME OVER al principio
        startText.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false); // Ocultar el puntaje al inicio
        Time.timeScale = 0; // Detener el juego

        UpdateScoreText(false);
    }

    private void Update()
    {
        if (!isGameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        if (score >= nextSpecialFruitScore && !specialFruitSpawned)
        {
            nextSpecialFruitScore += 100; // Incrementar el objetivo para la próxima fruta
            SpawnSpecialFruit();
        }
    }

    private void StartGame()
    {
        isGameStarted = true;
        startText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        Time.timeScale = 1; // Reanudar el juego
        snake.ResetState();
        specialFruitSpawned = false;
        ClearFruits();

        // Reproducir sonido de inicio
        PlaySound(ThemeBackground);

        // Asignar la acción al botón de reinicio
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(RestartGame);
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true); // Mostrar botón de reinicio
        Time.timeScale = 0; // Pausar el juego

        // Reproducir sonido de Game Over
        PlaySound(SoundGameOver);

    }

    private void RestartGame()
    {
        // Reiniciar el estado de la serpiente y reiniciar el juego
        food.DesignatePosition();
        UpdateScoreText(false);
        StartGame();
    }

    public void UpdateScoreText(bool statusScore = true)
    {
        if (statusScore)
        {
            score += 10;
        }
        else
        {
            score = 0;
            nextSpecialFruitScore = 100;
        }
        scoreText.text = $"Puntos: {score}"; // Actualizar el texto del puntaje
    }

    private void SpawnSpecialFruit()
    {
        specialFruitSpawned = true;

        // Generar la fruta especial en una posición aleatoria dentro del área
        Vector3 position = food.GetRandomPosition();
        SpecialFruit newSpecialFruit = Instantiate(specialFruit, position, Quaternion.identity);

        // Asignar el GameManager al prefab instanciado
        newSpecialFruit.gameManager = this;

        newSpecialFruit.tag = "SpecialFood";
    }

    public void BoostSnakeSpeed()
    {
        // Incrementar temporalmente la velocidad de la serpiente
        snake.BoostSpeed(2f, 5f); // 1.5x velocidad por 5 segundos
    }

    public void SpawnExtraFruits(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 position = food.GetRandomPosition();
            food.SpawnFood(position);
        }
    }

    private void ClearFruits()
    {
        foreach (var specialFruit in GameObject.FindGameObjectsWithTag("SpecialFood"))
        {
            Destroy(specialFruit);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(clip);
        }
    }
}
