import { Component, OnInit, Input, HostBinding } from "@angular/core";

@Component({
    selector: "[ni-button]",
    templateUrl: "./ni-button.component.html",
    styleUrls: ["./ni-button.component.scss"],
})
export class NiButtonComponent implements OnInit {
    @HostBinding("class.ni-btn") niBtn = "true";
    @HostBinding("[class.ni-btn-block]") niBtnBlock = "block";
    @HostBinding("[class.ni-btn-left]") niBtnLeft = 'align === "left"';
    @HostBinding("[class.ni-btn-right]") niBtnRight = 'align === "right"';
    @HostBinding("[class.ni-btn-large]") niBtnLarge = 'size === "large"';
    @HostBinding("[class.ni-btn-small]") niBtnSmall = 'size === "small"';
    @HostBinding("[class.ni-btn-default]") niBtnDefault = 'view === "default"';
    @HostBinding("[class.ni-btn-accent]") niBtnAccent = 'view === "accent"';
    @HostBinding("[class.ni-btn-info]") niBtnInfo = 'view === "info"';
    @HostBinding("[class.ni-btn-success]") niBtnSuccess = 'view === "success"';
    @HostBinding("[class.ni-btn-warning]") niBtnWarning = 'view === "warning"';
    @HostBinding("[class.ni-btn-error]") niBtnError = 'view === "error"';
    @HostBinding("[class.ni-btn-blank]") niBtnBlank = 'view === "blank"';
    @HostBinding("[class.ni-btn-customButton]") niBtnCustomButton =
        'padding === "padding"';
    @HostBinding("[class.ni-btn-outline]") niBtnOutline = "outline";
    @HostBinding("[class.ni-btn-gradient]") niBtnGradient = "gradient";
    @HostBinding("[class.ni-btn-disabled]") niBtnDisabled = "disabled";
    @HostBinding("[class.ni-btn-icon-animation]") niBtnIconAnimation =
        "iconAnimation";
    @HostBinding("[style.border-radius]") styleBorderRadius = "shape";
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
