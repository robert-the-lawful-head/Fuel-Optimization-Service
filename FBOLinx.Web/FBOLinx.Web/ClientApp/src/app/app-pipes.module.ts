import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {SafeHtmlPipe} from './shared/pipes/safe-html-pipe.pipe';

@NgModule({
    imports: [ CommonModule ],
    declarations: [SafeHtmlPipe],
    exports: [SafeHtmlPipe],
})

@NgModule({})
export class AppPipesModule {
    //static forRoot() {
    //    return {
    //        ngModule: AppPipesModule,
    //        providers: []
    //    };
    //}
}
