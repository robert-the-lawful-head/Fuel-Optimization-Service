import { Injectable } from '@angular/core';
import * as mapboxgl from 'mapbox-gl';
import { AIRCRAFT_IMAGES } from '../aircraft-images';

@Injectable({
    providedIn: 'root',
})
export class FlightWatchMapService {
    constructor() {}
    async loadAircraftIcons(map: mapboxgl.Map): Promise<void>{
        AIRCRAFT_IMAGES.forEach(async (image) => {
            const img = new Image(image.size, image.size);

            let imageName = `aircraft_image_${image.id}`;
            await this.loadImageInMap(map, img, imageName, image.url);

            const reversedImg = new Image(image.size, image.size);
            imageName = `aircraft_image_${image.id}_reversed`;
            await this.loadImageInMap(map, reversedImg, imageName, image.reverseUrl);

            const releaseImg = new Image(image.size, image.size);
            imageName = `aircraft_image_${image.id}_release`;
            await this.loadImageInMap(map, releaseImg, imageName, image.blueUrl);

            const releaseReversedImg = new Image(image.size, image.size);
            imageName = `aircraft_image_${image.id}_reversed_release`;
            await this.loadImageInMap(
                map,
                releaseReversedImg,
                imageName,
                image.blueReverseUrl
            );

            const fuelerlinxImg = new Image(image.size, image.size);
            imageName = `aircraft_image_${image.id}_fuelerlinx`;
            await this.loadImageInMap(
                map,
                fuelerlinxImg,
                imageName,
                image.fuelerlinxUrl
            );

            const fuelerlinxReversedImg = new Image(image.size, image.size);
            imageName = `aircraft_image_${image.id}_reversed_fuelerlinx`;
            await this.loadImageInMap(
                map,
                fuelerlinxReversedImg,
                imageName,
                image.fuelerlinxReverseUrl
            );
        });
    }
    async loadImageInMap(
        map: mapboxgl.Map,
        image: any,
        imageName: string,
        imageUrl: string
    ): Promise<void> {
        image.onload = async () => {
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
