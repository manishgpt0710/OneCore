import { Routes } from '@angular/router';
import { LoginComponent } from './components/login.component';
import { RegistrationComponent } from './components/registration.component';
import { DashboardComponent } from './components/dashboard.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegistrationComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];
