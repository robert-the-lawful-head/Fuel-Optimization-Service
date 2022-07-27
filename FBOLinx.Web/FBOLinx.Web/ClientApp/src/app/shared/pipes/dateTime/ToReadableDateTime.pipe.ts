import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'ToReadableDateTime'
})
export class ToReadableDateTimePipe implements PipeTransform {

  transform(dateObject: any, args?: any): string {
    console.log("🚀 ~ file: ToReadableDateTime.pipe.ts ~ line 9 ~ ToReadableDateTimePipe ~ transform ~ dateObject", dateObject)
    dateObject =  new Date(dateObject);
    console.log("🚀 ~ file: ToReadableDateTime.pipe.ts ~ line 9 ~ ToReadableDateTimePipe ~ transform ~ dateObject", dateObject)

    if(dateObject == null) return "";
    var dmy:string[] = [
        dateObject.getDate().toString(),
        (dateObject.getMonth() + 1).toString(),
        dateObject.getFullYear().toString() 
    ];
    var hms:string[] = [
        dateObject.getHours().toString(),
        dateObject.getMinutes().toString(), 
        dateObject.getSeconds().toString()   
    ];

    dmy[0] = ('0' + dmy[0]).slice(-2);
    dmy[1] = ('0' + dmy[1]).slice(-2);
    dmy[2] = ('0000' + dmy[2]).slice(-4);

    hms[0] = ('0' + hms[0]).slice(-2);
    hms[1] = ('0' + hms[1]).slice(-2);
    hms[2] = ('0' + hms[2]).slice(-2);

    return dmy.join('/') +' '+ hms.join(':');
  }
}
