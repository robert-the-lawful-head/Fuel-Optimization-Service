import { Component } from "@angular/core";
// import {
//    trigger,
//    state,
//    style,
//    animate,
//    transition
//    } from '@angular/animations';

// @Component({
//  selector: 'app-root',
//  templateUrl: './app.component.html',
//  styleUrls: ['./app.component.css']
// })
// export class AppComponent {
//  title = 'app';
// }

@Component({
    moduleId: module.id,
    selector: "app",
    template: `<router-outlet></router-outlet>`,
})
export class AppComponent {}
