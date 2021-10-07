import { Component, Input, OnInit } from '@angular/core';

@Component({
    selector: 'ni-badge',
    styleUrls: ['./ni-badge.component.scss'],
    templateUrl: './ni-badge.component.html',
})
export class NiBadgeComponent implements OnInit {
    @Input() color = '';
    @Input() customColor = '';
    @Input() outline = false;
    @Input() borderRadius = true;
    @Input() arrow = '';
    @Input() size = '';
    @Input() position = '';

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
            'arrow-left-badge': this.arrow === 'left',
            'arrow-right-badge': this.arrow === 'right',
            'border-radius-badge': this.borderRadius,
            'bottom-left': this.position === 'bottom-left',
            'bottom-right': this.position === 'bottom-right',
            'custom-badge': this.customColor,
            'danger-badge': this.color === 'danger',
            'info-badge': this.color === 'info',
            'large-badge': this.size === 'large',
            'medium-badge': this.size === 'medium',
            'outline-badge': this.outline,
            'success-badge': this.color === 'success',
            'top-left': this.position === 'top-left',
            'warning-badge': this.color === 'warning',
        };
    }
    getStyles() {
        return {
            'background-color': this.customColor,
            'border-color': this.customColor,
            color: this.customColor,
        };
    }
    getArrowClasses() {
        return {
            'arrow-bottom': this.arrow === 'bottom',
            'arrow-left': this.arrow === 'left',
            'arrow-right': this.arrow === 'right',
            'arrow-top': this.arrow === 'top',
        };
    }
}
