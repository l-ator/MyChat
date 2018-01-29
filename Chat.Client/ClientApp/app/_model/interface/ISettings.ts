export interface IAMQPSettings {
	amQPUrl: string;
	username: string;
	password: string;
}

export interface IRabbitMQSettings {
	hostUrl: string;
	username: string;
	password: string;
}

export interface IStompSettings {
	hostUrl: string;
	heartbeatIn: number
	heartbeatOut: number
	reconnectDelay: number
	debug: boolean
}

export interface IMongoIdentitySettings {
	databaseName: string;
	usersCollection: string;
	rolesCollection: string;
}

export interface IMongoDbSettings {
	connectionString: string;
	manageIndicies: boolean;
	mongoIdentitySettings: IMongoIdentitySettings;
}
