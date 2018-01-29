import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { AuthService } from '../../_service/index'
import { CustomValidators } from '../../_common/util/custom-validators'

import {
	Toastr, Helpers
} from '../../_common/index'

import {
	IRegister
} from '../../_model/interface/index'

declare var appVersion: any;

@Component({
	templateUrl: './registration.component.html',
	styles: [`
        .error{background-color:#f2cbcd;border-width:0}
        .tooltip-arrow{display:none !important}
        .invisible{display:none}
    `]
})
export class RegistrationComponent implements OnInit {

	registerUserForm: FormGroup
	newUserDetails: IRegister
	errors: string[] = []

	constructor(
		private authService: AuthService,
		private router: Router
	) { }

	ngOnInit() {
		let username = new FormControl
			(null, [CustomValidators.requiredValidator, CustomValidators.alphanumericValidator, CustomValidators.maxLengthValidator(20)])
		let email = new FormControl
			(null, [CustomValidators.requiredValidator, CustomValidators.emailValidator])
		let password = new FormControl
			(null, [CustomValidators.requiredValidator, CustomValidators.minLengthValidator(8), CustomValidators.maxLengthValidator(30), CustomValidators.containsDigitValidator])
		let passwordConfirm = new FormControl
			(null, [CustomValidators.requiredValidator])

		this.registerUserForm = new FormGroup({
			username: username,
			email: email,
			password: password,
			passwordConfirm: passwordConfirm
		}, CustomValidators.passwordConfirmValidator(password, passwordConfirm))
	}

	registerNewUser(formValues: IRegister) {

		if (this.registerUserForm.invalid) {
			Toastr.warning("Please fill the form with valid fields")
			return
		}

		this.authService.register(formValues)
			.then(result => {
				console.log(result)
				if (result.succeeded) {
					this.authService.currentUser.username = result.username
					this.authService.currentUser.email = result.email
					this.router.navigate(["/lobby"])
					Toastr.success(Helpers.stringFormat("User {0} successfully registered", formValues.username))
				}
				else result.errors.forEach(e => Toastr.warning(e))
			})
	}

	public getErrors(errors: { [key: string]: any }): string[] {
		if (!errors)
			return []
		let array: string[] = []
		for (let err in errors)
			array.push(errors[err])
		return array
	}
}