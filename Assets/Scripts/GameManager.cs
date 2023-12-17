using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameStep
{
    None,
    Step1,
    Step2,
    Step3
}
[System.Serializable]
public class GameData
{
    public string member_id;
    public string kiosk_category_id;
    public string play_date;
    public int play_stage;
    public int play_time;
    public int is_success;
    public int is_game;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int TotalCount;
    public int TotalPrice;

    public bool Step3Success;

    private GameStep _gameStep;
    private int playTime;
    private Stopwatch sw;
    private bool isSuccess;
    private bool _saveData;
    private GameData _gameData;
    private string _sceneNameType;

    [SerializeField] private GameObject _finishUI;
    [SerializeField] private TextMeshProUGUI _playTimeTxt;

    [SerializeField] private GameObject TotalCountText;
    [SerializeField] private GameObject TotalPriceText;
    [SerializeField] private Button _allClearButton;
    [SerializeField] private Button _payButton;
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private GameObject _successPanel;
    [SerializeField] private GameObject _failPanel;

    private void Awake()
    {
        Instance = this;
        _gameData = new GameData();
    }
    private void Start()
    {
        Application.ExternalCall("unityFunction", _gameData.member_id);

        _gameData.play_date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

        _allClearButton.onClick.AddListener(OnClickAllClearButton);

        //Game
        string sceneName = SceneManager.GetActiveScene().name;
        _sceneNameType = sceneName.Substring(5, 5);

        _gameData.kiosk_category_id = sceneName.Substring(0, 4);
        UnityEngine.Debug.Log(sceneName.Substring(9, 1));
        _gameData.play_stage = int.Parse(sceneName.Substring(9, 1));

        if (_sceneNameType.StartsWith("Prac"))
        {
            _gameData.is_game = 0;
        }
        else if (_sceneNameType.StartsWith("Test"))
        {
            _gameData.is_game = 1;
        }
        _gameStep = (GameStep)char.GetNumericValue(sceneName[sceneName.Length - 1]);

        sw = new Stopwatch();
        sw.Start();

    }

    private void Update()
    {

        if (_finishUI.activeSelf == true)
        {
            sw.Stop();

            switch (_gameStep)
            {
                case GameStep.Step1:
                    isSuccess = (TotalCount > 0);
                    break;
                case GameStep.Step2:
                    isSuccess = (TotalCount > 1);
                    break;
                case GameStep.Step3:
                    isSuccess = Step3Success;
                    break;
            }


            if (isSuccess)
            {
                _successPanel.SetActive(true);
            }
            else
            {
                _failPanel.SetActive(true);
            }
            _gameData.is_success = Convert.ToInt32(isSuccess);
            if (!_saveData)
            {
                SaveData(); //끝나면 정보 보내기

            }

                
        }

        

        // �ð� ���
        if (_playTimeTxt != null)
        {
            playTime = (int)sw.ElapsedMilliseconds / 1000;
            int minutes;
            int seconds;

            minutes = playTime / 60;
            seconds = playTime % 60;

            if (minutes > 0)
            {
                _playTimeTxt.text = "소요 시간 : " + minutes.ToString() + "분 " + seconds.ToString() + "초";
            }
            else
            {
                _playTimeTxt.text = "소요 시간 : " + seconds.ToString() + "초";
            }

            _gameData.play_time = playTime;
        }
    }

    public void UpdateText(int count, int price)
    {
        TotalCount += count;
        TotalPrice += price;
        TotalCountText.GetComponent<TextMeshProUGUI>().text = TotalCount.ToString();
        TotalPriceText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#,##0}",TotalPrice);

        if (count > 0)
        {
            _payButton.interactable = true;
        }
        else
        {
            _payButton.interactable = false;
        }
    }

    public void ReceiveData(string message)
    {
        _gameData.member_id = message;
        UnityEngine.Debug.Log("Received message from JavaScript: " + message);
    }

    public void SetQuit()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private void SaveData()
    {
        _saveData = true;
        //����ȭ
        string jsonData = JsonUtility.ToJson(_gameData);

        string url = "https://003operation.shop/kiosk/insertData";

        StartCoroutine(SendDataToWeb(jsonData, url));
    }

    private IEnumerator SendDataToWeb(string jsonData, string url)
    {
        // �����͸� ����Ʈ �迭�� ��ȯ
        byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // POST ��û ������
        UnityWebRequest www = UnityWebRequest.PostWwwForm(url, "POST");
        www.uploadHandler = new UploadHandlerRaw(dataBytes);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("withCredentials", "true");

        // ��û ������ �� ���� ��ٸ���
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            UnityEngine.Debug.LogError("Failed to send data to the web server: " + www.error);
        }
        else
        {
            UnityEngine.Debug.Log("Data sent successfully!");
            // ���������� ������ www.downloadHandler.text ���� ���� Ȯ���� �� �ֽ��ϴ�.
            SetQuit();
        }
    }

    private void OnClickAllClearButton()
    {
        foreach(Transform child in _spawnPoint)
        {
            child.GetComponent<ClickAddItem>().OnClickDeleteButton();
            Destroy(child.gameObject);
        }
    }
}