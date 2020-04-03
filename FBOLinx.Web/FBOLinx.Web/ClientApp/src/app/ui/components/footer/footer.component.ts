import { Component, OnInit, HostBinding } from "@angular/core";
import { AppService } from "../../../services/app.service";

@Component({
    selector: "app-footer",
    templateUrl: "./footer.component.html",
    styleUrls: ["./footer.component.scss"],
    host: {class: "app-footer"},
})
export class FooterComponent implements OnInit {
    public version: string;

    constructor(private appService: AppService) {
        this.getAppVersion();
    }

    ngOnInit() {}

    private getAppVersion() {
        this.appService.getVersion().subscribe((data: any) => {
            this.version = data.version;
        });
    }
}
