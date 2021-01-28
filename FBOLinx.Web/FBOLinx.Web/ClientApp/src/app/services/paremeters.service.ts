import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable()
export class Parametri {
    private param = new BehaviorSubject('default');

    getMessage() {
        return this.param.asObservable();
    }

    updateMessage(message: string) {
        this.param.next(message);
    }
}
