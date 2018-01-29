import { ChatMessageStatus } from '../enum/index'

export interface IChatMessage {
	id: string
	roomId:string
	body: string
	from: string
	to: string
	timestamp: Date
	status: ChatMessageStatus
}