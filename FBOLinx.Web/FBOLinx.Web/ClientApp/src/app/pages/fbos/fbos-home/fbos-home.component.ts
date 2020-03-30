import { Component, OnInit, Input } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { Router } from "@angular/router";

// Services
import { FbosService } from "../../../services/fbos.service";
import { FboairportsService } from "../../../services/fboairports.service";
import { SharedService } from "../../../layouts/shared-service";

@Component({
    selector: "app-fbos-home",
    templateUrl: "./fbos-home.component.html",
    styleUrls: ["./fbos-home.component.scss"],
})
export class FbosHomeComponent implements OnInit {
    @Input() groupInfo: any;

    // Public Members
    public fbosData: Array<any>;
    public currentFbo: any;
    public currentFboAirport: any;
    public airportData: Array<any>;

    constructor(
        private router: Router,
        private fboService: FbosService,
        private fboAirportsService: FboairportsService,
        public newFboDialog: MatDialog,
        private sharedService: SharedService
    ) {
        this.currentFbo = null;
        this.currentFboAirport = null;
    }

    ngOnInit() {
        this.loadInitialData();
    }

    public editFboClicked(record) {
        if (!this.groupInfo) {
            this.router.navigate(["/default-layout/fbos/" + record.oid]);
        } else {
            this.fboService
                .get(record)
                .subscribe((data: any) => (this.currentFbo = data));
            this.fboAirportsService
                .getForFbo(record)
                .subscribe((data: any) => (this.currentFboAirport = data));
        }
    }

    public deleteFboClicked(record) {}

    public saveFboEditClicked() {
        this.currentFboAirport = null;
        this.currentFbo = null;
    }

    public cancelFboEditClicked() {
        this.currentFbo = null;
    }

    // Private Methods
    private loadInitialData() {
        if (!this.groupInfo && this.sharedService.currentUser.role === 3) {
            this.loadAllFbos();
        } else {
            this.loadAllFbosForGroup();
        }
    }

    private loadAllFbos() {
        this.fboService
            .getAllFbos()
            .subscribe((data: any) => (this.fbosData = data));
    }

    private loadAllFbosForGroup() {
        if (!this.groupInfo) {
            this.fboService
                .getForGroup(this.sharedService.currentUser.groupId)
                .subscribe((data: any) => (this.fbosData = data));
        } else {
            this.fboService
                .getForGroup(this.groupInfo.oid)
                .subscribe((data: any) => (this.fbosData = data));
        }
    }
}
