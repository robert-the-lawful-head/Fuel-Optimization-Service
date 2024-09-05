import { CurrencyPipe } from '@angular/common';
import { Pipe, PipeTransform, inject } from '@angular/core';
import { localStorageAccessConstant } from 'src/app/constants/LocalStorageAccessConstant';

@Pipe({
  name: 'currencyPresicion'
})
export class CurrencyPresicionPipe implements PipeTransform {

    private currencyPipe = inject(CurrencyPipe);

  transform(value: any, args?: any): any {
    var decimalPrecision = Number(localStorage.getItem(localStorageAccessConstant.decimalPrecision));
    var decimlaPrecisionStringForCurrency = decimalPrecision ? `1.${decimalPrecision}-${decimalPrecision}` : '1.4-4';

    return this.currencyPipe.transform(value, 'USD', 'symbol', decimlaPrecisionStringForCurrency);
  }

}
