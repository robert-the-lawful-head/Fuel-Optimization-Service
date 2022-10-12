import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FlightWatchHelper {

constructor() { }

    getSlashSeparationDisplayString(e1: any,e2: any){
        let str = (e1)?e1:"";
        str += (e1 && e2)?"/":"";
        str += (e2)?e2:"";
        return str;
    }
    getEmptyorDefaultStringText(str: string){
        return str == ""  ? "Unknown" : str ;
    }

}
