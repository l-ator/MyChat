import { RouterModule } from '@angular/router';
import { Routes, CanActivate } from '@angular/router'
import { HomeComponent } from './modules/home/home.component'
import { LoggedInGuard, LoggedOutGuard } from './_common/guards/auth.guard'

export const appRoutes: Routes = [
	{ path: '', redirectTo: '/chat/lobby', pathMatch: 'full' },
	//{ path: 'home', component: HomeComponent },
	{ path: 'account', loadChildren: './modules/account/account.module#AccountModule' },
	{
		path: '', canActivate: [LoggedInGuard], children: [
			{ path: 'chat', loadChildren: './modules/chat/chat.module#ChatModule' },
		]
	},
	{ path: '**', redirectTo: 'chat' }
]

 