using Chat.Infrastructure.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Infrastructure.Model
{
	public class ChatRoom : IEntity
	{
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		[BsonElement("users")]
		public List<string> Users { get; set; }
		[BsonElement("messages")]
		public List<ChatMessage> Messages { get; set; }
		[BsonElement("lastMessageTime")]
		public DateTime LastMessageTime { get; set; }
		[BsonElement("type")]
		public ChatRoomType Type { get; set; }
		[BsonElement("name")]
		public string Name { get; set; }
	}
}
