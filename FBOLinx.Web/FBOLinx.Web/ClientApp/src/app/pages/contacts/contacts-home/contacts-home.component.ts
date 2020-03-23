import {
    Component,
    EventEmitter,
    Input,
    Output,
    OnInit,
    ViewChild,
} from "@angular/core";

// Services
import { ContactsService } from "../../../services/contacts.service";

@Component({
    selector: "app-contacts-home",
    templateUrl: "./contacts-home.component.html",
    styleUrls: ["./contacts-home.component.scss"],
})
export class ContactsHomeComponent {
    @Input() contactsData: Array<any>;

    constructor(private contactsService: ContactsService) {}
}
