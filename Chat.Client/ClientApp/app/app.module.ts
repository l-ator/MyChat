import { NgModule, Injectable, APP_INITIALIZER } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ROUTER_INITIALIZER } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { BlockUIModule } from 'ng-block-ui'

import { appRoutes } from './app.routes'

import { AppComponent } from './modules/app/app.component';
import { NavbarComponent } from './modules/navbar/navbar.component';
import { HomeComponent } from './modules/home/home.component';

import {
	AppInitService,
	AuthService,
	UserService,
	BlockUIService,
	ConfigService
} from './_service/index'

import { LoggedInGuard, LoggedOutGuard } from './_common/guards/auth.guard'

import * as rxjs from "rxjs"

export function appInit(appInitService: AppInitService) {
	function f() {
		return appInitService.init()
	}
	return f
}

@NgModule({
	imports: [
		BrowserModule,
		HttpModule,
		FormsModule,
		RouterModule.forRoot(appRoutes),
		BlockUIModule
	],
	declarations: [
		AppComponent,
		HomeComponent,
		NavbarComponent
	],
	providers: [
		AppInitService,
		ConfigService,
		AuthService,
		UserService,
		BlockUIService,
		LoggedInGuard,
		LoggedOutGuard,
		{
			provide: APP_INITIALIZER,
			//useFactory: (initializer: AppInitService) => () => initializer.init(),
			useFactory: appInit,
			deps: [AppInitService, AuthService, ConfigService],
			multi: true
		},
	],
	bootstrap: [AppComponent]
})
export class AppModule { }