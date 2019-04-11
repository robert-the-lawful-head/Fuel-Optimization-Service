import { Component } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { AuthenticationService } from '../../../services/authentication.service';
import { UserService } from '../../../services/user.service';

@Component({
    selector: 'app-authtoken',
    templateUrl: './authtoken.component.html',
    styleUrls: ['./authtoken.component.scss']
})
/** authtoken component*/
export class AuthtokenComponent {
    /** authtoken ctor */
    constructor(private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private userService: UserService) {

        //Check for passed in id
        let token = this.route.snapshot.paramMap.get('token');
        if (!token || token == '') {
            this.router.navigate(['/']);
        } else {
            this.authenticationService.preAuth(token)
                //.pipe(first())
                .subscribe(
                    data => {
                        if (data.role == 3 || data.role == 2)
                            this.router.navigate(['/default-layout/fbos/']);
                        else
                            this.router.navigate(['/default-layout/dashboard-fbo/']);
                    },
                    error => {
                        this.router.navigate(['/landing-site']);
                    });
        }
    }
}
