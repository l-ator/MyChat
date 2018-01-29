declare let toastr
export class Toastr {

	public static success(message: string, title?: string) {
		toastr.success(message, title)
	}

	public static info(message: string, title?: string) {
		toastr.info(message, title)
	}

	public static warning(message: string, title?: string) {
		toastr.warning(message, title)
	}

	public static error(message: string, title?: string) {
		toastr.error(message, title)
	}
}