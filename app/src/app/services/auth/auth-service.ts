import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { AccessToken, LoginData, User, UserForm } from '../../interfaces';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { baseUrl } from '../../../constants';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private router = inject(Router);

  currentToken = signal<string | undefined | null>(localStorage.getItem('accessToken'));

  private http = inject(HttpClient);

  isAuthenticated = computed(() => {
    const token = this.currentToken();
    return !!token && !this.isTokenExpired(token);
  });

  register(user: UserForm): Observable<any> {
    return this.http.post(`${baseUrl}/auth/register`, user);
  }

  login(login: LoginData): Observable<AccessToken> {
    return this.http.post<AccessToken>(`${baseUrl}/auth/login`, login);
  }

  findAllUsers() {
    return this.http.get<User[]>(`${baseUrl}/auth/users`);
  }

  assignUserRole(role:string, userId:string) {
    return this.http.get<User[]>(`${baseUrl}/auth/assign-role/${userId}/${role}`);
  }

  logout(): void {
    this.currentToken.set(null);
    localStorage.removeItem('accessToken');
    this.router.navigate(['/login']);
  }

  private isTokenExpired(token: string): boolean {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return Date.now() >= payload.exp * 1000;
    } catch {
      return true;
    }
  }

  private decodeToken(): any {
    try {
      const token = this.currentToken();
      if (token) {
        return jwtDecode(token);
      }
    } catch (error) {
      console.error('Erro ao decodificar token:', error);
      return null;
    }
  }

  getUserRole(): string {
    const decodedToken = this.decodeToken();
    return decodedToken.role;
  }

  isAdmin():boolean {
    return this.getUserRole() == 'admin'
  }

  getUserId():string {
    return this.decodeToken().nameid;
  }


}
