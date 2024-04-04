import { Pipe, PipeTransform } from '@angular/core';
import { defaultStringsEnum } from 'src/app/enums/strings.enums';

@Pipe({
  name: 'getTime'
})
export class GetTimePipe implements PipeTransform {

  transform(date?: any, defaultString: defaultStringsEnum = defaultStringsEnum.empty): any {
    if(date == null) return defaultString;

    let dateObject = new Date();

    if (typeof date === 'string' || date instanceof String) {
        if (date === null || date.trim() === "") return defaultString;
        dateObject  = new Date(date.toString());
    }

    var hms:string[] = [
        dateObject.getHours().toString(),
        dateObject.getMinutes().toString()
    ];

    hms[0] = ('0' + hms[0]).slice(-2);
    hms[1] = ('0' + hms[1]).slice(-2);

    return hms.join(':');
  }
}
