import { Routes } from '@angular/router'

import { ChatRoomsResolver, ChatUsersResolver } from '../../_common/index'

import { ChatLobbyComponent } from './index'

export const chatRoutes: Routes = [
	{ path: '', redirectTo: "/chat/lobby", pathMatch: 'full' },
	{ path: 'lobby', component: ChatLobbyComponent, resolve: { rooms: ChatRoomsResolver, users: ChatUsersResolver } },
	{ path: "**", redirectTo: "/chat/lobby" }
]