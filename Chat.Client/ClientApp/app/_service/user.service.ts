import { Injectable } from '@angular/core'
import { Http, Headers } from '@angular/http'
import { Toastr } from '../_common/util/toastr.helper'
import { Helpers } from '../_common/util/custom-helpers'

import {
	IUser,
	IOpResult,
	ILogin,
	IRegister
} from '../_model/interface/index'

import 'rxjs'

const serverError: string = "Internal Server Error"

@Injectable()
export class UserService {

	currentUser: IUser = { id: '', username: '', email: '' }

	constructor(
		private http: Http
	) { }

	userExists(username: string) {

		return new Promise<IOpResult>(resolve => {
			let headers: Headers = new Headers()
			headers.append("Content-Type", "application/json")

			let data = { username: username }

			let obs = this.http.post("User/UserExists", data, { headers: headers })
				.map(resp => <IOpResult>resp.json())
			obs.subscribe(result => resolve(result), e => Toastr.error(serverError))
		})
	}

	getAllUsernames() {
		return new Promise<string[]>(resolve => {
			let obs = this.http.get("User/GetAllUsers")
				.map(resp => <IOpResult>resp.json())
			obs.subscribe(result => resolve(<string[]>result.content), e => Toastr.error(serverError))
		})
	}
}