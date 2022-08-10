import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'ToReadableTime'
})
export class ToReadableTimePipe implements PipeTransform {

  transform(value: any, args?: any): any {
    value = value.split(/[^*\d]+/);
    return value[0]+':'+value[1]+':'+value[2];
  }
}
