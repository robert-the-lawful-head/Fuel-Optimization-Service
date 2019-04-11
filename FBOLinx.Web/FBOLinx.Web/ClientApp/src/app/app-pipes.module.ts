import { NgModule } from '@angular/core';

import {SafeHtmlPipe} from './shared/pipes/safe-html-pipe.pipe';

@NgModule({
    imports: [],
    declarations: [SafeHtmlPipe],
    exports: [SafeHtmlPipe],
})

@NgModule({})
export class AppPipesModule {
    static forRoot() {
        return {
            ngModule: AppPipesModule,
            providers: [],
        };
    }
}
