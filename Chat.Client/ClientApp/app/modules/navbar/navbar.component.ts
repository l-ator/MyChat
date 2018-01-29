import { Component } from '@angular/core'
import { Router } from '@angular/router'
import { AuthService, ChatMessageService, BlockUIService } from '../../_service/index'

import {
	Toastr,
	Helpers
} from '../../_common/index'

@Component({
	selector: "navbar",
	//template: <any>require("./navbar.component.html"),
	templateUrl: './navbar.component.html',
	styleUrls: ["./navbar.component.css"]
})
export class NavbarComponent {

	constructor(
		private auth: AuthService,
		private router: Router,
		private blockUI: BlockUIService
	) { }

	ngOnInit() {
	}

	isAuthenticated = () => this.auth.isAuthenticated()

	logoutUser() {
		this.blockUI.blockAppUI()
		this.auth.logout()
			.then(result => {
				if (result.succeeded)
					window.location.reload()
				else this.blockUI.unblockAppUI()
				//this.router.navigate(['/account/login'])
			})
	}
}