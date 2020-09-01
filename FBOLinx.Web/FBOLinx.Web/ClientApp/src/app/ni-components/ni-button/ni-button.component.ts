import { Component, OnInit, Input } from "@angular/core";

@Component({
    selector: "[ni-button]",
    templateUrl: "./ni-button.component.html",
    styleUrls: ["./ni-button.component.scss"],
    host: {
        "[class.ni-btn]": "true",
        "[class.ni-btn-block]": "block",
        "[class.ni-btn-left]": 'align === "left"',
        "[class.ni-btn-right]": 'align === "right"',
        "[class.ni-btn-large]": 'size === "large"',
        "[class.ni-btn-small]": 'size === "small"',
        "[class.ni-btn-default]": 'view === "default"',
        "[class.ni-btn-accent]": 'view === "accent"',
        "[class.ni-btn-info]": 'view === "info"',
        "[class.ni-btn-success]": 'view === "success"',
        "[class.ni-btn-warning]": 'view === "warning"',
        "[class.ni-btn-error]": 'view === "error"',
        "[class.ni-btn-blank]": 'view === "blank"',
        "[class.ni-btn-customButton]": 'padding === "padding"',
        "[class.ni-btn-outline]": "outline",
        "[class.ni-btn-gradient]": "gradient",
        "[class.ni-btn-disabled]": "disabled",
        "[class.ni-btn-icon-animation]": "iconAnimation",
        "[style.border-radius]": "shape",
    },
})
export class NiButtonComponent implements OnInit {
    @Input() block = false;
    @Input() gradient = false;
    @Input() disabled = false;
    @Input() outline = false;
    @Input() lineStyle: string;
    @Input() align = "center";
    @Input() size = "default";
    @Input() view = "default";
    @Input() padding = "default";
    @Input() shape: number | string;
    @Input() beforeIcon: string;
    @Input() afterIcon: string;
    @Input() iconAnimation = false;

    constructor() {}

    ngOnInit() {}
}
