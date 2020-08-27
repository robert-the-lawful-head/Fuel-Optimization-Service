import {
    Component,
    Input,
} from "@angular/core";

@Component({
    selector: "app-contacts-home",
    templateUrl: "./contacts-home.component.html",
    styleUrls: ["./contacts-home.component.scss"],
})
export class ContactsHomeComponent {
    @Input() contactsData: Array<any>;

    constructor() {}
}
