import { Pipe, PipeTransform } from '@angular/core';
import { localStorageAccessConstant } from 'src/app/constants/LocalStorageAccessConstant';

@Pipe({
  name: 'decimalPrecision'
})
export class DecimalPrecisionPipe implements PipeTransform {

  transform(value: any, forceDecimals: boolean = false): any {
    let numberValue = Number(value);
    if((numberValue == null || isNaN(numberValue) || Number.isInteger(value)) && !forceDecimals) return value;

    let precision = Number(localStorage.getItem(localStorageAccessConstant.decimalPrecision));
    if (precision) {
      return numberValue.toFixed(precision);
    }
    return numberValue.toFixed(4);
    }
}
