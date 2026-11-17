using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AA1Params : MonoBehaviour
{
    public static AA1Params instance;

    public UnityEvent onAnyParamEdited;

    public float G = 39.478f;

    [Header("Input Fields")]
    public TMP_InputField gInput;

    private float defaultG;

    private void Awake()
    {
        instance = this;
        defaultG = G;
    }

    private void Start()
    {
        if (gInput != null)
        {
            gInput.text = G.ToString();
            gInput.onEndEdit.AddListener(SetG);
        }
    }

    public void SetG(string value)
    {
        if (float.TryParse(value, out float result))
        {
            G = result;
            onAnyParamEdited.Invoke();
        }
    }

    public void ResetParams()
    {
        G = defaultG;
        if (gInput != null) 
            gInput.text = G.ToString();
        onAnyParamEdited.Invoke();
    }
}
