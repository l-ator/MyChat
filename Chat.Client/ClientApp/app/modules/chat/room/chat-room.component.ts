import { Component, Input, ViewChild, ElementRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Toastr, CustomValidators } from '../../../_common/index'
import { Message } from '@stomp/stompjs'

import { Helpers } from '../../../_common/index'

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

@Component({
	selector: 'chat-room',
	templateUrl: './chat-room.component.html',
	styleUrls: ['./chat-room.component.css']
})
export class ChatRoomComponent {

	@Input() room: IChatRoom
	sendMessageForm: FormGroup
	@ViewChild('msgList') msgList: ElementRef

	constructor(
		private auth: AuthService,
		private chatRoomService: ChatRoomService,
		private chatMessageService: ChatMessageService
	) { }

	ngOnInit() {
		this.scrollToBottom()
		let body = new FormControl('', [Validators.required, CustomValidators.stringNotEmpyValidator])
		this.sendMessageForm = new FormGroup({ body: body })
		this.msgList.nativeElement.addEventListener('DOMNodeInserted', this.scrollToBottom)
		this.msgList.nativeElement.addEventListener('DOMNodeRemoved', this.scrollToBottom)
		
	}

	ngOnChanges(args: any[]) {
		this.ngOnInit()
	}

	sendMessage(values) {
		let msg = {
			id: Helpers.generateGuid(),
			body: values.body.trim(),
			from: this.auth.currentUser.username,
			timestamp: new Date(),
			status: ChatMessageStatus.Sending
		}

		this.room.messages.push(<any>msg)

		this.sendMessageForm.reset()
		this.chatMessageService.send(this.room, msg.body, msg.id, sessionStorage["session"])
			.then(message => this.scrollToBottom())
	}

	public scrollToBottom() {
		let interval = setInterval(() => {
			if (this.msgList) {
				this.msgList.nativeElement.scrollTop = this.msgList.nativeElement.scrollHeight
				clearInterval(interval)
			}
		}, 1)
	}
}
