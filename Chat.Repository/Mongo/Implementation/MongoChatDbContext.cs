﻿using MongoDB.Driver;
using System;
using Microsoft.Extensions.Options;
using Chat.Infrastructure.Configuration.Settings;
using OIA.Common.Repository.Mongo.Interface;

namespace Chat.Repository.Mongo.Implementation
{
	public class MongoChatDbContext : IMongoChatDbContext
	{

		public IMongoDatabase MongoDatabase { get { return _db; } }

		private IMongoDatabase _db = null;
		private IMongoClient _client = null;


		public MongoChatDbContext(IOptions<MongoDbSettings> options)
		{
			try
			{
				var mongoUrl = new MongoUrl(options.Value.ConnectionString);
				_client = new MongoClient(mongoUrl);
				_db = this._client.GetDatabase(options.Value.ChatSettings.DatabaseName);
			}
			catch (MongoDB.Driver.MongoConnectionException mongoEx)
			{
				throw mongoEx;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}