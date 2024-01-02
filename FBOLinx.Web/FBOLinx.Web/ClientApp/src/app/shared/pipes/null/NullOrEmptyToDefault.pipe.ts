import { Pipe, PipeTransform } from '@angular/core';
import { defaultStringsEnum } from 'src/app/enums/strings.enums';

@Pipe({
  name: 'NullOrEmptyToDefault'
})
export class NullOrEmptyToDefault implements PipeTransform {

  transform(value: any, defaultString: defaultStringsEnum = defaultStringsEnum.unknown): any {
    return (value)?value:defaultString;
  }

}
