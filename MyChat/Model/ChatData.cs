using System;
using System.Collections.Generic;
using System.Text;

namespace MyChat.Model
{
    [Serializable]
    public class ChatData
    {
        public string NickName { get; set; }
        public string Message { get; set; }
        public byte[] Data { get; set; }
    }
}
