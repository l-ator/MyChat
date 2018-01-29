import { Injectable } from '@angular/core';
import * as rxjs from "rxjs"


import { AuthService } from './auth.service'
import { ConfigService } from './config.service'

@Injectable()
export class AppInitService {

	constructor(private config: ConfigService, private auth: AuthService) {
	}

	ngOnInit() {
		this.auth.loggedOutEvent.subscribe(()=>console.log("ASD"))
	}

	init() {
		return new Promise(resolve => {
			rxjs.Observable.forkJoin(
				this.config.initConfig(),
				this.auth.checkAuth())
				.subscribe(() => resolve())
		})
	}
}