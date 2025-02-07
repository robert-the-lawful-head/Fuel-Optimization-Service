import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'ToPriceFormatPipe'
})
export class ToPriceFormatPipe implements PipeTransform {

  transform(value: any): any {
      var price = +value;
      if(Number.isNaN(price)) return "N/A";
      return "$"+price.toString();
  }

}
