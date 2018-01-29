import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { RouterModule } from '@angular/router'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { HttpModule } from '@angular/http'
import { ModalModule } from 'angular2-modal'
import { BootstrapModalModule } from 'angular2-modal/plugins/bootstrap'

import {
	ChatMessageService,
	ChatRoomService,
	UserService,
	StompService
} from '../../_service/index'

import { ChatRoomsResolver, ChatUsersResolver } from '../../_common/index'

import { chatRoutes } from './chat.routes'

import {
	ChatLobbyComponent,
	ChatRoomComponent,
	ChatRoomTabComponent,
	NewChatRoomModal,
	ChatMessageComponent
} from './index'

export const Resolvers:any = [
	{ provide: ChatRoomsResolver, useFactory: service => new ChatRoomsResolver(service), deps: [ChatRoomService] },
	{ provide: ChatUsersResolver, useFactory: service => new ChatUsersResolver(service), deps: [UserService] }
]

@NgModule({
	imports: [
		CommonModule,
		RouterModule.forChild(chatRoutes),
		FormsModule,
		ReactiveFormsModule,
		ModalModule.forRoot(),
		BootstrapModalModule
	],
	declarations: [
		//COMPONENTS
		ChatLobbyComponent,
		ChatRoomComponent,
		ChatRoomTabComponent,
		NewChatRoomModal,
		ChatMessageComponent
	],
	entryComponents: [
		NewChatRoomModal
		],
	providers: [
		//SERVICES
		ChatMessageService,
		ChatRoomService,
		UserService,
		StompService
	].concat(Resolvers)
})
export class ChatModule { }
