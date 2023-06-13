using Riptide;
using Riptide.Utils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static APIScript;
//using static UnityEditor.ShaderData;
//using UnityEditor.Experimental.GraphView;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;
    public static UIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    [Header("Connect")]
    [SerializeField] private GameObject connectUI;
    [SerializeField] private Text text;
   
    [SerializeField] private InputField usernameField;
    APIScript apiscript;
    [SerializeField] private InputField passwordField;
   
    private void Awake()
    {
        Singleton = this;
    }
    private void Start()
    {
        if (apiscript == null)
            apiscript = GetComponent<APIScript>();
    }
    public async void ConnectClicked()
    {
       
        bool result = await apiscript.GET(usernameField.text, passwordField.text);

        if (result)
        {
            usernameField.interactable = false;
            passwordField.interactable = false;
            connectUI.SetActive(false);
            //SceneManager.LoadScene(2);
            NetworkManager.Singleton.Connect();
        }
        else text.text = "KET NOI KHONG THANH CONG";

    }

    public async void RegisterClicked()
    {

        bool result = await apiscript.SendNewData(usernameField.text, passwordField.text);
        if (result)
        {
           text.text = "DANG KI THANG CONGGGG";
        }
        else
        {
            text.text = "DANG KI THAT BAI";
        }

    }

    public void BackToMain()
    {
        usernameField.interactable = true;
        connectUI.SetActive(true);
    }

    public void SendName()
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.name);
        message.AddString(usernameField.text);
        NetworkManager.Singleton.Client.Send(message);

        Message message1 = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.GetEnemy);
        NetworkManager.Singleton.Client.Send(message1);
    }
    public void HealthUpdated(float health, float maxHealth)
    {
        GetComponent<Health>().healthSlider.value = health / maxHealth;

    }
}
