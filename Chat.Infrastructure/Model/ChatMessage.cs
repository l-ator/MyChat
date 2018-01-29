using Chat.Infrastructure.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Infrastructure.Model
{
	public class ChatMessage : IEntity
	{
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		[BsonElement("roomId")]
		public string RoomId { get; set; }
		[BsonElement("body")]
		public string Body { get; set; }
		[BsonElement("from")]
		public string From { get; set; }
		[BsonElement("to")]
		public string To { get; set; }
		[BsonElement("timestamp")]
		public DateTime Timestamp { get; set; }
		[BsonElement("status")]
		public ChatMessageStatus Status { get; set; }
	}
}
