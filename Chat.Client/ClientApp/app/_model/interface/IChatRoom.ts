import { ChatRoomType } from '../enum/index'
import { IChatMessage } from './IChatMessage'


export interface IChatRoom extends IChatRoomTab {
	messages: IChatMessage[]
}

export interface IChatRoomTab {
	id: string
	users: string[]
	messages: IChatMessage[]
	lastMessageTime:Date
	type: ChatRoomType
	name?:string
}

