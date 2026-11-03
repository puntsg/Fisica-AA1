using System;
using UnityEngine;

public class WaveManger : MonoBehaviour
{
    [Serializable]
    public enum EWaveType { SINUSOIDAL, GERSTNER }

    [SerializeField]
    public EWaveType waveType;
    public int width = 5;
    public int height = 5;
    public float spacing = 2;
    public GameObject cellprefab;
    public float amplitud = 1;
    public float stepTime = 0.01f;
    public float period = 1;
    public GameObject[,] cells;
    private Vector3[,] initialPositions;
    private Vector3 direction;
    private float[,] phases;
    private float time;

    void SetParams()
    {
        direction = AA3Params.instance.direction;
        amplitud = AA3Params.instance.amplitude;
        float frequency = AA3Params.instance.frequency;
        period = (frequency > 0f) ? 1f / frequency : period;
        waveType = (EWaveType)AA3Params.instance.waveType;
    }

    void Start()
    {
        SetParams();
        AA3Params.instance.onAnyParamEdited.AddListener(SetParams);
        Application.targetFrameRate = 90;
        cells = new GameObject[width, height];
        initialPositions = new Vector3[width, height];
        phases = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                
                Vector3 pos = new Vector3((x * spacing)- ((width * spacing)/2), 0f, (y * spacing)-((height * spacing)/2));
                GameObject cell = Instantiate(cellprefab, pos, Quaternion.identity, transform);
                cells[x, y] = cell;
                initialPositions[x, y] = pos;
                phases[x, y] = (x + y) * Mathf.PI / 4f;
            }
        }
        cellprefab.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            AA3Params.instance.ResetParams();
            GeneralParams.instance.ResetParams();
            time = 0f;
        }
        time += stepTime * GeneralParams.instance.timeMultiplier;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 basePos = initialPositions[x, y];
                float phase = 0;
                Vector3 newPosition = Vector3.zero;
                switch (waveType)
                {
                    case EWaveType.GERSTNER:
                        Vector2 k = new Vector2(direction.x, direction.z);
                        float k_mag = k.magnitude;
                        if (k_mag < 0.0001f)
                        {
                            newPosition = basePos;
                            break;
                        }
                        //Frecuencia angular : omega = 2 * PI / periodo
                        float omega = 2f * Mathf.PI / period;
                        phase = k.x * basePos.x + k.y * basePos.z - omega * time;
                        float offsetX = -(k.x / k_mag) * amplitud * Mathf.Sin(phase);
                        float offsetZ = -(k.y / k_mag) * amplitud * Mathf.Sin(phase);
                        float offsetY = amplitud * Mathf.Cos(phase);
                        newPosition = new Vector3(basePos.x + offsetX, offsetY, basePos.z + offsetZ);
                        break;
                    case EWaveType.SINUSOIDAL:
                        phase = phases[x, y];
                        //Onda sinusoidal : altura = amplitud * sin(2*PI * tiempo/periodo + fase)
                        float altura = amplitud * Mathf.Sin(2 * Mathf.PI * time / period + phase);
                        newPosition = new Vector3(basePos.x, altura, basePos.z);
                        break;
                }
                cells[x, y].transform.position = newPosition;
            }
        }
    }

    public float GetHeightAt(Vector3 worldPos)
    {
        if (cells == null) return 0f;
        float bestDist = float.MaxValue;
        float surfaceY = 0f;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 p = cells[x, y].transform.position;
                float dx = p.x - worldPos.x;
                float dz = p.z - worldPos.z;
                float dist = dx * dx + dz * dz;
                if (dist < bestDist)
                {
                    bestDist = dist;
                    surfaceY = p.y;
                }
            }
        }
        return surfaceY;
    }
}
