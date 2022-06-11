using System;
using ChatApp.Shared.Hubs;
using ChatApp.Shared.MessagePackObjects;
using ChatApp.Shared.Services;
using Grpc.Core;
using MagicOnion.Client;
using System.Threading;
using System.Threading.Tasks;
using MagicOnion;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ChatComponent : MonoBehaviour, IChatHubReceiver
    {
        private readonly CancellationTokenSource _shutdownCancellation = new();
        private ChannelBase _channel;
        private IChatHub _streamingClient;
        private IChatService _client;
        private bool _isJoin;
        private bool _isSelfDisConnected;

        public Text ChatText;
        public Button JoinOrLeaveButton;

        public Text JoinOrLeaveButtonText;

        public Button SendMessageButton;

        public InputField Input;

        public InputField ReportInput;

        public Button SendReportButton;

        public Button DisconnectButton;
        public Button ExceptionButton;
        public Button UnaryExceptionButton;


        private async void Start()
        {
            await InitializeClientAsync();
            InitializeUi();
        }

        private async void OnDestroy()
        {
            // Clean up Hub and channel
            _shutdownCancellation.Cancel();
            if (_streamingClient != null) await _streamingClient.DisposeAsync();
            if (_channel != null) await _channel.ShutdownAsync();
        }
        
        private async Task InitializeClientAsync()
        {
            // Initialize the Hub
            // NOTE: If you want to use SSL/TLS connection, see InitialSettings.OnRuntimeInitialize method.
            _channel = GrpcChannelx.ForAddress("http://localhost:5000");
            while (!_shutdownCancellation.IsCancellationRequested)
            {
                try
                {
                    Debug.Log($"Connecting to the server...");
                    _streamingClient = await StreamingHubClient.ConnectAsync<IChatHub, IChatHubReceiver>(_channel, 
                        this, cancellationToken: _shutdownCancellation.Token);
                    RegisterDisconnectEvent(_streamingClient);
                    Debug.Log($"Connection is established.");
                    break;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                Debug.Log($"Failed to connect to the server. Retry after 5 seconds...");
                await Task.Delay(5 * 1000);
            }
            _client = MagicOnionClient.Create<IChatService>(_channel);
        }
        private async void RegisterDisconnectEvent(IChatHub streamingClient)
        {
            try
            {
                // you can wait disconnected event
                await streamingClient.WaitForDisconnect();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                // try-to-reconnect? logging event? close? etc...
                Debug.Log($"disconnected from the server.");

                if (_isSelfDisConnected)
                {
                    // there is no particular meaning
                    await Task.Delay(2000);
                    // reconnect
                    await ReconnectServerAsync();
                }
            }
        }

        private void InitializeUi()
        {
            _isJoin = false;
            SendMessageButton.interactable = false;
            ChatText.text = string.Empty;
            Input.text = string.Empty;
            Input.placeholder.GetComponent<Text>().text = "Please enter your name.";
            JoinOrLeaveButtonText.text = "Enter the room";
            ExceptionButton.interactable = false;
        }


 

        public async void DisconnectServer()
        {
            _isSelfDisConnected = true;

            JoinOrLeaveButton.interactable = false;
            SendMessageButton.interactable = false;
            SendReportButton.interactable = false;
            DisconnectButton.interactable = false;
            ExceptionButton.interactable = false;
            UnaryExceptionButton.interactable = false;
            if (_isJoin)
                JoinOrLeave();
            await _streamingClient.DisposeAsync();
        }

        public async void ReconnectInitializedServer()
        {
            if (this._channel != null)
            {
                var chan = this._channel;
                if (chan == Interlocked.CompareExchange(ref this._channel, null, chan))
                {
                    await chan.ShutdownAsync();
                    this._channel = null;
                }
            }
            if (this._streamingClient != null)
            {
                var streamClient = this._streamingClient;
                if (streamClient == Interlocked.CompareExchange(ref this._streamingClient, null, streamClient))
                {
                    await streamClient.DisposeAsync();
                    this._streamingClient = null;
                }
            }

            if (this._channel == null && this._streamingClient == null)
            {
                await this.InitializeClientAsync();
                this.InitializeUi();
            }
        }


        private async Task ReconnectServerAsync()
        {
            Debug.Log($"Reconnecting to the server...");
            this._streamingClient = await StreamingHubClient.ConnectAsync<IChatHub, IChatHubReceiver>(this._channel, this);
            this.RegisterDisconnectEvent(_streamingClient);
            Debug.Log("Reconnected.");

            this.JoinOrLeaveButton.interactable = true;
            this.SendMessageButton.interactable = false;
            this.SendReportButton.interactable = true;
            this.DisconnectButton.interactable = true;
            this.ExceptionButton.interactable = true;
            this.UnaryExceptionButton.interactable = true;

            this._isSelfDisConnected = false;
        }


        #region Client -> Server (Streaming)
        public async void JoinOrLeave()
        {
            if (_isJoin)
            {
                await _streamingClient.LeaveAsync();
                InitializeUi();
            }
            else
            {
                var request = new JoinRequest { RoomName = "SampleRoom", UserName = Input.text };
                await _streamingClient.JoinAsync(request);

                _isJoin = true;
                SendMessageButton.interactable = true;
                JoinOrLeaveButtonText.text = "Leave the room";
                Input.text = string.Empty;
                Input.placeholder.GetComponent<Text>().text = "Please enter a comment.";
                ExceptionButton.interactable = true;
            }
        }


        public async void SendMessage()
        {
            if (!_isJoin)
                return;
            await _streamingClient.SendMessageAsync(Input.text);
            Input.text = string.Empty;
        }

        public async void GenerateException()
        {
            // hub
            if (!_isJoin) return;
            await _streamingClient.GenerateException("client exception(streaminghub)!");
        }

        public void SampleMethod()
        {
            throw new System.NotImplementedException();
        }
        #endregion


        #region Server -> Client (Streaming)
        public void OnJoin(string name)
        {
            this.ChatText.text += $"\n<color=grey>{name} entered the room.</color>";
        }


        public void OnLeave(string name)
        {
            this.ChatText.text += $"\n<color=grey>{name} left the room.</color>";
        }

        public void OnSendMessage(MessageResponse message)
        {
            this.ChatText.text += $"\n{message.UserName}：{message.Message}";
        }
        #endregion


        #region Client -> Server (Unary)
        public async void SendReport()
        {
            await _client.SendReportAsync(ReportInput.text);
            ReportInput.text = string.Empty;
        }

        public async void UnaryGenerateException()
        {
            // unary
            await this._client.GenerateException("client exception(unary)！");
        }
        #endregion
    }
}
