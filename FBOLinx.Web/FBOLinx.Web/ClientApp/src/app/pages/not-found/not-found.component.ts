import { Component, OnInit } from "@angular/core";
import { SharedService } from "../../layouts/shared-service";

@Component({
    moduleId: module.id,
    selector: "page-not-found",
    templateUrl: "not-found.component.html",
})
export class PageNotFoundComponent implements OnInit {
    pageTitle = "Not found!";

    constructor(private sharedService: SharedService) {
        this.sharedService.emitChange(this.pageTitle);
    }

    ngOnInit() {}
}
