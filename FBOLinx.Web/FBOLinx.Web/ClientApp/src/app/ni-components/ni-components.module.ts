import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbPopoverModule } from '@ng-bootstrap/ng-bootstrap';

import { NiBadgeComponent } from './ni-badge/ni-badge.component';
import { NiBreadcrumbComponent } from './ni-breadcrumb/ni-breadcrumb.component';
import { NiButtonComponent } from './ni-button/ni-button.component';
import { NiCardComponent } from './ni-card/ni-card.component';

import { ColorDirective } from './directives/color/color.directive';
import { BgDirective } from './directives/bg/bg.directive';
import { GradientDirective } from './directives/gradient/gradient.directive';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        NgbPopoverModule,
        RouterModule,
    ],
    declarations: [
        NiBadgeComponent,
        NiBreadcrumbComponent,
        NiButtonComponent,
        NiCardComponent,
        ColorDirective,
        BgDirective,
        GradientDirective,
    ],
    exports: [
        NiBadgeComponent,
        NiBreadcrumbComponent,
        NiButtonComponent,
        NiCardComponent,
        ColorDirective,
        BgDirective,
        GradientDirective,
    ],
})
export class NiComponentsModule {}
