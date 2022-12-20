import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'booleanTryCastToText'
})
export class BooleanToTextPipe implements PipeTransform {

  transform(value: any): string {
    if(this.isBoolean(value))
        return (value)?'Yes':'No';
    else
        return (value.toLowerCase() == 'yes' || value.toLowerCase() == 'true')?'Yes':'No';
  }
  private isBoolean(input) {
    return input === false || input === true;
  }
}
