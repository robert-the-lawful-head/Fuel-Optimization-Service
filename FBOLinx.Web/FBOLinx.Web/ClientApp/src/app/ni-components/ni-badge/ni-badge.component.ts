import { Component, OnInit, Input } from "@angular/core";

@Component({
    selector: "ni-badge",
    templateUrl: "./ni-badge.component.html",
    styleUrls: ["./ni-badge.component.scss"],
})
export class NiBadgeComponent implements OnInit {
    @Input() color = "";
    @Input() customColor = "";
    @Input() outline = false;
    @Input() borderRadius = true;
    @Input() arrow = "";
    @Input() size = "";
    @Input() position = "";

    public badgeClasses: any = {};
    public arrowClasses: any = {};
    public badgeStyles: any = {};

    constructor() {}

    ngOnInit() {
        this.badgeClasses = this.getClasses();
        this.arrowClasses = this.getArrowClasses();
        this.badgeStyles = this.getStyles();
    }

    getClasses() {
        return {
            "success-badge": this.color === "success",
            "info-badge": this.color === "info",
            "warning-badge": this.color === "warning",
            "danger-badge": this.color === "danger",
            "custom-badge": this.customColor,
            "outline-badge": this.outline,
            "border-radius-badge": this.borderRadius,
            "arrow-right-badge": this.arrow === "right",
            "arrow-left-badge": this.arrow === "left",
            "large-badge": this.size === "large",
            "medium-badge": this.size === "medium",
            "top-left": this.position === "top-left",
            "bottom-left": this.position === "bottom-left",
            "bottom-right": this.position === "bottom-right",
        };
    }
    getStyles() {
        return {
            "background-color": this.customColor,
            "border-color": this.customColor,
            color: this.customColor,
        };
    }
    getArrowClasses() {
        return {
            "arrow-top": this.arrow === "top",
            "arrow-right": this.arrow === "right",
            "arrow-bottom": this.arrow === "bottom",
            "arrow-left": this.arrow === "left",
        };
    }
}
