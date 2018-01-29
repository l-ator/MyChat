import { IChatRoom } from '../../_model/interface/index'

export class Sorters {

	static SortRoomsByLastMessage = (rooms: IChatRoom[]) =>
		rooms.sort((a, b) => {
			if (a.lastMessageTime < b.lastMessageTime)
				return 1
			else return -1
		})
}


