import { Component, OnInit, Input } from "@angular/core";

@Component({
    selector: "ni-alert",
    templateUrl: "./ni-alert.component.html",
    styleUrls: ["./ni-alert.component.scss"],
})
export class NiAlertComponent implements OnInit {
    @Input() color = "";
    @Input() customColor = "";
    @Input() outline = false;
    @Input() close = false;

    constructor() {}

    ngOnInit() {}

    delete(event: any, alert: any) {
        event.preventDefault();

        alert.remove();
    }
}
