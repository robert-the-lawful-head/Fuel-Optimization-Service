import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SafeHtmlPipe } from './shared/pipes/safe-html-pipe.pipe';

@NgModule({
    declarations: [SafeHtmlPipe],
    exports: [SafeHtmlPipe],
    imports: [CommonModule],
})
export class AppPipesModule {}
