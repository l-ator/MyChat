using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Infrastructure.Configuration.Settings
{
	public class MongoDbSettings
	{
		public string ConnectionString { get; set; }
		public bool ManageIndicies { get; set; }
		public MongoIdentitySettings IdentitySettings { get; set; }
		public MongoChatSettings ChatSettings { get; set; }
	}

	public class MongoIdentitySettings
	{
		public string DatabaseName { get; set; }
		public string UserCollectionName { get; set; }
		public string RoleCollectionName { get; set; }
	}

	public class MongoChatSettings
	{
		public string DatabaseName { get; set; }
		public string RoomsCollectionName { get; set; }
		public string MessagesCollectionName { get; set; }
	}
}
