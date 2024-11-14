using UnityEngine;

namespace Contracts
{
    public class MessageDto
    {
        public MessageDto(string message, Color color = default)
        {
            this.message = message;
            this.color = color;
        }

        public Color color { get; set; } = Color.yellow;    
        public string message { get; set; }
    }
}
