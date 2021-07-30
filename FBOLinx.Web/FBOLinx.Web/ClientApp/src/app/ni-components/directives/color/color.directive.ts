import {
    Directive,
    HostBinding,
    HostListener,
    Input,
    OnInit,
} from '@angular/core';

@Directive({
    selector: '[color]',
})
export class ColorDirective implements OnInit {
    @Input() color: string | string[];
    public currentColor: string;
    public defaultColor: string;
    public hoveredColor: string;

    constructor() {}

    ngOnInit() {
        this.defaultColor =
            typeof this.color === 'string' ? this.color : this.color[0];
        this.hoveredColor =
            typeof this.color === 'string' ? this.color : this.color[1];
        this.currentColor = this.defaultColor;
    }

    @HostListener('mouseenter') onMouseEnter() {
        this.currentColor = this.hoveredColor;
    }

    @HostListener('mouseleave') onMouseLeave() {
        this.currentColor = this.defaultColor;
    }

    @HostBinding('style.color') get getColor() {
        return this.currentColor;
    }

    @HostBinding('class.custom-color') get getClass() {
        return true;
    }


}
