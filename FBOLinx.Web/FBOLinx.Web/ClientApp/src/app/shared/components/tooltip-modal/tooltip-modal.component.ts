import { Component, Inject, EventEmitter, Output } from "@angular/core";
import { Popover } from "../popover";
import { SharedService } from "../../../layouts/shared-service";

@Component({
    selector: "app-tooltip-modal",
    templateUrl: "./tooltip-modal.component.html",
    styleUrls: ["./tooltip-modal.component.scss"],
    providers: [SharedService],
})
// close-confirmation component
export class TooltipModalComponent {
    public data: any;

    constructor(public popover: Popover, public sharedService: SharedService) {}

    close() {
        if (this.data.proceed) {
            this.popover.emitClose("proceed");
        }
        this.popover.close();
    }

    setProperty(data: any) {
        this.data = data;
        if (!data.okText) {
            this.data.okText = "Got it";
        }
    }
}
