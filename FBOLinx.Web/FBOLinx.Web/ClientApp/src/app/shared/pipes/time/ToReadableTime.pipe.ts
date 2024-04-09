import { Pipe, PipeTransform } from '@angular/core';
import { defaultStringsEnum } from 'src/app/enums/strings.enums';

@Pipe({
  name: 'ToReadableTime'
})
export class ToReadableTimePipe implements PipeTransform {

  transform(value: any, defaultString: defaultStringsEnum = defaultStringsEnum.empty): any {
    if(!value || value == null) return defaultString;
    value = value.split(/[^*\d]+/);
    return value[0]+':'+value[1];
  }
}
