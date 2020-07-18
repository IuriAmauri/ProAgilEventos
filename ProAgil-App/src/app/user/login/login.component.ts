import { Component, OnInit } from '@angular/core';
import { Route } from '@angular/compiler/src/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(
    private toastr: ToastrService,
    private authService: AuthService,
    private router: Router) { }

  model: any = {};

  ngOnInit() {
    if (localStorage.getItem('token') !== null) {
      this.router.navigate(['/dashboard']);
    }
  }

  login() {
    this.authService.login(this.model).subscribe(
      () => {
        this.router.navigate(['/dashboard']);
      },
      error => {
        this.toastr.error(`Falha ao logar. ${error}`);
      }
    );
  }

  redirect(url: string) {
    this.router.navigate([url]);
  }

}
