import { Component, OnInit, HostBinding } from "@angular/core";

@Component({
    moduleId: module.id,
    selector: "vertical-navbar",
    templateUrl: "vertical-navbar.component.html",
    styleUrls: ["vertical-navbar.component.scss"],
})
export class VerticalNavbarComponent implements OnInit {
    @HostBinding("class") class = "vertical-navbar";
    constructor() {}

    ngOnInit() {}
}
