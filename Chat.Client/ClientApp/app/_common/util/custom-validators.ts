import { FormGroup, FormControl, FormArray } from '@angular/forms'

export class CustomValidators {

	public static requiredValidator(control: FormControl): { [key: string]: any } {
		return !control.value
			? { 'required': 'This field is required' }
			: {}
	}

	public static stringNotEmpyValidator(control: FormControl): { [key: string]: any } {
		return control.value && !control.value.trim().length
			? { 'emptyString': 'This field is required' }
			: {}
	}

	public static alphanumericValidator(control: FormControl): { [key: string]: any } {
		let regex = /^[a-zA-Z0-9_]*$/
		return control.value && !regex.test(control.value.trim())
			? { 'invalidString': 'This field may contain only alphanumeric or underscore characters' }
			: {}
	}


	public static requiredIfValidator(required: boolean) {
		return (control: FormControl): { [key: string]: any } => {
			console.log("running", required)
			if (!required)
				return {}
			return !control.value
				? { 'required': 'This field is required' }
				: {}
		}
	}

	public static minLengthValidator(minLenght: number) {
		return (control: FormControl): { [key: string]: any } => {
			if (!control.value)
				return {}
			return control.value.length < minLenght
				? { 'minLengthExceeded': 'Minimum length is ' + minLenght }
				: {}
		}
	}

	public static maxLengthValidator(maxLength: number) {
		return (control: FormControl): { [key: string]: any } => {
			if (!control.value)
				return {}
			return control.value.length > maxLength
				? { 'maxLengthExceeded': 'Maximum length is ' + maxLength }
				: {}
		}
	}

	public static emailValidator(control: FormControl): { [key: string]: any } {
		let regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
		return !regex.test(control.value)
			? { 'invalidEmail': 'Insert a valid email' }
			: {}
	}

	public static containsDigitValidator(control: FormControl): { [key: string]: any } {
		if (control.value == "")
			return {}
		let regex = /\d/
		return !regex.test(control.value)
			? { 'passwordNoDigits': "Password must contain at least one digit" }
			: {}
	}

	public static passwordConfirmValidator(password: FormControl, passwordConfirm: FormControl) {
		return (group: FormGroup): { [key: string]: any } => {
			if (!password.value || !passwordConfirm.value) {
				return {}
			}
			return password.value != passwordConfirm.value
				? { "passwordNotMatching": "Password and Confirm Password do not match." }
				: {}
		}
	}

	public static selectedRolesValidator(array: FormArray): { [key: string]: any } {
		return array.controls.length <= 0
			? { "noRoleSelected": "User must be assigned at least one role" }
			: {}
	}

	public static minDateValidator(minDate: Date) {
		return (control: FormControl): { [key: string]: any } => {
			if (!control.value || !minDate)
				return {}
			console.log("end", control.value.getTime(), "start", minDate.getTime(), "pass", control.value.getTime() >= minDate.getTime())
			return control.value.getTime() < minDate.getTime()
				? { 'minDateRequired': 'Minimum Date is ' + minDate }
				: {}
		}
	}

	public static DateRangeValidator(startDate: FormControl, endDate: FormControl) {
		return (group: FormGroup): { [key: string]: any } => {
			if (!startDate.value || !endDate.value || startDate.value == '' || endDate.value == '') {
				return {}
			}
			return startDate.value > endDate.value
				? { "dateRangeInvalid": "start date is later than end date" }
				: {}
		}
	}
}