using UnityEngine;

public class SpecialFruit : MonoBehaviour
{
    public Color blueColor = Color.blue; // Color azul
    public Color purpleColor = Color.magenta; // Color morado
    public GameManager gameManager; // Referencia al GameManager
    private SpriteRenderer spriteRenderer;
    private bool isBlue = true; // Indica si la fruta es azul
    private float colorChangeInterval = 1f; // Tiempo entre cambios de color
    private float lifeTime = 15f; // Tiempo de vida en segundos

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(ChangeColor());
        Destroy(gameObject, lifeTime); // Destruir después de un tiempo
    }

    private System.Collections.IEnumerator ChangeColor()
    {
        while (true)
        {
            // Cambiar entre azul y morado
            isBlue = !isBlue;
            spriteRenderer.color = isBlue ? blueColor : purpleColor;

            yield return new WaitForSeconds(colorChangeInterval);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Snake")) // Si la serpiente toca la fruta
        {
            if (isBlue)
            {
                // Si es azul, aumentar la velocidad de la serpiente
                gameManager.BoostSnakeSpeed();
            }
            else
            {
                // Si es morado, generar 3 frutas extra
                gameManager.SpawnExtraFruits(3);
            }
            gameManager.specialFruitSpawned = false;

            Destroy(gameObject);
        }
    }
}
