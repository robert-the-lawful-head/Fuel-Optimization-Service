import { Component, OnInit } from "@angular/core";
import { SharedService } from "../../layouts/shared-service";

const BREADCRUMBS: any[] = [
    {
        title: "UI Elements",
        link: "#",
    },
    {
        title: "Typography",
        link: "",
    },
];

@Component({
    moduleId: module.id,
    selector: "page-typography",
    templateUrl: "typography.component.html",
})
export class PageTypographyComponent implements OnInit {
    pageTitle = "Typography";
    breadcrumb: any[] = BREADCRUMBS;

    constructor(private sharedService: SharedService) {
        this.sharedService.titleChange(this.pageTitle);
    }

    ngOnInit() {}
}
