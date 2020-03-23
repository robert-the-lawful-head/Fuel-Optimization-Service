import {
    Component,
    OnInit,
    Input,
    HostBinding,
    ElementRef,
} from "@angular/core";
import { NgForm } from "@angular/forms";
import { Message } from "./message";
import { User } from "./user";

@Component({
    selector: "ni-chat",
    templateUrl: "./ni-chat.component.html",
    styleUrls: ["./ni-chat.component.scss"],
    host: {
        "[class.ni-chat]": "true"
    }
})
export class NiChatComponent implements OnInit {
    @Input() contacts: any[] = [];
    @Input() activeUser: User;
    @Input() messages: Message[] = [];
    message: Message;
    firstLetter: string;
    dialogMessages: any;

    constructor(private elementRef: ElementRef) {
        this.dialogMessages = this.elementRef.nativeElement.getElementsByClassName(
            "dialog-messages"
        );
    }

    ngOnInit() {
        this.firstLetter = this.activeUser.name.charAt(0);
        this.scrollToBottom(this.dialogMessages);
    }

    onSubmit(form: NgForm) {
        let messageContent: any = form.value.message;
        const newMessage =
            messageContent !== "" && messageContent !== null
                ? messageContent.replace(/(?:\r\n|\r|\n)/g, "")
                : messageContent;

        if (newMessage !== "" && newMessage !== null) {
            messageContent = messageContent.replace(/(?:\r\n|\r|\n)/g, " ");

            const date: any = new Date();
            const minutes: number = date.getMinutes();

            this.message = {
                date:
                    date.getHours() +
                    " : " +
                    (minutes > 9 ? minutes : "0" + minutes),
                content: messageContent,
                my: true,
            };
            this.messages.push(this.message);

            form.reset();

            const chatDialogMessages: any = this.dialogMessages[0];

            chatDialogMessages.classList.add("add-message");
            setTimeout(() => {
                chatDialogMessages.classList.remove("add-message");
            }, 200);

            // Scroll to bottom
            this.scrollToBottom(this.dialogMessages);
        }
    }

    scrollToBottom(elem: any) {
        const eleArray = Array.prototype.slice.call(elem) as Element[];

        setTimeout(() => {
            eleArray.map((val) => {
                val.scrollTop = val.scrollHeight;
            });
        }, 0);
    }

    loadFile(event) {
        event.stopPropagation();
    }
}
