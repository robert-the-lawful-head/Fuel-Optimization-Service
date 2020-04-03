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
        this.sharedService.titleChange(this.pageTitle);
    }

    ngOnInit() {}

    onSubmit() {}
}
