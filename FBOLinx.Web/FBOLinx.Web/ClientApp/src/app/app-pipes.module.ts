import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { BooleanToTextPipe } from './shared/pipes/boolean/booleanToText.pipe';
import { GetTimePipe } from './shared/pipes/dateTime/getTime.pipe';
import { ToReadableDateTimePipe } from './shared/pipes/dateTime/ToReadableDateTime.pipe';
import { NullOrEmptyToDefault } from './shared/pipes/null/NullOrEmptyToDefault.pipe';
import { ToPriceFormatPipe } from './shared/pipes/price/ToPriceFormat.pipe';
import { SafeHtmlPipe } from './shared/pipes/safe-html-pipe.pipe';
import { ToReadableTimePipe } from './shared/pipes/time/ToReadableTime.pipe';

@NgModule({
    declarations: [SafeHtmlPipe,BooleanToTextPipe,NullOrEmptyToDefault,ToPriceFormatPipe,ToReadableDateTimePipe,ToReadableTimePipe,GetTimePipe],
    exports: [SafeHtmlPipe,BooleanToTextPipe,NullOrEmptyToDefault,ToPriceFormatPipe,ToReadableDateTimePipe,ToReadableTimePipe,GetTimePipe],
    imports: [CommonModule],
})
export class AppPipesModule {}
