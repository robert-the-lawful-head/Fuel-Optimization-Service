import { Injectable } from '@angular/core';
import { localStorageAccessConstant } from 'src/app/constants/LocalStorageAccessConstant';

@Injectable({
  providedIn: 'root'
})
export class StringHelperService {

constructor() { }
    getNumberInputStepDefaultValue(){
    let storedDecimalSettings = localStorage.getItem(localStorageAccessConstant.decimalPrecision) ?? 4;
    let decimalPrecision = Number(storedDecimalSettings) ?? 4;
    return "0." + "0".repeat(decimalPrecision - 1) + "1";
    }
}
