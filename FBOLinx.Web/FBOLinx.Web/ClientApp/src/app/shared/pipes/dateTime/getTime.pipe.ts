import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'getTime'
})
export class GetTimePipe implements PipeTransform {

  transform(date?: any, args?: any): any {
    if(date == null) return "";

    let dateObject = new Date();

    if (typeof date === 'string' || date instanceof String) {
        if (date === null || date.trim() === "") return "";
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
