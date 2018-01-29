import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { RouterModule } from '@angular/router'
import { ReactiveFormsModule } from '@angular/forms'
import { TooltipModule } from 'ngx-tooltip'

import { accountRoutes } from './account.routes'

import { AuthService } from '../../_service/auth.service'

import {
	LoginComponent,
	RegistrationComponent
} from './index'

@NgModule({
	imports: [
		CommonModule,
		RouterModule.forChild(accountRoutes),
		ReactiveFormsModule,
		TooltipModule
	],
	declarations: [
		LoginComponent,
		RegistrationComponent
	],
	providers: [
	]
})
export class AccountModule { }