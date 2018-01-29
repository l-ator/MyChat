using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Infrastructure.Enum
{
    public enum ChatMessageStatus
    {
		Sending = 1,
		Sent = 2,
		Received = 3,
		Read = 4,
		Error = 5
    }
}
