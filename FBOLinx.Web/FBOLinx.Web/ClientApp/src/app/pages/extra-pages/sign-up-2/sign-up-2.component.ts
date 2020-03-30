import { Component, OnInit } from "@angular/core";
import { SharedService } from "../../../layouts/shared-service";

@Component({
    selector: "page-sign-up-2",
    templateUrl: "./sign-up-2.component.html",
    styleUrls: ["./sign-up-2.component.scss"],
})
export class PageSignUp2Component implements OnInit {
    pageTitle = "Sign Up";

    constructor(private sharedService: SharedService) {
        this.sharedService.emitChange(this.pageTitle);
    }

    ngOnInit() {}

    onSubmit() {}
}
