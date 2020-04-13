import {
    Component,
    OnInit,
    Input,
} from "@angular/core";
import { trigger, state, style, transition, animate } from '@angular/animations';


@Component({
    selector: "ni-card",
    templateUrl: "./ni-card.component.html",
    styleUrls: ["./ni-card.component.scss"],
    animations: [
        trigger('openClose', [
            state('true', style({
                height: '*',
                paddingBottom: '15px',
                paddingTop: '15px'
            })),
            state('false', style({
                height: '0',
                paddingBottom: '0',
                paddingTop: '0'
            })),
            transition('false <=> true', [ animate(300) ])
        ])
    ],
    host: {
        "[class.ni-card]": "true",
    },
})
export class NiCardComponent implements OnInit {
    @Input() title = "";
    @Input() subtitle = "";

    @Input() bgColor = "";
    @Input() color = "";
    @Input() headerBgColor = "";
    @Input() headerColor = "";

    @Input() outline = false;

    @Input() theme = "";
    @Input() align = "";

    @Input() customStyle = "";

    @Input() collapsible = false;

    @Input() opened = false;

    constructor() {}

    ngOnInit() {
    }

    public headerClick() {
        if (this.collapsible) {
            this.opened = !this.opened;
        }
    }
}
