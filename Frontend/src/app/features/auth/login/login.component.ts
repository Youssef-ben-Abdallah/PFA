import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid) return;
    this.auth.login(this.loginForm.value).subscribe({
      next: (res) => {
        this.auth.saveToken(res.token);
        this.auth.saveUser({ username: res.username, roles: res.roles });
        this.router.navigate(['/dashboard/executive-overview']);
      },
      error: (err) => alert('Login failed: ' + err.error)
    });
  }
}