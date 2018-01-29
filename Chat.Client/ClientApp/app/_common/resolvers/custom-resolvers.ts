import { Injectable } from '@angular/core'
import { Router, Resolve, ActivatedRouteSnapshot } from '@angular/router'

import { ChatRoomService } from '../../_service/chat-room.service'
import { UserService } from '../../_service/user.service'

@Injectable()
export class ChatRoomsResolver implements Resolve<any> {

	constructor(private chatRoomService: ChatRoomService) { }

	resolve = () => this.chatRoomService.getChatRoomsForUser()
}


@Injectable()
export class ChatUsersResolver implements Resolve<any> {

	constructor(private userService: UserService) { }

	resolve = () => this.userService.getAllUsernames()
}

