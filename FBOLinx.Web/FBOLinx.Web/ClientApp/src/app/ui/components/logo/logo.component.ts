import { Component, OnInit, HostBinding } from "@angular/core";

@Component({
    moduleId: module.id,
    selector: "logo",
    templateUrl: "logo.component.html",
    styleUrls: ["logo.component.scss"],
})
export class LogoComponent implements OnInit {
    @HostBinding("class") class = "app-logo";
    constructor() {}

    ngOnInit() {}
}
