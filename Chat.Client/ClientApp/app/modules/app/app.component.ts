import { Component } from '@angular/core';
import { AuthService } from '../../_service/auth.service'
import { BlockUI, NgBlockUI } from 'ng-block-ui'
import { BlockUIService } from '../../_service/blockUI.service'

import {
	Router,
	Event as RouterEvent,
	NavigationStart,
	NavigationEnd,
	NavigationCancel,
	NavigationError
} from "@angular/router"

@Component({
	selector: 'app',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css']
})
export class AppComponent {

	@BlockUI('app-blockUI') blockUI: NgBlockUI

	constructor(
		private auth: AuthService,
		private blockUIService: BlockUIService,
		private router:Router
	) {
		router.events.subscribe((event: RouterEvent) =>
			this.navigationInterceptor(event))
	}

	navigationInterceptor(event: RouterEvent): void {

		if (event instanceof NavigationStart)
			this.blockUIService.blockAppUI()

		else if (event instanceof NavigationEnd)
			this.blockUIService.unblockAppUI()

		else if (event instanceof NavigationCancel)
			this.blockUIService.unblockAppUI()

		else if (event instanceof NavigationError)
			this.blockUIService.unblockAppUI()
	}

}
