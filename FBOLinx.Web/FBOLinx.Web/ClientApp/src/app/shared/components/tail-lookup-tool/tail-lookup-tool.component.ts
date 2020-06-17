import {
    Component,
} from "@angular/core";
import { SharedService } from "../../../layouts/shared-service";


@Component({
    selector: "tail-lookup-tool",
    templateUrl: "./tail-lookup-tool.component.html",
    styleUrls: ["./tail-lookup-tool.component.scss"],
    providers: [SharedService],
})
export class TailLookupToolComponent {
    public aircraftTailNumber: any;
    public fuelVolume: any;

    constructor() { }

    LookupItem() {
        console.log(this.aircraftTailNumber);
    }
}
