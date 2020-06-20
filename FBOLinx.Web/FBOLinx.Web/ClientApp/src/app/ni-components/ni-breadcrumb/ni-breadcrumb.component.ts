import { Component, OnInit, Input } from "@angular/core";
import { Item } from "./item";

@Component({
    selector: "ni-breadcrumb",
    templateUrl: "./ni-breadcrumb.component.html",
    styleUrls: ["./ni-breadcrumb.component.scss"],
})
export class NiBreadcrumbComponent implements OnInit {
    @Input() menu: Item[] = [];
    @Input() separator = "/";
    @Input() style = "default"; // custom1 | custom2

    public breadCrumbClasses: any = {};

    constructor() {}

    ngOnInit() {
        this.breadCrumbClasses = this.getClasses();
    }

    getClasses() {
        return {
            "custom-1": this.style === "custom1",
            "custom-2": this.style === "custom2",
        };
    }
}
