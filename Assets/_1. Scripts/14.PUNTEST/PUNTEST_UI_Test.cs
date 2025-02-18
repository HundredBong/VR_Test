using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PUNTEST_UI_Test : MonoBehaviour
{
    private Button button;
    private int i = 1;
    public TextMeshProUGUI text;

    private void Awake()
    {
        button = GetComponent<Button>();    
    }

    private void OnEnable()
    {
        button.onClick.AddListener(Logging);
    }

    public void Logging()
    {
        Debug.Log($"VR³¤ Âðµû´Â ¸øº¸´Â ·Î±× {i}Æ®");
        text.text = $"VR³¤ Âðµû´Â ¸øº¸´Â ·Î±× {i}Æ®";
        i++;
    }
}
