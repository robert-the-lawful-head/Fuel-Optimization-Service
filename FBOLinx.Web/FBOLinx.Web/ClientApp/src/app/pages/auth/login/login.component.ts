import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { FormBuilder, FormGroup, FormControl } from "@angular/forms";

// Services
import { AuthenticationService } from "../../../services/authentication.service";
import { UserService } from "../../../services/user.service";


@Component({
    selector: "app-login",
    templateUrl: "./login.component.html",
    styleUrls: ["./login.component.scss"],
})
export class LoginComponent implements OnInit {
    partner: string;
    redirectTo: string;
    loginForm: FormGroup;
    error: string;
    submit: boolean;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private userService: UserService,
        private formBuilder: FormBuilder
    ) {
        this.loginForm = this.formBuilder.group({
            username: new FormControl(""),
            password: new FormControl(""),
        });
    }

    ngOnInit() {
        this.route.queryParams.subscribe(params => {
            if (!params.partner || !params.redirectTo) {
                this.router.navigate(["/"]);
            }
            this.partner = params.partner;
            this.redirectTo = params.redirectTo;
        });
    }

    public onSubmit() {
        if (this.submit) return;

        this.error = "";
        if (this.loginForm.valid) {
            this.submit = true;
            this.authenticationService.accessToken({
                username: this.loginForm.value.username,
                password: this.loginForm.value.password,
                partnerId: this.partner
            }).subscribe((token: any) => {
                window.location.href = this.redirectTo + "?accessToken=" + token.accessToken;
            }, (err: any) => {
                console.log(err);
                this.error = err;
                this.submit = false;
            }, () => {
            })
        }
    }
}
