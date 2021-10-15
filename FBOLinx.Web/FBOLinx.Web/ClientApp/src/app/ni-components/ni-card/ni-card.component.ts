import {
    animate,
    state,
    style,
    transition,
    trigger,
} from '@angular/animations';
import {
    AfterContentChecked,
    Component,
    ElementRef,
    HostListener,
    Input,
    ViewChild,
} from '@angular/core';

@Component({
    animations: [
        trigger('openClose', [
            state(
                'true',
                style({
                    height: '*',
                    paddingBottom: '15px',
                    paddingTop: '15px',
                })
            ),
            state(
                'false',
                style({
                    height: '0',
                    paddingBottom: '0',
                    paddingTop: '0',
                })
            ),
            transition('false <=> true', [animate(300)]),
        ]),
    ],
    host: {
        '[class.ni-card]': 'true',
    },
    selector: 'ni-card',
    styleUrls: ['./ni-card.component.scss'],
    templateUrl: './ni-card.component.html',
})
export class NiCardComponent implements AfterContentChecked {
    @ViewChild('cardTitle') cardTitle: ElementRef;
    @ViewChild('cardSubTitle') cardSubTitle: ElementRef;

    @Input() title = '';
    @Input() subtitle = '';

    @Input() bgColor = '';
    @Input() color = '';
    @Input() headerBgColor = '';
    @Input() headerColor = '';

    @Input() outline = false;

    @Input() theme = '';
    @Input() align = '';

    @Input() customStyle = '';

    @Input() collapsible = false;

    @Input() opened = false;

    subTitleVisible = true;

    constructor() {}

    ngAfterContentChecked(): void {
        this.checkSubtitleVisible();
    }

    @HostListener('window:resize')
    onResize() {
        this.checkSubtitleVisible();
    }

    headerClick() {
        if (this.collapsible) {
            this.opened = !this.opened;
        }
    }

    private checkSubtitleVisible() {
        if (this.subtitle && this.cardTitle && this.cardSubTitle) {
            const limit =
                this.cardTitle.nativeElement.offsetLeft +
                this.cardTitle.nativeElement.offsetWidth +
                10;
            const subtitleStart = this.cardSubTitle.nativeElement.offsetLeft;
            if (subtitleStart < limit && this.subTitleVisible) {
                this.subTitleVisible = false;
            }
            if (subtitleStart >= limit && !this.subTitleVisible) {
                this.subTitleVisible = true;
            }
        }
    }
}
