using System.Collections.Generic;
using UnityEngine;

public class SnakeMov : MonoBehaviour
{
    private Vector2 direction = Vector2.right;
    private List<Transform> segments;
    public Transform segmentPrefab;
    public GameManager gameManager;

    private void Start()
    {
        segments = new List<Transform>();
        segments.Add(this.transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2.down)
        {
            direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2.up)
        {
            direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right)
        {
            direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) && direction != Vector2.left)
        {
            direction = Vector2.right;
        }
    }

    private void FixedUpdate()
    {
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }

        this.transform.position = new Vector3(Mathf.Round(this.transform.position.x + direction.x), Mathf.Round(this.transform.position.y + direction.y), 0.0f);
    }

    private void Grow()
    {
        Transform body = Instantiate(this.segmentPrefab);

        body.position = segments[segments.Count - 1].position;

        segments.Add(body);
    }

    private void Test()
    {
        print("hello");
    }

    public void ResetState()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }

        segments.Clear();
        segments.Add(this.transform);

        this.transform.position = Vector3.zero;
        direction = Vector2.right;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Food"))
        {
            Grow();
        }
        else if(other.gameObject.CompareTag("Obstacle"))
        {
            gameManager.GameOver();
        }
    }

    public void BoostSpeed(float multiplier, float duration)
    {
        StartCoroutine(TemporarySpeedBoost(multiplier, duration));
    }

    private System.Collections.IEnumerator TemporarySpeedBoost(float multiplier, float duration)
    {
        float originalSpeed = 1;
        Time.timeScale = originalSpeed * multiplier;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = originalSpeed;
    }

}
