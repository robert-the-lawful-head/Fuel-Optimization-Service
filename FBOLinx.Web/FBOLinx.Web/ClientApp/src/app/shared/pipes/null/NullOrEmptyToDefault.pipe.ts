import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'NullOrEmptyToDefault'
})
export class NullOrEmptyToDefault implements PipeTransform {

  transform(value: any, UnknowsAsDefault: boolean = true): any {
    if (!UnknowsAsDefault) {
      return (value)?value:'';
    }
    return (value)?value:'Unknown';
  }

}
