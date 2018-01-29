import { ChatMessageStatus } from '../enum/index'

export interface IStatusUpdate {
	id: string
	roomId:string
	status: ChatMessageStatus
}