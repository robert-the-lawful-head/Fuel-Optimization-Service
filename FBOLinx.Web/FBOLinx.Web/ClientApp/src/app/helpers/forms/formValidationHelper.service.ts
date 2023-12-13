import { Injectable } from '@angular/core';
import { FormControl } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class FormValidationHelperService {

    constructor() { }
    public noWhitespaceValidator(control: FormControl) {
        return (control.value || '' ).trim().length? null : { 'whitespace': true };
    }
}
