import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { SharedService } from "../../../layouts/shared-service";

@Component({
    selector: "page-sign-in-2",
    templateUrl: "./sign-in-2.component.html",
    styleUrls: ["./sign-in-2.component.scss"],
})
export class PageSignIn2Component {
    pageTitle = "Sign In";

    constructor(private router: Router, private sharedService: SharedService) {
        this.sharedService.emitChange(this.pageTitle);
    }

    onSubmit() {
        this.router.navigate(["/default-layout/dashboard"]);
    }
}
