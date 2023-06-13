using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Riptide;
using Riptide.Utils;

public class ChatManager : MonoBehaviour
{
    public GameObject BoxChat;
    public Text chatText;
    static Stack<string> chat = new Stack<string>();
    public static ChatManager instance;
    public InputField inputField;
    static int maxMessage = 15;
    static Stack<string> msg = new Stack<string>();
    bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //BoxChat.SetActive(false);
  
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && Input.GetKey(KeyCode.LeftControl))
        {

            if (active == false)
            {
                BoxChat.SetActive(true);
                active = true;
            }
            else
            {
                BoxChat.SetActive(false);
                active = false;
            }
        }
        if (msg.Count > 0)
        {
            chat.Push(msg.Pop());
            if (chat.Count > maxMessage) chat.Pop();
            chatText.text = string.Join("\n", chat);
        }
    }


    public void SendMessageFromUI(string msg)
    {
        inputField.text = "";
       // AddChat(msg);
        SendMessageToServer(msg);
    }

    void SendMessageToServer(string text)
    {
        Message message = Message.Create(MessageSendMode.Unreliable, (ushort)ClientToServerId.chat);
        message.AddString(text);
        NetworkManager.Singleton.Client.Send(message);
    }



    [MessageHandler((ushort)ServerToClientId.message)]
    private static void ReceiveMessage(Message message)
    {
       // AddChat(message.GetString());
       msg.Push(message.GetString());
    }
}
