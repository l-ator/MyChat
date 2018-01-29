import { Injectable } from '@angular/core'
import { Observable } from 'rxjs/Rx'
import { Router, CanActivate } from '@angular/router'
import { AuthService } from '../../_service/index'

@Injectable()
export class LoggedInGuard implements CanActivate {

	constructor(
		private authService: AuthService,
		private router: Router
	) { }

	canActivate(): Observable<boolean> | boolean {


		if (this.authService.isAuthenticated())
			return true;
		else {
			this.router.navigate(['/account/login']);
			return false;
		}
	}
}

@Injectable()
export class LoggedOutGuard implements CanActivate {

	constructor(
		private authService: AuthService,
		private router: Router
	) { }

	canActivate(): Observable<boolean> | boolean {

		if (!this.authService.isAuthenticated()) 
			return true;
		else {
			this.router.navigate(['/chat/lobby']);
			return false;
		}
	}
}