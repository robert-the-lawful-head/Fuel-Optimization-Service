import { ElementRef } from '@angular/core';
import { Dictionary } from 'lodash';
import * as mapboxgl from 'mapbox-gl';
import { environment } from 'src/environments/environment';
export interface PopUpProps{
    coordinates: [number,number];
    popupId: string;
    popupInstance: mapboxgl.Popup
    isOpen: boolean;
}
export abstract class MapboxglBase {
    public map: mapboxgl.Map;
    public openedPopUps: Dictionary<PopUpProps> = {};
    public popUpPropsNewInstance : PopUpProps = {
        coordinates: [0,0],
        popupId: '',
        popupInstance: null,
        isOpen: true
    };
    buildMap(
        center: mapboxgl.LngLatLike,
        container: string,
        style: string,
        zoom: number = 12,
        optimizeMap: boolean = false
    ) {
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
        this.map.addControl(new mapboxgl.NavigationControl(), 'top-left');
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
    /* start on functions */
    onZoomStart(callBack): this{
        this.map.on('zoomstart',  async() => callBack);
        return this;
    }
    onZoomEnd(callBack): this{
        this.map.on('zoomend', callBack);
        return this;
    }
    onZoom(callBack): this{
        this.map.on('zoom', callBack);
        return this;
    }
    onDragend(callBack): this{
        this.map.on('dragend', callBack);
        return this;
    }
    onRotate(callBack): this{
        this.map.on('rotate', callBack);
        return this;
    }
    onResize(callBack): this{
        this.map.on('resize', callBack);
        return this;
    }
    onIdle(callBack): this{
        this.map.on('idle', callBack);
        return this;
    }
    onStyleData(callBack): this{
        this.map.on('styledata', callBack);
        return this;
    }
    onStyleLoad(callBack): this{
        this.map.on('style.load', callBack);
        return this;
    }
    onLoad(callBack): this{
        this.map.on('load', callBack);
        return this;
    }
    onClick(elementId:string,callBack): this{
        this.map.on('click', elementId, callBack);
        return this;
    }
    onceClick(elementId:string,callBack): this{
        this.map.once('click', elementId, callBack);
        return this;
    }
    onMapClick(callBack): this{
        this.map.on('click', callBack);
        return this;
    }
    onSourcedata(callBack): this{
        this.map.on('sourcedata', callBack);
        return this;
    }
    onReady(callBack): this{
        this.map.on('ready', callBack);
        return this;
    }
    /* end on functions */

    /* start get functions */
    getBounds(): mapboxgl.LngLatBounds {
        return this.map.getBounds();
    }
    getStyle(): mapboxgl.Style{
        return this.map.getStyle();
    }
    getLayoutProperty(layer: string, name: string): any{
        return this.map.getLayoutProperty(layer, name);
    }
    getSource(sourceId: string): mapboxgl.GeoJSONSource {
        return this.map.getSource(sourceId) as mapboxgl.GeoJSONSource
    }
    getLayer(layerId: string): mapboxgl.AnyLayer{
        return this.map.getLayer(layerId);
    }
    /* ends get functions */

    /* start set functions */
    setStyle(style: string): void {
        this.map.setStyle(style);
    }
    setLayoutProperty(layer: string, name: string, value: any): void{
        this.map.setLayoutProperty(layer, name, value);
    }
    setPaintProperty(layerId: string, name: string,  value: string): void{
        this.map.setPaintProperty(layerId, name, value);
    }
    /* end set functions */

    mapResize(): this{
        this.map.resize();
        return this;
    }
    mapRemove(): void{
        this.map.remove();
    }
    addSource(id: string, sourceData: mapboxgl.AnySourceData): void {
        this.map.addSource(id, sourceData);
    }
    addLayer(layerData: mapboxgl.AnyLayer): void {
        this.map.addLayer(layerData);
    }
    removeLayer(layerId: string): void {
        this.map.removeLayer(layerId);
    }
    removeSource(sourceId: string): void {
        this.map.removeSource(sourceId);
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
    async loadPNGImageAsync(src: string,imageId: string){
        return new Promise((resolve, reject) => {
            this.map.loadImage(src,
                (error, image) => {
                if (error) reject(imageId);
                this.map.addImage(imageId, image);
                resolve(imageId);
            });
          });
    }
   async loadSVGImageAsync(width:number, height:number, src: string, imageName:string){
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
    async getFncAsAsync(callBack): Promise<unknown>{
        return new Promise((resolve, reject) => {
            resolve(callBack);
          });
    }
    openPopupRenderComponent(coordinates: [number,number],elemRef: ElementRef):mapboxgl.Popup{
        return new mapboxgl.Popup()
            .setLngLat(coordinates)
            .setDOMContent(elemRef.nativeElement)
            .setMaxWidth("450px")
            .addTo(this.map);
    }
    openPopupRenderHtml(coordinates: [number,number],html: string):mapboxgl.Popup{
        return new mapboxgl.Popup()
            .setLngLat(coordinates)
            .setHTML(html)
            .setMaxWidth("450px")
            .addTo(this.map);
    }
    flyTo(center: mapboxgl.LngLatLike,hasAnimation = true){
        this.map.flyTo({
            zoom: 13,
            center: center,
            essential: hasAnimation // this animation is considered essential with respect to prefers-reduced-motion
        });
    }
}
