import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'getTime'
})
export class GetTimePipe implements PipeTransform {

  transform(dateObject: Date, args?: any): any {
    if(dateObject == null) return "";
    var hms:string[] = [
        dateObject.getHours().toString(),
        dateObject.getMinutes().toString()
    ];

    hms[0] = ('0' + hms[0]).slice(-2);
    hms[1] = ('0' + hms[1]).slice(-2);

    return hms.join(':');
  }
}
