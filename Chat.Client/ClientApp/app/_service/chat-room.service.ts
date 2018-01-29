import { Injectable, EventEmitter, OnInit } from '@angular/core'

import { Http, Headers } from '@angular/http'

import { StompService } from './stomp.service'
import { StompHeaders, Message } from '@stomp/stompjs'
import { AuthService } from './auth.service'
import { Helpers, Toastr } from '../_common/index'

import {
	IChatRoom,
	IChatMessage,
	IOpResult
} from '../_model/interface/index'

const serverError: string = "Internal Server Error"
const inboxQueueFormat = "/queue/inbox_{0}"
const messageExchangeFormat = "/exchange/message_exchange/from.{0}.to.{1}"
const notificationQueueFormat = "/temp-queue/{0}"

@Injectable()
export class ChatRoomService {

	//msgReceived: EventEmitter<Message> = new EventEmitter<Message>()
	public selectedRoomId: string = ""
	roomSelected: EventEmitter<string> = new EventEmitter<string>()
	roomCreated: EventEmitter<IChatRoom> = new EventEmitter<IChatRoom>()
	constructor(
		private stompService: StompService,
		private auth: AuthService,
		private http: Http
	) {
	}

	ngOnInit() {
		this.selectedRoomId = ""
	}

	selectChatRoom(roomId: string) {
		this.selectedRoomId = roomId
		this.roomSelected.emit(roomId)
	}


	getChatRoomsForUser() {
		return new Promise<IChatRoom[]>(resolve => {
			let headers = new Headers()
			headers.append("Content-Type", "application/json")
			let data = { username: this.auth.currentUser.username }

			let obs = this.http.post("ChatRoom/GetChatRoomsForUser", data, { headers: headers })
				.map(resp => <IChatRoom[]>resp.json())
			obs.subscribe(
				result => resolve(result),
				e => { console.error(e); Toastr.error(serverError) })
		})
	}

	createSingleChatRoom(username: string, message?: string) {
		return new Promise<any>(resolve => {
			let headers = new Headers()
			headers.append("Content-Type", "application/json")
			let data = { username: username, message: message }

			let obs = this.http.post("ChatRoom/CreateSingleChatRoom", data, { headers: headers })
				.map(resp => <IOpResult>resp.json())
			obs.subscribe(result => {
				if (result.succeeded)
					this.roomCreated.emit(<IChatRoom>result.content)
				resolve(result)
			}, e => { Toastr.error(serverError);resolve() })
		})
	}
}