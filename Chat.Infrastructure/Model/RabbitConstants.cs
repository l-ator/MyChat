using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Infrastructure.Model
{
    public static class RabbitConst
    {
		public const string MessageExchange = "message_exchange";
		public const string StatuUpdateExchange = "status_update_exchange";
		public const string MessagePersistorQueue = "message_persistor_queue";
		public const string StatusPersistorQueue = "status_persistor_queue";
		public const string IdHeader = "id";
		public const string RoomIdHeader = "room";
		public const string FromHeader = "from";
		public const string ToHeader = "to";
		public const string TimestampHeader = "timestamp";
		public const string StatusHeader = "status";
	}
}
