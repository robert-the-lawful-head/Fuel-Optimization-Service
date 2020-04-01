import { Directive, ElementRef, HostListener } from "@angular/core";

@Directive({
    selector: "[clickStopPropagation]"
})
export class ClickStopPropagationDirective {
    constructor(el: ElementRef) {}

    @HostListener("click", ["$event"])
    public onClick(event: any) {
        event.stopPropagation();
    }
}
