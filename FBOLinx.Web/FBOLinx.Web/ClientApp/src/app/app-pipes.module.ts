import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { BooleanToTextPipe } from './shared/pipes/boolean/booleanToText.pipe';
import { NullOrEmptyToDefault } from './shared/pipes/null/NullOrEmptyToDefault.pipe';
import { ToPriceFormatPipe } from './shared/pipes/price/ToPriceFormat.pipe';

import { SafeHtmlPipe } from './shared/pipes/safe-html-pipe.pipe';

@NgModule({
    declarations: [SafeHtmlPipe,BooleanToTextPipe,NullOrEmptyToDefault,ToPriceFormatPipe],
    exports: [SafeHtmlPipe,BooleanToTextPipe,NullOrEmptyToDefault,ToPriceFormatPipe],
    imports: [CommonModule],
})
export class AppPipesModule {}
