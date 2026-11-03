using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GeneralParams : MonoBehaviour
{
    public static GeneralParams instance;
    public UnityEvent onAnyParamEdited;

    public float timeMultiplier = 1f;

    [Header("Input Fields")]
    public TMP_InputField timeInput;

    private float defaultTimeMultiplier;

    private void Awake()
    {
        instance = this;
        defaultTimeMultiplier = timeMultiplier;
    }

    private void Start()
    {
        if (timeInput != null)
        {
            timeInput.text = timeMultiplier.ToString();
            timeInput.onEndEdit.AddListener((string s)=>{ SetTime(s); });
        }
    }

    public void SetTime(string value)
    {
        if (float.TryParse(value, out float result))
        {
            timeMultiplier = result;
            onAnyParamEdited.Invoke();
        }
    }

    public void ResetParams()
    {
        timeMultiplier = defaultTimeMultiplier;
        if (timeInput != null) 
            timeInput.text = timeMultiplier.ToString();
        onAnyParamEdited.Invoke();
    }
}
