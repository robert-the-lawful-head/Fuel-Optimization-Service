import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { BooleanToTextPipe } from './shared/pipes/boolean/booleanToText.pipe';
import { NullOrEmptyToDefault } from './shared/pipes/null/NullOrEmptyToDefault.pipe';

import { SafeHtmlPipe } from './shared/pipes/safe-html-pipe.pipe';

@NgModule({
    declarations: [SafeHtmlPipe,BooleanToTextPipe,NullOrEmptyToDefault],
    exports: [SafeHtmlPipe,BooleanToTextPipe,NullOrEmptyToDefault],
    imports: [CommonModule],
})
export class AppPipesModule {}
