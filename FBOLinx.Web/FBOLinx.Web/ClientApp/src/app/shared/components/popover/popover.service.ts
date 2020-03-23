import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

import { PopoverComponent } from "./popover.component";
import {
    Overlay,
    OverlayProperties,
    EventService as OverlayEventService,
} from "@ivylab/overlay-angular";
import { PopoverProperties } from "./interfaces";
import { defaultProperties } from "./default-properties";

@Injectable()
export class Popover {
    popoverDefaultProps: OverlayProperties;
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
            id: "popover",
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
                noArrow: properties.noArrow,
            },
        });
    }

    public close() {
        this.overlayEventService.emitChangeEvent({
            type: "Hide",
        });
    }

    applyPropertieDefaults(dproperties, properties) {
        if (!properties) {
            properties = {};
        }

        for (const propertie in properties) {
            if (properties[propertie] === undefined) {
                delete properties[propertie];
            }
        }

        this.popoverDefaultProps = Object.assign({}, dproperties);
        return Object.assign(this.popoverDefaultProps, properties);
    }

    // Service message commands
    public emitClose(change: string) {
        console.log(change);
        this.emitChangeSource.next(change);
    }
}
