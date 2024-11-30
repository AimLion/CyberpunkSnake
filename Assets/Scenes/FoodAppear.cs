using System.Collections.Generic;
using UnityEngine;

public class FoodAppear : MonoBehaviour
{
    public BoxCollider2D gridArea;
    public AudioClip SoundFood; // Sonido de la fruta
    private AudioSource audioSource;
    public GameManager gameManager; // Referencia al GameManager

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DesignatePosition();
    }

    public void DesignatePosition()
    {
        Bounds bounds = this.gridArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DesignatePosition();
        PlaySound(SoundFood);
        gameManager.UpdateScoreText();
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SoundFood o AudioSource no está asignado.");
        }
    }

    public void SpawnFood(Vector3 position)
    {
        GameObject newFood = Instantiate(this.gameObject, position, Quaternion.identity);
        newFood.tag = "SpecialFood";
    }

    public Vector3 GetRandomPosition()
    {
        Bounds bounds = this.gridArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
    }
}
