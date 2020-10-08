import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, FormControl, AbstractControl } from '@angular/forms';

// Services
import { UserService } from '../../../services/user.service';
import { User } from '../../../models/user';

@Component({
    selector: 'app-reset-password',
    templateUrl: './reset-password.component.html',
    styleUrls: ['./reset-password.component.scss'],
})
export class ResetPasswordComponent implements OnInit {
  token: string;
  form: FormGroup;
  submit: boolean;
  validated: boolean;
  validationError: boolean;
  error: string;
  user: User;
  reset: boolean;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private formBuilder: FormBuilder
  ) {
  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      if (!params.token) {
        this.router.navigate(['/']);
      }

      this.token = params.token;
      this.validated = false;
      this.validationError = false;

      this.userService.validateResetPasswordToken(this.token).subscribe((user: User) => {
        this.form = this.formBuilder.group({
          email: new FormControl(user.username),
          newPassword: new FormControl(''),
          confirmPassword: new FormControl(''),
        }, {
          validators: this.passwordConfirming
        });
        this.validated = true;
        this.user = user;
      }, () => {
        this.validated = true;
        this.validationError = true;
      });
    });
  }

  passwordConfirming(c: AbstractControl) {
    if (c.get('newPassword').value !== c.get('confirmPassword').value) {
      return {
        passwordNotMatch: true
      };
    }
  }

  onSubmit() {
    if (this.submit) {
      return;
    }

    this.error = '';
    if (this.form.valid) {
      this.submit = true;
      this.userService.resetPassword({
        username: this.user.username,
        password: this.form.value.newPassword,
        resetPasswordToken: this.token
      }).subscribe(() => {
        this.reset = true;
        setTimeout(() => {
          this.router.navigate(['/']);
        }, 2000);
      }, () => {
        this.error = 'Failed to reset the password. Please contact the FBOLinx team!';
        this.submit = false;
      });
    }
  }
}
