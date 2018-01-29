import { Component, Input } from '@angular/core';

import { Message } from '@stomp/stompjs'

import {
	AuthService,
	ChatRoomService,
	ChatMessageService
} from '../../../_service/index'

import {
	IChatMessage,
	IChatRoomTab,
	IChatRoom
} from '../../../_model/interface/index'

import {ChatRoomType,ChatMessageStatus} from '../../../_model/enum/index'

import { Helpers } from '../../../_common/index'


@Component({
	selector: 'chat-room-tab',
	templateUrl: './chat-room-tab.component.html',
	styleUrls: ['./chat-room-tab.component.css']
})
export class ChatRoomTabComponent {

	@Input() tabRoom: IChatRoom
	@Input() message: IChatMessage
	//@Input() unread:number
	roomTypes = ChatRoomType

	constructor(
		private auth: AuthService,
		private chatRoomService: ChatRoomService,
		private chatMessageService: ChatMessageService
	) { }

	selectRoom() {
		if (this.tabRoom.id != this.chatRoomService.selectedRoomId) {
			this.chatRoomService.selectChatRoom(this.tabRoom.id)
		}
	}

	isSelected = () => this.tabRoom.id == this.chatRoomService.selectedRoomId

	unread = () => this.tabRoom.messages
		.filter(m => m.from != this.auth.currentUser.username && m.status < ChatMessageStatus.Read).length

	lastMessage = () => this.tabRoom.messages[this.tabRoom.messages.length - 1]

	getStatusIcon = (status: ChatMessageStatus) => Helpers.getMessageStatusFA(status)
}
