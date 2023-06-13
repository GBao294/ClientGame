using Riptide;
using Riptide.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

public enum ServerToClientId : ushort
{
    sync = 1,
    activeScene,
    playerSpawned,
    EnemySpawned,
    playerMovement,
    EnemyMovement,
    EnemySpin,
    message,
    shootNotification,
    playerHealthChanged,
    EnemyHealthChanged,
    playerDied,
    EnemyDied,
    playerRespawned,
    EnemyRespawned,


}

public enum ClientToServerId : ushort
{
    name = 1,
    GetEnemy,
    input,
    shoot,
    chat,

}


public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;
    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public Client Client { get; private set; }

    private ushort _serverTick;
    public ushort ServerTick
    {
        get => _serverTick;
        private set
        {
            _serverTick = value;
            InterpolationTick = (ushort)(value - TicksBetweenPositionUpdates);
        }
    }
    public ushort InterpolationTick { get; private set; }


    private ushort _ticksBetweenPositionUpdates = 2;
    public ushort TicksBetweenPositionUpdates
    {
        get => _ticksBetweenPositionUpdates;
        private set
        {
            _ticksBetweenPositionUpdates = value;
            InterpolationTick = (ushort)(ServerTick - value);
        }
    }
    private void SetTick(ushort serverTick)
    {
        if (Mathf.Abs(ServerTick - serverTick) > tickDivergenceTolerance)
        {
            Debug.Log($"Client tick: {ServerTick} -> {serverTick}");
            ServerTick = serverTick;
        }
    }

    [SerializeField] private InputField ip;
   
    [SerializeField] private ushort port;
    [Space(10)]
    [SerializeField] private ushort tickDivergenceTolerance = 1;

    APIScript apiscript;
 
    private void Awake()
    {
        Singleton = this;
    }

   
    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Client = new Client();
        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailedToConnect;
        Client.ClientDisconnected += PlayerLeft;
        Client.Disconnected += DidDisconnect;

        ServerTick = TicksBetweenPositionUpdates;
        if (apiscript == null)
            apiscript = GetComponent<APIScript>();
    }

    private void FixedUpdate()
    {
        Client.Update();
        ServerTick++;
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }

    public async void Connect()
    {
      
            Client.Connect($"{ip.text}:{port}");
      
    }

    private void DidConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.SendName();
    }

    private void FailedToConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.BackToMain();
    }

    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        if (Player.list.TryGetValue(e.Id, out Player player))
            Destroy(player.gameObject);
    }

    private void DidDisconnect(object sender, EventArgs e)
    {
        UIManager.Singleton.BackToMain();
        foreach (Player player in Player.list.Values)
            Destroy(player.gameObject);

        //GameLogic.Singleton.UnloadActiveScene();
    }

    

    [MessageHandler((ushort)ServerToClientId.sync)]
    public static void Sync(Message message)
    {
        Singleton.SetTick(message.GetUShort());
    }
}