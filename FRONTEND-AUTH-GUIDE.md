# 🔐 Guia de Autenticação para o Frontend

## 📋 **ARQUITETURA SIMPLIFICADA**

### **Endpoints Disponíveis:**
- **`POST /api/auth/authenticate`** → Email + Senha → Token + Dados do usuário
- **`GET /api/users`** → Listar usuários (requer token)
- **`POST /api/users`** → Criar usuário (requer token + ser administrador)
- **`GET /api/users/{id}`** → Buscar usuário (requer token)

## 🔧 **IMPLEMENTAÇÃO NO FRONTEND**

### **1. AuthService (auth.service.ts)**

```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';

export interface User {
  id: number;
  name: string;
  email: string;
  isActive: boolean;
  isAdministrator: boolean;
  createdAt: string;
  updatedAt: string | null;
}

export interface AuthResponse {
  token: string;
  user: User;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:7205/api';
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {
    // Verificar se há token salvo no localStorage
    this.loadStoredUser();
  }

  // ✅ ÚNICO método de autenticação
  authenticate(email: string, password: string): Observable<AuthResponse> {
    const body = { email, password };
    
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/authenticate`, body)
      .pipe(
        tap(response => {
          // Salvar token e dados do usuário
          localStorage.setItem('token', response.token);
          localStorage.setItem('user', JSON.stringify(response.user));
          this.currentUserSubject.next(response.user);
        })
      );
  }

  // ✅ Acessar recursos protegidos
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/users`, {
      headers: this.getAuthHeaders()
    });
  }

  createUser(userData: { name: string; email: string; password: string }): Observable<User> {
    return this.http.post<User>(`${this.apiUrl}/users`, userData, {
      headers: this.getAuthHeaders()
    });
  }

  getUserById(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/users/${id}`, {
      headers: this.getAuthHeaders()
    });
  }

  // ✅ Utilitários
  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  isAdministrator(): boolean {
    const user = this.getCurrentUser();
    return user?.isAdministrator || false;
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.currentUserSubject.next(null);
  }

  private getAuthHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  private loadStoredUser(): void {
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      try {
        const user = JSON.parse(storedUser);
        this.currentUserSubject.next(user);
      } catch (error) {
        console.error('Erro ao carregar usuário salvo:', error);
        this.logout();
      }
    }
  }
}
```

### **2. Login Component (login.component.ts)**

```typescript
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  loading = false;
  error = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  ngOnInit(): void {
    // Se já estiver autenticado, redirecionar para dashboard
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/dashboard']);
    }
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.loading = true;
      this.error = '';

      const { email, password } = this.loginForm.value;

      this.authService.authenticate(email, password).subscribe({
        next: (response) => {
          console.log('Login realizado com sucesso:', response);
          
          // Redirecionar baseado no tipo de usuário
          if (response.user.isAdministrator) {
            this.router.navigate(['/admin-dashboard']);
          } else {
            this.router.navigate(['/dashboard']);
          }
        },
        error: (error) => {
          console.error('Erro no login:', error);
          this.loading = false;
          
          if (error.error && error.error.General) {
            this.error = error.error.General;
          } else {
            this.error = 'Erro ao fazer login. Verifique suas credenciais.';
          }
        },
        complete: () => {
          this.loading = false;
        }
      });
    }
  }
}
```

### **3. Auth Guard (auth.guard.ts)**

```typescript
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(): boolean {
    if (this.authService.isAuthenticated()) {
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }
}
```

### **4. Admin Guard (admin.guard.ts)**

```typescript
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(): boolean {
    if (this.authService.isAuthenticated() && this.authService.isAdministrator()) {
      return true;
    } else {
      this.router.navigate(['/dashboard']);
      return false;
    }
  }
}
```

### **5. HTTP Interceptor (auth.interceptor.ts)**

```typescript
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Não adicionar token para endpoints públicos
    if (req.url.includes('/auth/authenticate')) {
      return next.handle(req);
    }

    // Adicionar token para endpoints protegidos
    const token = this.authService.getToken();
    if (token) {
      const authReq = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${token}`)
      });
      return next.handle(authReq);
    }

    return next.handle(req);
  }
}
```

### **6. App Module (app.module.ts)**

```typescript
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';

import { AuthService } from './services/auth.service';
import { AuthInterceptor } from './interceptors/auth.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    DashboardComponent,
    AdminDashboardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [
    AuthService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
```

### **7. App Routing (app-routing.module.ts)**

```typescript
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { AdminGuard } from './guards/admin.guard';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { 
    path: 'dashboard', 
    component: DashboardComponent, 
    canActivate: [AuthGuard] 
  },
  { 
    path: 'admin-dashboard', 
    component: AdminDashboardComponent, 
    canActivate: [AdminGuard] 
  },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: '**', redirectTo: '/dashboard' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
```

## 🎯 **FLUXO DE USO**

### **1. Login:**
```typescript
// No componente de login
this.authService.authenticate('joao.silva@email.com', 'MinhaSenh@123')
  .subscribe(response => {
    // Token e dados do usuário são salvos automaticamente
    // Usuário é redirecionado para dashboard
  });
```

### **2. Acessar recursos protegidos:**
```typescript
// Listar usuários
this.authService.getUsers().subscribe(users => {
  console.log('Usuários:', users);
});

// Criar usuário (apenas administradores)
this.authService.createUser({
  name: 'Maria Silva',
  email: 'maria@email.com',
  password: 'MinhaSenh@456'
}).subscribe(user => {
  console.log('Usuário criado:', user);
});
```

### **3. Verificar autenticação:**
```typescript
// Verificar se está autenticado
if (this.authService.isAuthenticated()) {
  console.log('Usuário autenticado');
}

// Verificar se é administrador
if (this.authService.isAdministrator()) {
  console.log('Usuário é administrador');
}
```

## ✅ **VANTAGENS DESTA ARQUITETURA**

1. **Simples** - Apenas um endpoint de autenticação
2. **Claro** - `/authenticate` é autoexplicativo
3. **Padrão** - Segue convenções da indústria
4. **Seguro** - JWT com validação adequada
5. **Flexível** - Fácil de estender e manter

## 🔑 **CREDENCIAIS DE TESTE**

- **Email:** `joao.silva@email.com`
- **Senha:** `MinhaSenh@123`
- **Tipo:** Administrador

- **Email:** `maria.silva@email.com`
- **Senha:** `MinhaSenh@456`
- **Tipo:** Usuário comum
