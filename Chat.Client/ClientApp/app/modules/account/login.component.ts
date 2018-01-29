import { Component, OnInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { Router } from '@angular/router'

import {
	AuthService,
	BlockUIService
} from '../../_service/index'

import {
	Toastr,
	Helpers
} from '../../_common/index'

import { ILogin } from '../../_model/interface'

@Component({
	//styleUrls: ['app/account/login.component.css'],
	templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {

	loginForm: FormGroup

	constructor(
		private authService: AuthService,
		private blockUIService: BlockUIService,
		private router: Router,
	) { }

	ngOnInit() {
		this.loginForm = new FormGroup({
			username: new FormControl("", [Validators.required]),
			password: new FormControl("", [Validators.required]),
			rememberMe: new FormControl(false)
		})
	}

	loginUser(formValues: ILogin) {

		formValues.username = formValues.username.trim().toLocaleLowerCase()

		if (this.loginForm.invalid) {
			Toastr.warning("Please enter valid login fields")
			return
		}

		this.blockUIService.blockAppUI("Authenticating..")
		this.authService.login(formValues)
			.then(result => {
				if (result.succeeded) {
					this.router.navigate(["/lobby"])
					Toastr.success(Helpers.stringFormat("User {0} successfully logged in", formValues.username))
				}
				else result.errors.forEach(e => Toastr.warning(e))
				this.blockUIService.unblockAppUI()
			})
	}
}