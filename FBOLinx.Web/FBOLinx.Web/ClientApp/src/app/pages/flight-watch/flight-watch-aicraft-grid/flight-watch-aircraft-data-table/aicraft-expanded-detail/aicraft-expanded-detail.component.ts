import { Component, Input, OnInit } from '@angular/core';
import { FlightWatchModelResponse } from 'src/app/models/flight-watch';
import { FlightWatchHelper } from '../../../FlightWatchHelper.service';
import { Swim } from 'src/app/models/swim';

@Component({
  selector: 'app-aicraft-expanded-detail',
  templateUrl: './aicraft-expanded-detail.component.html',
  styleUrls: ['./aicraft-expanded-detail.component.scss']
})
export class AicraftExpandedDetailComponent implements OnInit {
    @Input() data: FlightWatchModelResponse | any;
    @Input() isArrival: boolean;
    @Input() icao: string;
    @Input() fbo: string;

    constructor(private flightWatchHelper: FlightWatchHelper) { }

    ngOnInit() {
    }
    getOriginCityLabel(){
        return this.isArrival
        ? "Origin City"
        : "Destination City";
    }
    getPastArrivalsValue(row: FlightWatchModelResponse): number{
        return this.isArrival
            ? row.arrivals
            : row.departures;
    }
    getMakeModelDisplayString(element: FlightWatchModelResponse){
        if(element === null || element === undefined) return "TBD"
        let makemodelstr = (element.make) ?
            this.flightWatchHelper.getSlashSeparationDisplayString(element.make,element.model) :
            this.flightWatchHelper.getSlashSeparationDisplayString(element.faaMake,element.faaModel);

            return this.flightWatchHelper.getEmptyorDefaultStringText(makemodelstr);
    }
}
