import {
    Injectable,
    Injector,
    ComponentFactoryResolver,
    EmbeddedViewRef,
    ApplicationRef,
    ComponentRef,
    EventEmitter
} from '@angular/core';
import { Subject } from 'rxjs';

import { PopoverComponent } from './popover.component';
import {
    Overlay,
    OverlayProperties,
    EventService as OverlayEventService
} from '@ivylab/overlay-angular';
import { PopoverProperties } from './interfaces';
import { defaultProperties } from './default-properties';

@Injectable()
export class Popover {
    _defaultProperties: OverlayProperties;
    emitChangeSource = new Subject();
    // Observable string streams
    changeEmitted$ = this.emitChangeSource.asObservable();

    constructor(
        public overlay: Overlay,
        private overlayEventService: OverlayEventService
    ) {}

    public load(properties: PopoverProperties) {
        properties = this.applyPropertieDefaults(defaultProperties, properties);

        this.overlay.load({
            id: 'popover',
            mainComponent: PopoverComponent,
            childComponent: properties.component,
            width: properties.width,
            height: properties.height,
            left: properties.left,
            top: properties.top,
            animationDuration: properties.animationDuration,
            animationTimingFunction: properties.animationTimingFunction,
            animationTranslateY: properties.animationTranslateY,
            zIndex: properties.zIndex,
            data: properties.tooltipData,
            metadata: {
                placement: properties.placement,
                alignToCenter: properties.alignToCenter,
                event: properties.event,
                element: properties.element,
                offset: properties.offset,
                theme: properties.theme,
                popoverClass: properties.popoverClass,
                padding: properties.padding,
                noArrow: properties.noArrow
            }
        });
    }

    public close() {
        this.overlayEventService.emitChangeEvent({
            type: 'Hide'
        });
        this.emitClose('closed');
    }

    applyPropertieDefaults(defaultProperties, properties) {
        if (!properties) {
            properties = {};
        }

        for (var propertie in properties) {
            if (properties[propertie] === undefined) {
                delete properties[propertie];
            }
        }

        this._defaultProperties = Object.assign({}, defaultProperties);
        return Object.assign(this._defaultProperties, properties);
    }

    // Service message commands
    public emitClose(change: string) {
        this.emitChangeSource.next(change);
    }
}
