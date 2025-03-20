import { Injectable } from '@angular/core';
import { UntypedFormControl } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class FormValidationHelperService {

    constructor() { }
    public noWhitespaceValidator(control: UntypedFormControl) {
        return (control.value || '' ).trim().length? null : { 'whitespace': true };
    }
}
