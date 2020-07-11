import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/_models/User';
import { AuthService } from 'src/app/_services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  constructor(
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private authService: AuthService,
    private router: Router
    ) {
    }

  registerForm: FormGroup;
  user: User;

  ngOnInit() {
    this.criarForm();
  }

  criarForm() {
    this.registerForm = this.formBuilder.group({
      fullName : ['', Validators.required],
      email : ['', [Validators.required, Validators.email]],
      userName : ['', Validators.required],
      passwords : this.formBuilder.group({
        password : ['', [Validators.required, Validators.minLength(4)]],
        confirmPassword : ['', Validators.required],
      }, { validator : this.compararSenhas})
    });
  }

  ehCampoInvalido(fieldName: string): boolean {
    const field = this.registerForm.get(fieldName);
    return field.errors && field.touched;
  }

  cadastrarUsuario() {
    if (this.registerForm.valid) {
      this.user = Object.assign(this.registerForm.value, {password: this.registerForm.get('passwords.password').value});

      this.authService.register(this.user).subscribe(
        () => {
          this.router.navigate(['/user/login']);
          this.toastr.success('Usuário cadastrado com sucesso!');
        }, error => {
          error.error.forEach(element => {
            switch (element.code) {
              case 'DuplicateUserName':
                this.toastr.error('Usuário já existe!');
                break;
              default:
                this.toastr.error(`Erro ao cadastrar usuário {element.code}`);
            }
          });
        }
      );
    }
  }

  compararSenhas(formGroup: FormGroup) {
    const confirmPassword = formGroup.get('confirmPassword');

    if ((confirmPassword.errors == null) || 'mismatch' in confirmPassword.errors) {
      if (confirmPassword.value !== formGroup.get('password').value) {
        confirmPassword.setErrors({mismatch: true});
      } else {
        confirmPassword.setErrors(null);
      }
    }
  }
}
