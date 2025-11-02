import { Routes } from '@angular/router';
import { RegisterPage } from './components/auth/register-page/register-page';
import { LoginPage } from './components/auth/login-page/login-page';
import { HomePage } from './components/home-page/home-page';
import { AuthGuard } from './services/auth/guard-service';
import { AdminPage } from './components/auth/admin-page/admin-page';
import { AdminGuard } from './services/auth/admin-guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'register',
    component: RegisterPage
  },
  {
    path: 'login',
    component: LoginPage
  },
  {
    path:'home',
    component: HomePage,
    canActivate: [AuthGuard]
  },
  {
    path:'admin',
    component: AdminPage,
    canActivate: [AdminGuard]
  },
  {
    path:'**',
    redirectTo: 'login'
  },

];
