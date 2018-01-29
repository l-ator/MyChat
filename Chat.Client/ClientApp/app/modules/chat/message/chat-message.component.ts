import { Component, Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Toastr, CustomValidators } from '../../../_common/index'
import { Message } from '@stomp/stompjs'

import {
	AuthService,
	ChatRoomService,
	ChatMessageService
} from '../../../_service/index'

import {
	IChatMessage,
	IChatRoom
} from '../../../_model/interface/index'
import {
	ChatMessageStatus
} from '../../../_model/enum/index'

import { Helpers } from '../../../_common/index'

@Component({
	selector: 'chat-message',
	templateUrl: './chat-message.component.html',
	styleUrls: ['./chat-message.component.css']
})
export class ChatMessageComponent {

	@Input() message: IChatMessage
	isMine = false

	constructor(
		private auth: AuthService,
		private chatRoomService: ChatRoomService,
		private chatMessageService: ChatMessageService
	) { }

	ngOnInit() {
		this.isMine = this.message.from === this.auth.currentUser.username
	}

	getStatusIcon = (status: ChatMessageStatus) => Helpers.getMessageStatusFA(status)

}
