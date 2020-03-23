import { Component, OnInit, HostBinding } from "@angular/core";

@Component({
    selector: "app-footer",
    templateUrl: "./footer.component.html",
    styleUrls: ["./footer.component.scss"],
})
export class FooterComponent implements OnInit {
    @HostBinding("class") class = "app-footer";
    constructor() {}

    ngOnInit() {}
}
