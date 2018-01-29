import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../_service/index'


@Component({
	selector: 'home',
	templateUrl: './home.component.html'
})
export class HomeComponent {

	constructor(
		private auth: AuthService,
		private router: Router
	) { }

	ngOnInit() {
		console.log("HOME COMPONENT", this.auth)
		if (this.auth.isAuthenticated()) {
			console.log("redirect to lobby")
			this.router.navigate(['chat/lobby'])
		}
		else {
			console.log("redirect to login")
			this.router.navigate(['account/login'])
		}
	}
}