import { Injectable, EventEmitter, OnInit } from '@angular/core'
import { Http, Headers } from '@angular/http'
import { StompHeaders, Message } from '@stomp/stompjs'

import { AuthService } from './auth.service'
import { StompService } from './stomp.service'

import { Helpers, Toastr } from '../_common/index'

import {
	IChatMessage,
	IChatRoom,
	IStatusUpdate
} from '../_model/interface/index'
import { ChatMessageStatus } from '../_model/enum/index'

import * as RXJS from 'rxjs'

const MESSAGE_EXCHANGE = "/exchange/message_exchange"
const STATUS_UPDATE_EXCHANGE = "/exchange/status_update_exchange"
const INBOX_SUB_ID = "inbox-subscription"
const STATUS_UPDATE_SUB_ID = "status-notify-subscription"

declare let escape


@Injectable()
export class ChatMessageService {

	messageReceived: EventEmitter<IChatMessage> = new EventEmitter<IChatMessage>()
	statusUpdated: EventEmitter<IStatusUpdate> = new EventEmitter<IStatusUpdate>()

	constructor(
		private auth: AuthService,
		private http: Http,
		private stompService: StompService
	) { }

	ngOnInit() {
	}

	send(room: IChatRoom, body: string, id?: string, session?: string) {

		console.log(room, body, id)

		return new Promise<IChatMessage>(resolve => {

			let headers = new Headers()
			headers.append("Content-Type", "application/json")

			let data = { room: room, body: body, id: id, session: session }

			let obs = this.http.post("ChatMessage/SendMessage", data, { headers: headers })
				.map(resp => <IChatMessage>resp.json())
			obs.subscribe(result => resolve(result), e => Toastr.error("Internal Server Error"))
		})
	}

	updateMessagesStatus(ids: string | string[], status: ChatMessageStatus) {

		let toUpdate: string[] = []
		if (!Array.isArray(ids))
			toUpdate.push(ids)
		else ids.forEach(id => toUpdate.push(id))

		return new Promise<void>(resolve => {


			let headers = new Headers()
			headers.append("Content-Type", "application/json")

			let data = { ids: toUpdate, status: status }

			let obs = this.http.post("ChatMessage/UpdateMessageStatus", data, { headers: headers })
				.map(resp => resp.json())
			obs.subscribe(result => resolve(), e => Toastr.error("Internal Server Error"))
		})


	}

	consumeInbox() {

		let bindingKeys = [`#.${this.auth.currentUser.username}.#`]
		let msgExchange = MESSAGE_EXCHANGE

		bindingKeys.forEach(bk => this.stompService.consume(`${msgExchange}/${bk}`, { "ack": "auto", "id": INBOX_SUB_ID })
			.subscribe((message: Message) => {

				console.log("ASD", message.headers["greeting"])

				let msg: IChatMessage = <any>{
					id: message.headers["id"],
					from: message.headers["from"],
					to: message.headers["to"],
					roomId: message.headers["room"],
					timestamp: new Date(Number(message.headers["timestamp"]) * 1000),
					status: ChatMessageStatus.Sent,
					body: message.body,
					greeting: message.headers["greeting"] ? true : false,
					session: message.headers["session"]
				}

				this.messageReceived.emit(msg)
			}))
	}

	consumeStatusUpdates() {
		let bindingKeys = [`#.${this.auth.currentUser.username}.#`]
		let statusExchange = STATUS_UPDATE_EXCHANGE

		bindingKeys.forEach(bk => this.stompService.consume(`${statusExchange}/${bk}`, { "ack": "auto", "id": STATUS_UPDATE_SUB_ID })
			.subscribe((message: Message) => {

				let update: IStatusUpdate = {
					id: message.headers["id"],
					roomId: message.headers["room"],
					status: <ChatMessageStatus>Number(message.headers["status"])
				}

				this.statusUpdated.emit(update)
			}))
	}

	stopConsuming() {
		this.stompService.unsubscribe(INBOX_SUB_ID)
		this.stompService.unsubscribe(STATUS_UPDATE_SUB_ID)
	}
}