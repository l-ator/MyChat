import { Injectable } from "@angular/core"
import { BlockUIService as BUIService } from 'ng-block-ui'

const appLabel: string = "app-blockUI"
const defaultMessage: string = ""

@Injectable()
export class BlockUIService {


	appBlockUICounter: number = 0

	constructor(
		private blockUIService: BUIService
	) { }

	blockAppUI(message: string = defaultMessage) {
		if (++this.appBlockUICounter)
			this.blockUIService.start(appLabel, message)
	}

	unblockAppUI() {
		if (!--this.appBlockUICounter)
			this.blockUIService.stop(appLabel)
	}
}