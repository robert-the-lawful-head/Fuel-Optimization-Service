import { Directive, HostBinding, Input, OnInit } from '@angular/core';

@Directive({
    selector: '[gradient]',
})
export class GradientDirective implements OnInit {
    @Input() gradient: string[];
    public firstColor: string;
    public secondColor: string;
    public linearGradient: string;

    constructor() {}

    ngOnInit() {
        this.firstColor = this.gradient[0];
        this.secondColor = this.gradient[1];
        this.linearGradient =
            'linear-gradient(to right, ' +
            this.firstColor +
            ' 0%, ' +
            this.secondColor +
            ' 51%, ' +
            this.firstColor +
            ' 100%)';
    }

    @HostBinding('style.backgroundImage') get getGradient() {
        return this.linearGradient;
    }

    @HostBinding('class.custom-gradient') get getClass() {
        return true;
    }
}
