using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AA3Params : MonoBehaviour
{
    public static AA3Params instance;

    public UnityEvent onAnyParamEdited;

    public float frequency = 0.5f;
    public float amplitude;
    public Vector3 direction;
    public int waveType;

    [Header("Input Fields")]
    public TMP_InputField frequencyInput;
    public TMP_InputField amplitudeInput;
    public TMP_InputField directionXInput;
    public TMP_InputField directionYInput;
    public TMP_InputField directionZInput;
    public TMP_Dropdown waveTypeDropdown;

    [Header("Boat")]
    public Body boat;
    public TMP_InputField massInput;

    private float defaultFrequency;
    private float defaultAmplitude;
    private Vector3 defaultDirection;
    private int defaultWaveType;
    private float defaultMass;

    private void Awake()
    {
        instance = this;
        defaultFrequency = frequency;
        defaultAmplitude = amplitude;
        defaultDirection = direction;
        defaultWaveType = waveType;
    }

    private void Start()
    {
        if (boat != null) defaultMass = boat.mass;

        if (frequencyInput != null) {
            frequencyInput.text = frequency.ToString();
            frequencyInput.onEndEdit.AddListener(SetFrequency);
        }
        if (amplitudeInput != null) {
            amplitudeInput.text = amplitude.ToString();
            amplitudeInput.onEndEdit.AddListener(SetAmplitude);
        }
        if (directionXInput != null) {
            directionXInput.text = direction.x.ToString();
            directionXInput.onEndEdit.AddListener(SetDirectionX);
        }
        if (directionYInput != null) {
            directionYInput.text = direction.y.ToString();
            directionYInput.onEndEdit.AddListener(SetDirectionY);
        }
        if (directionZInput != null) {
            directionZInput.text = direction.z.ToString();
            directionZInput.onEndEdit.AddListener(SetDirectionZ);
        }
        if (waveTypeDropdown != null) {
            waveTypeDropdown.value = waveType;
            waveTypeDropdown.onValueChanged.AddListener(SetWaveType);
        }
        if (massInput != null && boat != null) {
            massInput.text = boat.mass.ToString();
            massInput.onEndEdit.AddListener(SetMass);
        }
    }

    public void SetFrequency(string value)
    {
        if (float.TryParse(value, out float result)) {
            frequency = result;
            onAnyParamEdited.Invoke();
        }
    }

    public void SetAmplitude(string value)
    {
        if (float.TryParse(value, out float result)) {
            amplitude = result;
            onAnyParamEdited.Invoke();
        }
    }

    public void SetDirectionX(string value)
    {
        if (float.TryParse(value, out float result)) {
            direction.x = result;
            onAnyParamEdited.Invoke();
        }
    }

    public void SetDirectionY(string value)
    {
        if (float.TryParse(value, out float result)) {
            direction.y = result;
            onAnyParamEdited.Invoke();
        }
    }

    public void SetDirectionZ(string value)
    {
        if (float.TryParse(value, out float result)) {
            direction.z = result;
            onAnyParamEdited.Invoke();
        }
    }

    public void SetWaveType(int value)
    {
        waveType = value;
        onAnyParamEdited.Invoke();
    }

    public void SetMass(string value)
    {
        if (boat != null && float.TryParse(value, out float result))
            boat.mass = result;
    }

    public void ResetParams()
    {
        frequency = defaultFrequency;
        amplitude = defaultAmplitude;
        direction = defaultDirection;
        waveType = defaultWaveType;

        if (boat != null)
        {
            boat.mass = defaultMass;
            boat.ResetBody();
        }

        if (frequencyInput != null) 
            frequencyInput.text = frequency.ToString();
        if (amplitudeInput != null) 
            amplitudeInput.text = amplitude.ToString();
        if (directionXInput != null) 
            directionXInput.text = direction.x.ToString();
        if (directionYInput != null) 
            directionYInput.text = direction.y.ToString();
        if (directionZInput != null) 
            directionZInput.text = direction.z.ToString();
        if (waveTypeDropdown != null) 
            waveTypeDropdown.value = waveType;
        if (massInput != null && boat != null)
            massInput.text = boat.mass.ToString();

        onAnyParamEdited.Invoke();
    }
}
