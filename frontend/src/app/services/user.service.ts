import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';
import { CreateUserRequest } from '../models/auth.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = `${environment.apiUrl}/api`;

  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]> {
    console.log(`API URL: ${this.apiUrl}/Users`);
    return this.http.get<User[]>(`${this.apiUrl}/Users`);
  }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/Users/${id}`);
  }

  createUser(user: User): Observable<any> {
    const createUserRequest: CreateUserRequest = {
      username: user.username,
      password: user.password || user.passwordHash || '', // Önce password, yoksa passwordHash kullan
      sicil: user.sicil,
      isAdmin: user.isAdmin
    };
    
    // Kontrol amaçlı log
    console.log('Gönderilen kullanıcı verisi:', createUserRequest);
    
    return this.http.post<any>(`${this.apiUrl}/auth/create-user`, createUserRequest);
  }

  updateUser(id: number, user: User): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/Users/${id}`, user);
  }

  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/Users/${id}`);
  }
}
