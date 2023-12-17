using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickPointButton);
    }

    private void Update()
    {
        if (_text.text != "")
        {
            GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }

    private void OnClickPointButton()
    {
        GameManager.Instance.Step3Success = true;
    }
}
