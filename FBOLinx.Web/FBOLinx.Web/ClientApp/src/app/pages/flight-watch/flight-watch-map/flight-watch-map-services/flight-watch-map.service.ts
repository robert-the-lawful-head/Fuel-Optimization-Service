import { Injectable } from '@angular/core';
import * as mapboxgl from 'mapbox-gl';
import { AIRCRAFT_IMAGES } from '../aircraft-images';

@Injectable({
    providedIn: 'root',
})
export class FlightWatchMapService {
    constructor() {}
    async loadAircraftIcons(map: mapboxgl.Map): Promise<void>{
        await AIRCRAFT_IMAGES.forEach( async (image,idx) => {

            let imageName = `aircraft_image_${image.id}`;
            var img1 =  this.addImageProcess(map,image,image.url,imageName,idx);

            imageName = `aircraft_image_${image.id}_reversed`;
            var img2 =  this.addImageProcess(map,image,image.reverseUrl,imageName,idx);

            imageName = `aircraft_image_${image.id}_release`;
            var img3 =  this.addImageProcess(map,image,image.blueUrl,imageName,idx);

            imageName = `aircraft_image_${image.id}_reversed_release`;
            var img4 =  this.addImageProcess(map,image,image.blueReverseUrl,imageName,idx);

            imageName = `aircraft_image_${image.id}_fuelerlinx`;
            var img5 =  this.addImageProcess(map,image,image.fuelerlinxUrl,imageName,idx);

            imageName = `aircraft_image_${image.id}_reversed_fuelerlinx`;
            var img6 =  this.addImageProcess(map,image,image.fuelerlinxReverseUrl,imageName,idx);
            
            const p = Promise.resolve([img1,img2,img3,img4,img5,img6]);
            p.then(function(v) {
            });
        });
    }
    addImageProcess(map: mapboxgl.Map, image: any, src: string,imageName:string,idx: number){
        return new Promise((resolve, reject) => {
          let img = new Image(image.size, image.size);
          img.onload = () => {
            map.addImage(imageName, img);
            resolve(imageName);
          };
          img.onerror = reject;
          img.src = src;
        })
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
