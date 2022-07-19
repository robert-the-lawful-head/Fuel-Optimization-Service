import { ElementRef } from '@angular/core';
import { timeStamp } from 'console';
import * as mapboxgl from 'mapbox-gl';
import { stringify } from 'querystring';
import { FlightWatch } from 'src/app/models/flight-watch';
import { AircraftPopupContainerComponent } from 'src/app/pages/flight-watch/aircraft-popup-container/aircraft-popup-container.component';
import { AircraftImageData } from 'src/app/pages/flight-watch/flight-watch-map/aircraft-images';
import { FlightWatchMapComponent } from 'src/app/pages/flight-watch/flight-watch-map/flight-watch-map.component';
import { environment } from 'src/environments/environment';
export interface PopUpProps{
    isPopUpOpen: boolean;
    coordinates: [number,number];
    popupId: number;
}
export abstract class MapboxglBase {
    public map: mapboxgl.Map;
    public currentPopup: PopUpProps;
    buildMap(
        center: mapboxgl.LngLatLike,
        container: string,
        style: string,
        zoom: number = 12,
        optimizeMap: boolean = false
    ) {
        this.currentPopup= {
            isPopUpOpen: false,
            popupId: null,
            coordinates: null,
        }
        let optimizeMapFlag = (optimizeMap)?'?optimize=true':'';
        this.map = new mapboxgl.Map({
            container: container,
            style: style + optimizeMapFlag,
            zoom: zoom,
            center: center,
            accessToken: environment.mapbox.accessToken,
        });

        return this;
    }
    addNavigationControls(): this{
        this.map.addControl(new mapboxgl.NavigationControl());
        return this;
    }
    addGeolocationControls(): this{
        this.map.addControl(new mapboxgl.GeolocateControl({
            positionOptions: {
            enableHighAccuracy: true
            },
            // When active the map will receive updates to the device's location as it changes.
            trackUserLocation: true,
            // Draw an arrow next to the location dot to indicate which direction the device is heading.
            showUserHeading: true
            }));
        return this;
    }
    onZoomAsync(callBack): this{
        this.map.on('zoom', async () => callBack);
        return this;
    }
    onDragendAsync(callBack): this{
        this.map.on('dragend', async () => callBack);
        return this;
    }
    onRotateAsync(callBack): this{
        this.map.on('rotate', async () => callBack);
        return this;
    }
    onResizeAsync(callBack): this{
        this.map.on('resize', async () => callBack);
        return this;
    }
    onIdleAsync(callBack): this{
        this.map.on('idle', async () => callBack);
        return this;
    }
    onStyleDataAsync(callBack): this{
        this.map.on('styledata', async () => callBack);
        return this;
    }
    onLoad(callBack): this{
        this.map.on('load', callBack);
        return this;
    }
    mapResize(): this{
        this.map.resize();
        return this;
    }
    getStyle(): mapboxgl.Style{
        return this.map.getStyle();
    }
    mapRemove(): void{
        this.map.remove();
    }
    getBounds() {
        return this.map.getBounds()
    }

    onClick(elementId:string,callBack): void{
        this.map.on('click', elementId, callBack);
    }
    setLayoutProperty(layer: string, name: string, value: any): void{
        this.map.setLayoutProperty(layer, name, value);
    }
    getLayoutProperty(layer: string, name: string){
        return this.map.getLayoutProperty(layer, name);
    }
    getSource(sourceId: string): mapboxgl.GeoJSONSource {
        return this.map.getSource(sourceId) as mapboxgl.GeoJSONSource
    }
    addSource(id: string, sourceData: mapboxgl.AnySourceData): void {
        this.map.addSource(id, sourceData);
    }
    addLayer(layerData: mapboxgl.AnyLayer): void {
        this.map.addLayer(layerData);
    }
    removeLayer(id: string): void {
        this.map.removeLayer(id);
    }
    removeSource(id: string): void {
        this.map.removeSource(id);
    }
    removeMouseHoverActions(id: string): void{
        this.map.off('mouseenter', id, () => this.cursorPointer('pointer'));
        this.map.off('mouseleave', id, () => this.cursorPointer(''));
    }
    addHoverPointerActions(id: string):void {
        this.map.on('mouseenter', id, () => this.cursorPointer('pointer'));
        this.map.on('mouseleave', id, () => this.cursorPointer(''));
    }
    cursorPointer(cursor: string): void{
        this.map.getCanvas().style.cursor = cursor;
    }
    createPopUpOnMouseEnterFromDescription(layerId: string): void{
        // Create a popup, but don't add it to the map yet.
        const popup = new mapboxgl.Popup({
            closeButton: false,
            closeOnClick: false,
        });

        this.map.on('mouseenter', layerId, (e) => {
            // Change the cursor style as a UI indicator.
            this.map.getCanvas().style.cursor = 'pointer';

            // Copy coordinates array.
            const coordinates = e.lngLat;
            const description = e.features[0].properties.description;

            // Ensure that if the map is zoomed out such that multiple
            // copies of the feature are visible, the popup appears
            // over the copy being pointed to.
            while (Math.abs(e.lngLat.lng - coordinates[0]) > 180) {
                coordinates[0] += e.lngLat.lng > coordinates[0] ? 360 : -360;
            }

            // Populate the popup and set its coordinates
            // based on the feature found.
            popup.setLngLat(coordinates).setHTML(description).addTo(this.map);
        });

        this.map.on('mouseleave', layerId, () => {
            this.map.getCanvas().style.cursor = '';
            popup.remove();
        });
    }
    createPopUpOnClickFromDescription(layerId: string): void{
        // When a click event occurs on a feature in the places layer, open a popup at the
        // location of the feature, with description HTML from its properties.
        this.map.on('click', layerId, (e) => {

            // Copy coordinates array.
            const coordinates = e.features[0].geometry['coordinates'].slice();
            const description = e.features[0].properties.description;
            // Ensure that if the map is zoomed out such that multiple
            // copies of the feature are visible, the popup appears
            // over the copy being pointed to.
            while (Math.abs(e.lngLat.lng - coordinates[0]) > 180) {
                coordinates[0] += e.lngLat.lng > coordinates[0] ? 360 : -360;
            }

            new mapboxgl.Popup()
                .setLngLat(coordinates)
                .setHTML(description)
                .addTo(this.map);
        });

        // Change the cursor to a pointer when the mouse is over the places layer.
        this.map.on('mouseenter', layerId, () => {
            this.map.getCanvas().style.cursor = 'pointer';
        });

        // Change it back to a pointer when it leaves.
        this.map.on('mouseleave', layerId, () => {
            this.map.getCanvas().style.cursor = '';
        });
    }
    closeAllPopUps(){
        const elements = Array.from(document.getElementsByClassName('mapboxgl-popup'));
        elements.forEach(elem => {
            elem.remove();
        });
    }
    loadPNGImageAsync(src: string,imageId: string){
        return new Promise((resolve, reject) => {
            this.map.loadImage(src,
                (error, image) => {
                if (error) reject(imageId);
                this.map.addImage(imageId, image);
                resolve(imageId);
            });
          });
    }
    loadSVGImageAsync(width:number, height:number, src: string, imageName:string){
        return new Promise((resolve, reject) => {
            let img = new Image(width, height);
            img.onload = () => { 
              this.map.addImage(imageName, img);
              resolve(imageName);
            };
            img.onerror = reject;
            img.src = src;
        });
    }
    openPopupRenderComponent(coordinates: [number,number],elemRef: ElementRef,currentPopup: PopUpProps):void{
        new mapboxgl.Popup()
            .setLngLat(coordinates)
            .setDOMContent(elemRef.nativeElement)
            .setMaxWidth("330px")
            .addTo(this.map)  
            .on('close', function(e) {
                currentPopup.isPopUpOpen = false;
            });
    }
    flyTo(center: mapboxgl.LngLatLike,hasAnimation = true){
        this.map.flyTo({
            center: center,
            essential: hasAnimation // this animation is considered essential with respect to prefers-reduced-motion
        });
    }
}
