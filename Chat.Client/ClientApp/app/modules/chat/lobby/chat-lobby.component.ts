import { Component, ViewContainerRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Toastr } from '../../../_common/util/toastr.helper'
import { Message } from '@stomp/stompjs'
import { overlayConfigFactory } from 'angular2-modal'
import { Modal, BSModalContext } from 'angular2-modal/plugins/bootstrap'

import { NewChatRoomModal } from "../modal/new-chat-room.modal"
import {
	ChatRoomService,
	ChatMessageService,
	AuthService,
	BlockUIService
} from '../../../_service/index'

import {
	IChatMessage,
	IChatRoom,
	IChatRoomTab,
	IStatusUpdate
} from '../../../_model/interface/index'

import { ChatRoomType, ChatMessageStatus } from '../../../_model/enum/index'

import { Sorters, Helpers } from '../../../_common/index'

const MOCK_ROOM = "mock"

@Component({
	selector: 'chat-lobby',
	templateUrl: './chat-lobby.component.html',
	styleUrls: ['./chat-lobby.component.css']
})
export class ChatLobbyComponent {

	chatRooms: IChatRoom[] = []
	allUsers: string[] = []
	selectedRoom: IChatRoom
	user: string = ''
	sessionId: string
	search: string = ''

	constructor(
		private chatRoomService: ChatRoomService,
		private chatMessageService: ChatMessageService,
		public  auth: AuthService,
		private blockUI: BlockUIService,
		private route: ActivatedRoute,
		private modal: Modal
	) {
	}

	ngOnInit() {

		this.sessionId = Helpers.generateGuid()
		sessionStorage["session"] = this.sessionId
		this.chatRoomService.selectedRoomId = ""

		//retrieves resolved data from route
		let routeData = (<any>this.route.data).value
		let roomsData = <IChatRoom[]>routeData.rooms

		this.allUsers = <string[]>routeData.users
		//sorts by  last Message timestamp and renames rooms of type Single 
		this.chatRooms = Sorters.SortRoomsByLastMessage(
			roomsData.map(r => {
				if (r.type == ChatRoomType.Single)
					r.name = r.users.find(u => u != this.auth.currentUser.username)
				return r
			}))

		//marks as received message persisted while offline
		let receivedList: string[] = []
		this.chatRooms.forEach((room: IChatRoom) => {
			let received = room.messages
				.filter(msg =>
					msg.status <= ChatMessageStatus.Sent
					&& msg.from != this.auth.currentUser.username)
				.map(msg => msg.id)

			received.forEach(r => receivedList.push(r))
		})
		if (receivedList.length)
			this.chatMessageService.updateMessagesStatus(receivedList, ChatMessageStatus.Received)

		//starts consuming from inbox and status queues
		this.chatMessageService.consumeInbox()
		this.chatMessageService.consumeStatusUpdates()


		//both sent and received messages are pushed in the inbox queue
		//this handler is executed upon consumption  of the inbox queue
		this.chatMessageService.messageReceived
			.subscribe((message: IChatMessage) => {

				console.log(message)

				let isMine = message.from == this.auth.currentUser.username
				let isGreeting = (<any>message).greeting
				let sessionId = (<any>message).session
				//determines status to set
				message.status = isMine
					? ChatMessageStatus.Sent
					: (this.chatRoomService.selectedRoomId != message.roomId
						? ChatMessageStatus.Received
						: ChatMessageStatus.Read)

				console.log(message)

				//checks if new room has to be created
				let roomexists: boolean = true
				let room = this.chatRooms.find(r => r.id == message.roomId)
				if (!room) {
					roomexists = false
					room = {
						id: message.roomId,
						users: [this.auth.currentUser.username, message.from],
						type: ChatRoomType.Single,
						lastMessageTime: message.timestamp,
						messages: [],
						name: isMine ? message.from : message.to
					}


					if (isMine && !isGreeting) {
						if (sessionId == this.sessionId) {
							this.chatRoomService.createSingleChatRoom(message.to, message.body)
								.then(result => {
									console.log("RESULT", result)
									if (!result.succeeded)
										window.location.reload()
									else { 
										let newRoom = <IChatRoom>result.content
										this.selectedRoom = newRoom
										this.chatRoomService.selectedRoomId = newRoom.id
									}
								})
						}
						else {
							this.blockUI.blockAppUI();
							window.location.reload()
						}
					}
				}

				//if message is in "SENDING" state sets it to "SENT"
				var existingMsgIndex = room.messages.findIndex(m => m.id == message.id)
				if (existingMsgIndex < 0)
					room.messages.push(message)
				else room.messages[existingMsgIndex] = message

				//update received message status
				if (!isMine)
					this.chatMessageService.updateMessagesStatus(message.id, message.status)

				room.lastMessageTime = message.timestamp

				if (!roomexists
					&& message.from != this.auth.currentUser.username
					&& (<any>message).greeting) {
					room.name = message.from
					this.chatRooms.unshift(room)
				}
				this.chatRooms = Sorters.SortRoomsByLastMessage(this.chatRooms)
			})


		//status updates for both sent and received are pushed in the status_update queue
		//this handler is executed upon consumption of the inbox queue
		this.chatMessageService.statusUpdated.subscribe((update: IStatusUpdate) => {
			let room = this.chatRooms.find(r => r.id == update.roomId)
			if (!room)
				return

			let message = room.messages.find(m => m.id == update.id)
			if (!message)
				return
			else message.status =
				Math.max(message.status, update.status)
		})

		this.chatRoomService.roomSelected
			.subscribe(roomId => {
				this.selectedRoom = this.chatRooms.filter(c => c.id == roomId)[0]

				let readList = this.selectedRoom.messages.filter(msg =>
					msg.status < ChatMessageStatus.Read
					&& msg.from != this.auth.currentUser.username)
					.map(msg => msg.id)

				this.chatMessageService.updateMessagesStatus(readList, ChatMessageStatus.Read)
			})
		this.chatRoomService.roomCreated
			.subscribe((room: IChatRoom) => {
				this.chatRooms.unshift(room)
			})

		this.auth.loggedOutEvent.subscribe(
			() => this.chatMessageService.stopConsuming())
	}

	newChatroom() {
		this.modal.open(NewChatRoomModal, overlayConfigFactory({ chatRooms: this.chatRooms }, BSModalContext))
	}

	searchName = (rooms: IChatRoom[]) => !this.search.length ? rooms : rooms.filter(r => (<string>r.name).trim().indexOf(this.search.trim()) >= 0)

	newSingleChat = (username: string) => {
		this.chatRoomService.selectedRoomId = ""
		this.search = ""
		console.log("new chat with", username)
		this.selectedRoom = <any>{
			users: [this.auth.currentUser.username, username],
			name: username,
			messages: [],
			lastMessageTime: new Date(),
			id: null,
			type: ChatRoomType.Single
		}
	}

	getTab = (room: IChatRoom) => {
		let tab = JSON.parse(JSON.stringify(room))
		return tab
	}

	getAddableUsers = () => {
		let added: string[] = [this.auth.currentUser.username]
		this.chatRooms.forEach(r => r.type == ChatRoomType.Single
			? r.users.filter(u => u != this.auth.currentUser.username).forEach(u => added.push(u)) : null)
		return this.allUsers.filter(u => added.indexOf(u) < 0)
	}
	searchAddable = (addables: string[]) => addables.filter(a => a.indexOf(this.search.trim()) >= 0)




}