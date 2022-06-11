using MessagePack;

namespace ChatApp.Shared.MessagePackObjects
{
  
    /// <summary>
    /// Message information
    /// </summary>
    [MessagePackObject]
    public struct Test1
    {
        [Key(0)]
        public string UserName { get; set; }

        [Key(1)]
        public string Message { get; set; }
    }
}