import { Component, EventEmitter, Input, Output, OnInit } from "@angular/core";
import { Router, ActivatedRoute, ParamMap } from "@angular/router";

// Services
import { GroupsService } from "../../../services/groups.service";
import { SharedService } from "../../../layouts/shared-service";
import { MatSnackBar } from '@angular/material/snack-bar';

const BREADCRUMBS: any[] = [
    {
        title: "Main",
        link: "#/default-layout",
    },
    {
        title: "Groups",
        link: "#/default-layout/groups",
    },
    {
        title: "Edit Group",
        link: "",
    },
];

@Component({
    selector: "app-groups-edit",
    templateUrl: "./groups-edit.component.html",
    styleUrls: ["./groups-edit.component.scss"],
})
export class GroupsEditComponent implements OnInit {
    @Output() cancelClicked = new EventEmitter<any>();
    @Input() groupInfo: any;

    // Public Members
    public pageTitle = "Edit FBO";
    public breadcrumb: any[] = BREADCRUMBS;
    public currentContact: any;
    public contactsData: any;

    // Private Members
    private selectedContactRecord: any;
    private requiresRouting = false;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private groupsService: GroupsService,
        private snackBar: MatSnackBar,
    ) {}

    ngOnInit() {
        const id = this.route.snapshot.paramMap.get("id");
        if (!id) {
            this.requiresRouting = false;
        } else {
            this.requiresRouting = true;
            this.groupsService.get({ oid: id }).subscribe((data: any) => {
                this.groupInfo = data;
            });
        }
    }

    // Public Methods
    public saveEdit() {
        this.groupsService.update(this.groupInfo).subscribe(() => {
            this.snackBar.open("Successfully updated!", "", {
                duration: 2000,
                panelClass: ["blue-snackbar"],
            });
        });
    }

    public cancelEdit() {
        this.router.navigate(["/default-layout/groups/"]);
    }
}
