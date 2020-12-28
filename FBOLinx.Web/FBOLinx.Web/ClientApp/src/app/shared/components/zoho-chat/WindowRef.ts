import { Injectable } from '@angular/core';

/***
 * @description: Responsible for returing the window object.
 * @author: Zoho SalesIQ
 */

const getWindow = () =>
   // return the global native browser window object
    window;


@Injectable()
export class WindowRef {
   get nativeWindow(): any {
      return getWindow();
   }
}
