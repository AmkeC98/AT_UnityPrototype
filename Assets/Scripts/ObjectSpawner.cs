using System.Collections;
using UnityEngine;
using System.IO;

public class Spawner : MonoBehaviour
{
    public GameObject testObjectPrefab;
    public Sprite squareImage;
    public Sprite circleImage;
    public int objectCount = 100;
    public bool useCircleCollider = false;
    public bool useRigidbody = true;
    public bool applyGravity = false;
    public float spacing = 1.5f;
    public float testDuration = 10f;

    private float timeAccumulator = 0f;
    private int frameCounter = 0;
    private float testTimer = 0f;
    private float fpsSum = 0f;
    private int fpsSamples = 0;

    private void Start()
    {
        SpawnObjects();
    }

    private void Update()
    {
        timeAccumulator += Time.deltaTime;
        frameCounter++;
        testTimer += Time.deltaTime;

        if (timeAccumulator >= 1f)
        {
            float fps = frameCounter / timeAccumulator;
            Debug.Log($"FPS: {fps:F2}");
            fpsSum += fps;
            fpsSamples++;

            timeAccumulator = 0f;
            frameCounter = 0;
        }

        if (testTimer >= testDuration)
        {
            ExportToCSV();
            enabled = false;
        }
    }

    private void SpawnObjects()
    {
        int rowLength = Mathf.CeilToInt(Mathf.Sqrt(objectCount));

        for (int i = 0; i < objectCount; i++)
        {
            int x = i % rowLength;
            int y = i / rowLength;

            Vector2 position = new Vector2(x * spacing, y * spacing);
            GameObject obj = Instantiate(testObjectPrefab, position, Quaternion.identity);

            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            Collider2D col = obj.GetComponent<Collider2D>();
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

            if (!useRigidbody)
            {
                Destroy(rb);
            }
            else
            {
                rb.gravityScale = applyGravity ? 1f : 0f;
                rb.bodyType = RigidbodyType2D.Dynamic;
            }

            // Replace collider
            Destroy(col);
            if (useCircleCollider)
            {
                obj.AddComponent<CircleCollider2D>();
                sr.sprite = circleImage;
            }
            else
            {
                obj.AddComponent<BoxCollider2D>();
                sr.sprite = squareImage;
            }
        }
    }

    private void ExportToCSV()
    {
        float avgFPS = fpsSamples > 0 ? fpsSum / fpsSamples : 0f;
        string path = Path.Combine("D:\\CMGT\\24-25\\Advanced_Tools\\AT_Assignment\\TestResults", "unity_fps_results.csv");

        bool fileExists = File.Exists(path);
        using (StreamWriter writer = new StreamWriter(path, true))
        {
            if (!fileExists)
            {
                writer.WriteLine("ObjectCount,UseRigidbody,UseCircle,ApplyGravity,AvgFPS");
            }

            writer.WriteLine($"{objectCount},{useRigidbody},{useCircleCollider},{applyGravity},{avgFPS:F2}");
        }

        Debug.Log($"Results written to: {path}");
    }
}