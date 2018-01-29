import { Injectable } from '@angular/core'

import { Http } from '@angular/http'

import {
	IAMQPSettings,
	IRabbitMQSettings,
	IMongoDbSettings,
	IStompSettings
} from '../_model/interface/index'

import * as Rxjs from 'rxjs'

@Injectable()
export class ConfigService {

	amqpSettings: IAMQPSettings
	rabbitMQSettings: IRabbitMQSettings
	mongoDBSettings: IMongoDbSettings
	stompSettings: IStompSettings

	constructor(private http: Http) { }

	initConfig() {
		return Rxjs.Observable.forkJoin(
			this.getAMQPSettings().then(amqpSettings => this.amqpSettings = amqpSettings),
			this.getRabbitMQSettings().then(rabbitMQSettings => this.rabbitMQSettings = rabbitMQSettings),
			this.getMongoDBSettings().then(mongoDBSettings => this.mongoDBSettings = mongoDBSettings),
			this.getStompSettings().then(stompSettings => this.stompSettings = stompSettings),
		)
	}

	getAMQPSettings() {
		return new Promise<IAMQPSettings>(resolve => {
			let obs = this.http.get("Config/GetAMQPSettings")
				.map(resp => <IAMQPSettings>resp.json())
			obs.subscribe(result => { this.amqpSettings = result; resolve(result) }, e => console.error(e))
		})
	}

	getRabbitMQSettings() {
		return new Promise<IRabbitMQSettings>(resolve => {
			let obs = this.http.get("Config/GetRabbitMQSettings")
				.map(resp => <IRabbitMQSettings>resp.json())
			obs.subscribe(result => { this.rabbitMQSettings = result; resolve(result) }, e => console.error(e))
		})
	}

	getStompSettings() {
		return new Promise<IStompSettings>(resolve => {
			let obs = this.http.get("Config/GetStompSettings")
				.map(resp => <IStompSettings>resp.json())
			obs.subscribe(result => { this.stompSettings = result; resolve(result) }, e => console.error(e))
		})
	}

	getMongoDBSettings() {
		return new Promise<IMongoDbSettings>(resolve => {
			let obs = this.http.get("Config/GetMongoDBSettings")
				.map(resp => <IMongoDbSettings>resp.json())
			obs.subscribe(result => { this.mongoDBSettings = result; resolve(result) }, e => console.error(e))
		})
	}






}