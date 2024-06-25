import { Pipe, PipeTransform } from '@angular/core';
import { localStorageAccessConstant } from 'src/app/constants/LocalStorageAccessConstant';

@Pipe({
  name: 'decimalPrecision'
})
export class DecimalPrecisionPipe implements PipeTransform {

  transform(value: any, args?: any): any {
    let numberValue = Number(value);
    if(numberValue == null || isNaN(numberValue)) return value;

    let precision = Number(localStorage.getItem(localStorageAccessConstant.decimalPrecision));
    if (precision) {
      return numberValue.toFixed(precision);
    }
    return numberValue.toFixed(4);
    }
}
