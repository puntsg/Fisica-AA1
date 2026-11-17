using UnityEngine;

public class Fluid : MonoBehaviour
{
    [Header("Fluid Properties")]
    public float density = 1000f;
    public float fluidLevel = 0f;
    public float dragCoefficient = 0.5f;
    public WaveManger waveManager;

    public virtual float GetSurfaceHeight(Vector3 position)
    {
        if (waveManager != null)
            return waveManager.GetHeightAt(position);
        return fluidLevel;
    }
}
