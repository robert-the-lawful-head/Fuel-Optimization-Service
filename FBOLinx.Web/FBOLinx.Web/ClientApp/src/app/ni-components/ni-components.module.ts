import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgbPopoverModule } from '@ng-bootstrap/ng-bootstrap';

import { BgDirective } from './directives/bg/bg.directive';
import { ColorDirective } from './directives/color/color.directive';
import { GradientDirective } from './directives/gradient/gradient.directive';
import { NiBadgeComponent } from './ni-badge/ni-badge.component';
import { NiBreadcrumbComponent } from './ni-breadcrumb/ni-breadcrumb.component';
import { NiButtonComponent } from './ni-button/ni-button.component';
import { NiCardComponent } from './ni-card/ni-card.component';

@NgModule({
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
    imports: [
        CommonModule,
        FormsModule,
        NgbPopoverModule,
        RouterModule,
    ],
})
export class NiComponentsModule {}
