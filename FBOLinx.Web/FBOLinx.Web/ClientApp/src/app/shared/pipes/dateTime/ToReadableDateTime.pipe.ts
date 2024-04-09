import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({
  name: 'ToReadableDateTime'
})
export class ToReadableDateTimePipe implements PipeTransform {

  transform(dateObject: string, args?: any): string {
    if(dateObject == null) return "";
    return moment(dateObject).format('L');
  }
}
