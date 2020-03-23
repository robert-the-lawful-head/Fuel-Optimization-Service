import { Component, OnInit, Input } from "@angular/core";

@Component({
    selector: "ni-file",
    templateUrl: "./ni-file.component.html",
    styleUrls: ["./ni-file.component.scss"],
})
export class NiFileComponent implements OnInit {
    @Input() title = "no-name";
    @Input() type = "*";
    @Input() image = "";
    @Input() size = "normal";
    @Input() delete = false;
    @Input() spinner = false;
    @Input() link: any = false;

    constructor() {}

    ngOnInit() {}
}
