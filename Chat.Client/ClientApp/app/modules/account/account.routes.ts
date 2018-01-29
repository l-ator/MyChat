import { Routes, CanActivate } from '@angular/router'

import {
	LoginComponent,
	RegistrationComponent
} from './index'

import { LoggedInGuard, LoggedOutGuard } from '../../_common/guards/auth.guard'

export const accountRoutes: Routes = [
	{ path: 'login', canActivate: [LoggedOutGuard], component: LoginComponent },
	{ path: 'register', canActivate: [LoggedOutGuard], component: RegistrationComponent }
]