import { Injectable } from '@angular/core';
import * as mapboxgl from 'mapbox-gl';
import { AIRCRAFT_IMAGES } from '../aircraft-images';

@Injectable({
    providedIn: 'root',
})
export class FlightWatchMapService {
    constructor() {}
    loadAircraftIcons(map: mapboxgl.Map): void{
        AIRCRAFT_IMAGES.forEach( (image) => {
            const img = new Image(image.size, image.size);

            let imageName = `aircraft_image_${image.id}`;
            this.loadImageInMap(map, img, imageName, image.url);

            const reversedImg = new Image(image.size, image.size);
            imageName = `aircraft_image_${image.id}_reversed`;
            this.loadImageInMap(map, reversedImg, imageName, image.reverseUrl);

            const releaseImg = new Image(image.size, image.size);
            imageName = `aircraft_image_${image.id}_release`;
            this.loadImageInMap(map, releaseImg, imageName, image.blueUrl);

            const releaseReversedImg = new Image(image.size, image.size);
            imageName = `aircraft_image_${image.id}_reversed_release`;
            this.loadImageInMap(
                map,
                releaseReversedImg,
                imageName,
                image.blueReverseUrl
            );

            const fuelerlinxImg = new Image(image.size, image.size);
            imageName = `aircraft_image_${image.id}_fuelerlinx`;
            this.loadImageInMap(
                map,
                fuelerlinxImg,
                imageName,
                image.fuelerlinxUrl
            );

            const fuelerlinxReversedImg = new Image(image.size, image.size);
            imageName = `aircraft_image_${image.id}_reversed_fuelerlinx`;
            this.loadImageInMap(
                map,
                fuelerlinxReversedImg,
                imageName,
                image.fuelerlinxReverseUrl
            );
        });
    }
    loadImageInMap(
        map: mapboxgl.Map,
        image: any,
        imageName: string,
        imageUrl: string
    ): void {
        image.onload = () => {
            map.addImage(imageName, image);
            image.src = imageUrl;
        };
        
    }
    public getDefaultAircraftType(atype: string): string {
        if (!AIRCRAFT_IMAGES.find((ai) => ai.id === atype)) {
            atype = 'default';
        }
        return atype;
    }
    public getGeojsonFeatureSourceJsonData(
        features: any[]
    ): mapboxgl.AnySourceData {
        return {
            type: 'geojson',
            data: {
                type: 'FeatureCollection',
                features: features,
            },
        };
    }
    public buildAircraftId(aircraftId: number): string {
        return `aircraft_${aircraftId}`;
    }
}
