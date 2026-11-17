using UnityEngine;

public class Sinusoidal : MonoBehaviour
{
    
    public int width = 5;
    public int height = 5;
    public float spacing = 2;
    public GameObject cellprefab;
    public float amplitud = 1;
    public float stepTime = 0.01f;
    public float period = 1;
    public GameObject[,] cells;
    private Vector2[,] initialPositions;
    private float[,] phases;
    private float time;

    void Start()
    {
        Application.targetFrameRate = 90;
        cells = new GameObject[width, height];
        initialPositions = new Vector2[width, height];
        phases = new float[width, height];

        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                Vector2 pos = new Vector2(x * spacing, y * spacing);
                GameObject cell = Instantiate(cellprefab, pos, Quaternion.identity, transform);
                cells[x, y] = cell;
                initialPositions[x, y] = pos;
                phases[x, y] = (x+y) * Mathf.PI / 4f;
            }
        }
        cellprefab.SetActive(false);
    }

    void Update()
    {
        time += stepTime;
        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                Vector2 basePos = initialPositions[x, y];
                float phase = phases[x, y];
                //Onda sinusoidal : altura = amplitud * sin(2*PI * tiempo/periodo + fase)
                float altura = amplitud*Mathf.Sin(2*Mathf.PI * time/period + phase);
                //float altura = amplitud * Mathf.Sin(2 * Mathf.PI * Time.time / Time.deltaTime + phase);
                Vector2 newPosition = new Vector2(basePos.x, basePos.y + altura);
                cells[x, y].transform.position = newPosition;
            }
        }
    }
}
