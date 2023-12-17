using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickAddItem : MonoBehaviour
{
    private int price;

    [SerializeField] private Button _minusButton;
    [SerializeField] private Button _plusButton;
    [SerializeField] private Button _deleteButton;
    // Start is called before the first frame update
    void Start()
    {
        string str = transform.GetChild(5).GetComponent<TextMeshProUGUI>().text;
        price = int.Parse(str.Substring(0, str.Length - 1).Replace(",", ""));
        _minusButton.onClick.AddListener(OnClickMinusButton);
        _plusButton.onClick.AddListener(OnClickPlusButton);
        _deleteButton.onClick.AddListener(OnClickDeleteButton);
    }

    private void OnClickMinusButton()
    {
        int count = int.Parse(transform.GetChild(4).GetComponent<TextMeshProUGUI>().text);
        count--;
        if (count == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = count.ToString();
            transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = string.Format("{0:#,##0}¿ø", (count * price));
        }
        GameManager.Instance.UpdateText(-1, -price);
    }

    public void OnClickPlusButton()
    {
        int count = int.Parse(transform.GetChild(4).GetComponent<TextMeshProUGUI>().text);
        count++;
        transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = count.ToString();
        transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = string.Format("{0:#,##0}¿ø", (count * price)) ;
        GameManager.Instance.UpdateText(1, price);
    }

    public void OnClickDeleteButton()
    {
        int count = int.Parse(transform.GetChild(4).GetComponent<TextMeshProUGUI>().text);
        int price = int.Parse(transform.GetChild(5).GetComponent<TextMeshProUGUI>().text.Replace(",","").Replace("¿ø",""));
        GameManager.Instance.UpdateText(-count, -price);
        Destroy(gameObject);
    }
}
