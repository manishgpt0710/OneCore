import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  user: {
    firstName: string;
    lastName: string;
    username: string;
    email: string;
  };
}

export interface RegistrationRequest {
  firstName: string;
  lastName: string;
  username: string;
  email: string;
  password: string;
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {
    console.log('api base url', this.apiUrl);
  }

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/Account/login`, credentials);
  }

  register(userData: RegistrationRequest): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Account/register`, userData);
  }
}
