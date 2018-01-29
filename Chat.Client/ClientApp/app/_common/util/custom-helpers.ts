import { ChatMessageStatus } from '../../_model/enum/index'

export class Helpers {

	public static stringFormat(fmtstr: string, ...args: any[]): string {
		var i;
		if (args instanceof Array) {
			for (i = 0; i < args.length; i++) {
				fmtstr = fmtstr.replace(new RegExp('\\{' + i + '\\}', 'gm'), args[i]);
			}
			return fmtstr;
		}
		for (i = 0; i < arguments.length - 1; i++) {
			fmtstr = fmtstr.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i + 1]);
		}
		return fmtstr;
	}

	public static generateGuid() {
		let s4 = () => Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);

		return `${s4()}${s4()}${s4()}${s4()}${s4()}${s4()}`
	}

	public static getMessageStatusFA(status: ChatMessageStatus) {

		let icon = ""

		switch (status) {
			case ChatMessageStatus.Sending:
				icon = "clock-o"
				break
			case ChatMessageStatus.Sent:
				icon = "check-circle-o"
				break
			case ChatMessageStatus.Received:
				icon = "check-circle"
				break
			case ChatMessageStatus.Read:
				icon = "eye"
				break
			default:
				icon = ""
		}

		return `fa fa-${icon}`
	}
}