import { Component, } from '@angular/core'
import { DialogRef, ModalComponent } from 'angular2-modal'
import { BSModalContext } from 'angular2-modal/plugins/bootstrap'

import {
	AuthService,
	UserService,
	ChatRoomService,
	BlockUIService
} from '../../../_service/index'

import { Toastr, Helpers } from '../../../_common/index'
import { IChatRoom, IUser } from '../../../_model/interface/index'
import { ChatRoomType } from '../../../_model/enum/index'

class NewChatRoomContext extends BSModalContext {

	chatRooms: IChatRoom[]

}

declare var appVersion: any;

@Component({
	selector: "new-chat-room-modal",
	templateUrl: "./new-chat-room.modal.html",
	styleUrls: ["./new-chat-room.modal.css"]
})
export class NewChatRoomModal implements ModalComponent<NewChatRoomContext> {

	context: NewChatRoomContext
	userSearch: string = ""
	username: string = ""
	message: string = ""
	wait = false

	constructor(
		public dialog: DialogRef<NewChatRoomContext>,
		private auth: AuthService,
		private userService: UserService,
		private chatRoomService: ChatRoomService,
		private blockUI: BlockUIService
	) {
		this.context = dialog.context
	}

	findUser(username: string) {
		this.username = ""
		this.userSearch = ""
		this.message = ""

		username = username.trim()

		if (!username.length || username == this.auth.currentUser.username) {
			Toastr.warning("Enter a valid username")
			return
		}

		this.blockUI.blockAppUI("Finding user..")
		let existingRoom = this.context.chatRooms.find(c =>
			c.type == ChatRoomType.Single && c.users.indexOf(username) >= 0)
		if (existingRoom) {
			this.chatRoomService.selectChatRoom(existingRoom.id)
			this.dialog.close()
			this.blockUI.unblockAppUI()
		}
		else this.userService.userExists(username)
			.then(result => {
				if (result.succeeded) 
					this.username= result.content.userName
				else result.errors.forEach(e => Toastr.warning(e))
					this.blockUI.unblockAppUI()
				})
	}


	newChat() {
		this.blockUI.blockAppUI()

		if (!this.username || !this.message.length)
			return

		this.chatRoomService.createSingleChatRoom(this.username, this.message)
			.then(result => {
				this.blockUI.unblockAppUI()
				if (result && result.succeeded)
					Toastr.success("Chatroom successfully created!")
				else if (result && result.errors)
					result.errors.forEach(e => Toastr.warning(e))
				this.dialog.close()
			})
	}
}


