//import { Toastr } from '../_common/util/index'






import { Injectable } from '@angular/core';
import { Observable, Observer, Subscription } from 'rxjs/Rx';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

import * as Stomp from '@stomp/stompjs';
import { StompSubscription } from '@stomp/stompjs';

import { ConfigService } from './config.service'

export type StompHeaders = { [key: string]: string };

export enum StompState {
	CLOSED,
	TRYING,
	CONNECTED,
	DISCONNECTING
}

@Injectable()
export class StompService {

	public state: BehaviorSubject<StompState>;

	public connectObservable: Observable<StompState>;

	protected queuedMessages: { queueName: string, message: string, headers: StompHeaders }[] = [];

	protected client: Stomp.Client;

	protected stompHeaders: StompHeaders = {
		login: this.config.rabbitMQSettings.username,
		passcode: this.config.rabbitMQSettings.password
	}

	public constructor(private config: ConfigService) {

		this.state = new BehaviorSubject<StompState>(StompState.CLOSED);

		this.connectObservable = this.state
			.filter((currentState: number) => {
				return currentState === StompState.CONNECTED;
			});

		// Setup sending queuedMessages
		this.connectObservable.subscribe(() => {
			this.sendQueuedMessages();
		});

		this.initStompClient();
		this.try_connect();
	}

	/** Initialize STOMP Client */
	protected initStompClient(): void {
		let hostUrl = this.config.stompSettings.hostUrl

		if (typeof (hostUrl) === 'string') 
			this.client = Stomp.client(hostUrl)
		else this.client = Stomp.over(hostUrl);

		// Configure client heart-beating
		this.client.heartbeat.incoming = this.config.stompSettings.heartbeatIn;
		this.client.heartbeat.outgoing = this.config.stompSettings.heartbeatOut;
		this.client.reconnect_delay = this.config.stompSettings.reconnectDelay;

		if (!this.config.stompSettings.debug) 
			this.debug = function () { };
		
		// Set function to debug print messages
		this.client.debug = this.debug;
	}


	/**
	 * Perform connection to STOMP broker
	 */
	protected try_connect(): void {

		// Attempt connection, passing in a callback
		this.client.connect(
			this.stompHeaders,
			this.on_connect,
			this.on_error
		);

		this.debug('Connecting...');
		this.state.next(StompState.TRYING);
	}

	public disconnect(): void {

		// Disconnect if connected. Callback will set CLOSED state
		if (this.client && this.client.connected) {
			// Notify observers that we are disconnecting!
			this.state.next(StompState.DISCONNECTING);

			this.client.disconnect(
				() => this.state.next(StompState.CLOSED)
			);
		}
	}

	public connected(): boolean {
		return this.state.getValue() === StompState.CONNECTED;
	}

	public publish(queueName: string, message: string, headers: StompHeaders = {}): void {
		if (this.connected()) {
			this.client.send(queueName, headers, message);
		} else {
			this.debug(`Not connected, queueing ${message}`);
			this.queuedMessages.push({ queueName: <string>queueName, message: <string>message, headers: headers });
		}
	}

	protected sendQueuedMessages(): void {
		const queuedMessages = this.queuedMessages;
		this.queuedMessages = [];

		this.debug(`Will try sending queued messages ${queuedMessages}`);

		for (const queuedMessage of queuedMessages) {
			this.debug(`Attempting to send ${queuedMessage}`);
			this.publish(queuedMessage.queueName, queuedMessage.message, queuedMessage.headers);
		}
	}

	public consume(queueName: string, headers: StompHeaders = {}): Observable<Stomp.Message> {

		this.debug(`Request to subscribe ${queueName}`);

		// By default auto acknowledgement of messages
		if (!headers['ack']) {
			headers['ack'] = 'auto';
		}

		const coldObservable = Observable.create(
			(messages: Observer<Stomp.Message>) => {
				/*
				 * These variables will be used as part of the closure and work their magic during unsubscribe
				 */
				let stompSubscription: StompSubscription;

				let stompConnectedSubscription: Subscription;

				stompConnectedSubscription = this.connectObservable
					.subscribe(() => {
						this.debug(`Will subscribe to ${queueName}`);
						stompSubscription = this.client.subscribe(queueName, (message: Stomp.Message) => {
							messages.next(message);
						},
							headers);
					});

				return () => { /* cleanup function, will be called when no subscribers are left */
					this.debug(`Stop watching connection state (for ${queueName})`);
					stompConnectedSubscription.unsubscribe();

					if (this.state.getValue() === StompState.CONNECTED) {
						this.debug(`Will unsubscribe from ${queueName} at Stomp`);
						stompSubscription.unsubscribe();
					} else {
						this.debug(`Stomp not connected, no need to unsubscribe from ${queueName} at Stomp`);
					}
				};
			});

		return coldObservable.share();
	}



	protected debug = (args: any): void => {
		console.log(args);
	}

	/** Callback run on successfully connecting to server */
	protected on_connect = () => {

		this.debug('Connected');

		// Indicate our connected state to observers
		this.state.next(StompState.CONNECTED);
	}

	/** Handle errors from stomp.js */
	protected on_error = (error: string | Stomp.Message) => {

		if (typeof error === 'object') {
			error = (<Stomp.Message>error).body;
		}

		this.debug(`Error: ${error}`);

		// Check for dropped connection and try reconnecting
		if (!this.client.connected) {
			// Reset state indicator
			this.state.next(StompState.CLOSED);
		}
	}

	public unsubscribe(subId:string) { (<any>this.client.unsubscribe)(subId) }

}