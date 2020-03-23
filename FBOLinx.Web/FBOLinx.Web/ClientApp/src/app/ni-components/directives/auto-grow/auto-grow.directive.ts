import {
    Directive,
    ElementRef,
    HostListener,
    AfterContentChecked,
    HostBinding,
} from "@angular/core";

@Directive({
    selector: "[autoGrow]",
    host: {
        "(input)": "setHeight()"
    }
})
export class AutoGrowDirective implements AfterContentChecked {
    @HostListener("input", ["$event.target"])
    input = "setHeight()";
    onInput(textArea: HTMLTextAreaElement): void {
        this.adjust();
    }

    constructor(public element: ElementRef) {}

    ngAfterContentChecked(): void {
        this.adjust();
    }

    adjust(): void {
        const nativeElement = this.element.nativeElement;
        nativeElement.style.overflow = "hidden";
        nativeElement.style.height = "auto";
        nativeElement.style.height = nativeElement.scrollHeight + "px";
    }
}
