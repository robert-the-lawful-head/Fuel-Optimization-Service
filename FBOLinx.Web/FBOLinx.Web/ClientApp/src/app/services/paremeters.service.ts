import { Injectable, Inject } from "@angular/core";
import { Subject, Observable, BehaviorSubject } from "rxjs";

@Injectable()
export class Parametri {
    private param = new BehaviorSubject("default");

    getMessage() {
        return this.param.asObservable();
    }

    updateMessage(message: string) {
        this.param.next(message);
    }
}
