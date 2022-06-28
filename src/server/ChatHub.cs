using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fantasy.Logic.ServerShared;
using MagicOnion.Server.Hubs;



namespace ChatApp.Server
{
    /// <summary>
    /// Chat server processing.
    /// One class instance for one connection.
    /// </summary>
    public class ChatHub : StreamingHubBase<IChatHub, IChatHubReceiver>, IChatHub
    {
        private IGroup _room;
        private string _name;

        public async Task JoinAsync(JoinRequest request)
        {
            _room = await Group.AddAsync(request.RoomName);
            _name = request.UserName;
            Broadcast(_room).OnJoin(request.UserName);
        }
        public async Task LeaveAsync()
        {
            await _room.RemoveAsync(Context);
            Broadcast(_room).OnLeave(_name);
        }

        public async Task SendMessageAsync(string message)
        {
            var response = new MessageResponse {UserName = _name, Message = message};
            Broadcast(_room).OnSendMessage(response);
            await Task.CompletedTask;
        }
        public Task GenerateException(string message)
        {
            throw new Exception(message);
        }

        // It is not called because it is a method as a sample of arguments.
        public Task SampleMethod(List<int> sampleList, Dictionary<int, string> sampleDictionary)
        {
            throw new NotImplementedException();
        }
        protected override ValueTask OnConnecting()
        {
            // handle connection if needed.
            Console.WriteLine($"client connected {Context.ContextId}");
            return CompletedTask;
        }

        protected override ValueTask OnDisconnected()
        {
            // handle disconnection if needed.
            // on disconnecting, if automatically removed this connection from group.
            return CompletedTask;
        }
    }
}