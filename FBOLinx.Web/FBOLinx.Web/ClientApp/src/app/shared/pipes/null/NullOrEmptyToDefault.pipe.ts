import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'NullOrEmptyToDefault'
})
export class NullOrEmptyToDefault implements PipeTransform {

  transform(value: any, args?: any): any {
    return (value)?value:'Unknown';
  }

}
