import { Injectable, EventEmitter } from '@angular/core'
import { Http, Headers } from '@angular/http'
import { Toastr } from '../_common/util/toastr.helper'
import { Helpers } from '../_common/util/custom-helpers'

import {
	IUser,
	IAuthOpResult,
	ILogin,
	IRegister
} from '../_model/interface/index'

import 'rxjs'

const serverError: string = "Internal Server Error"

@Injectable()
export class AuthService {

	currentUser: IUser = { id: '', username: '', email: '' }

	loggedInEvent = new EventEmitter<IUser>()
	loggedOutEvent = new EventEmitter<void>()



	constructor(
		private http: Http,
	) { }

	isAuthenticated() {
		return this.currentUser.username && this.currentUser.username != ''
	}


	login(loginModel: ILogin) {
		return new Promise<IAuthOpResult>(resolve => {

			let headers: Headers = new Headers()
			headers.append("contentType", "application/json")

			let obs = this.http.post("Auth/Login", loginModel, { headers: headers })
				.map(resp => {
					console.log("LOGGED IN")
					let result = <IAuthOpResult>resp.json()
					if (result.succeeded) {
						this.currentUser.id = result.id
						this.currentUser.username = result.username
						this.currentUser.email = result.email
					}
					return result
				})

			obs.subscribe(result => {
				this.loggedInEvent.emit(this.currentUser)
				resolve(result)
			},
				e => Toastr.error(serverError))
		})
	}

	logout() {
		this.loggedOutEvent.emit()
		return new Promise<IAuthOpResult>(resolve => {
			let obs = this.http.post("Auth/Logout", null)
				.map(resp => {
					let result = <IAuthOpResult>resp.json()
					if (result.succeeded) {
						this.currentUser.id = ''
						this.currentUser.username = ''
						this.currentUser.email = ''
					}
					return result
				})

			obs.subscribe(result => {
				resolve(result)
			},
				e => Toastr.error(serverError))
		})
	}

	register(registerModel: IRegister) {

		return new Promise<IAuthOpResult>(resolve => {

			let headers: Headers = new Headers()
			headers.append("contentType", "application/json")

			let obs = this.http.post("Auth/Register", registerModel, { headers: headers })
				.map(resp => {
					let result = <IAuthOpResult>resp.json()
					if (result.succeeded) {
						this.currentUser.id = result.id
						this.currentUser.username = result.username
						this.currentUser.email = result.email
					}
					return result
				})

			obs.subscribe(result => {
				this.loggedInEvent.emit(this.currentUser)
				resolve(result)
			},
				e => Toastr.error(serverError))
		})
	}

	checkAuth() {
		return new Promise<boolean>(resolve => {
			let obs = this.http.get("Auth/CheckAuth")
				.map(resp => <IAuthOpResult>resp.json())

			obs.subscribe(result => {
				if (result.username && result.username != '') {
					this.currentUser.username = result.username;
					resolve(true)
				}
				else resolve(false)
			}, e => Toastr.error(serverError))
		})
	}
}