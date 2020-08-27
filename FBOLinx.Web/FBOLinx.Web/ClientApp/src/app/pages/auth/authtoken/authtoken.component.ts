import { Component } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";

// Services
import { AuthenticationService } from "../../../services/authentication.service";

@Component({
    selector: "app-authtoken",
    templateUrl: "./authtoken.component.html",
    styleUrls: ["./authtoken.component.scss"],
})
export class AuthtokenComponent {
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService
    ) {
        // Check for passed in id
        const token = this.route.snapshot.paramMap.get("token");
        if (!token || token === "") {
            this.router.navigate(["/"]);
        } else {
            this.authenticationService
                .preAuth(token)
                // .pipe(first())
                .subscribe(
                    (data) => {
                        if (data.role === 3 || data.role === 2) {
                            this.router.navigate(["/default-layout/fbos/"]);
                        } else {
                            this.router.navigate([
                                "/default-layout/dashboard-fbo/",
                            ]);
                        }
                    },
                    () => {
                        this.router.navigate(["/landing-site"]);
                    }
                );
        }
    }
}
