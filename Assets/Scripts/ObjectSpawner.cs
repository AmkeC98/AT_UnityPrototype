using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject prefab;
    public int objectCount = 100;
    public float spacing = 1.5f;
    public bool applyGravity = false;

    private void Start()
    {
        SpawnObjects();
        InvokeRepeating(nameof(LogFPS), 1f, 1f); // Log FPS every second
    }

    private void SpawnObjects()
    {
        int rowLength = Mathf.CeilToInt(Mathf.Sqrt(objectCount));

        for (int i = 0; i < objectCount; i++)
        {
            int x = i % rowLength;
            int y = i / rowLength;

            Vector2 position = new Vector2(x * spacing, y * spacing);
            GameObject obj = Instantiate(prefab, position, Quaternion.identity);

            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = applyGravity ? 1f : 0f;
                //rb.linearVelocity = applyGravity ? Vector2.zero : new Vector2(1f, 0f); // Optional: constant horizontal motion
            }
        }
    }

    private void LogFPS()
    {
        float fps = 1.0f / Time.deltaTime;
        Debug.Log("FPS: " + fps.ToString());
    }
}
