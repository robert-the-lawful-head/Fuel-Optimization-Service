import {
    Component,
} from "@angular/core";
import { SharedService } from "../../../layouts/shared-service";
import { FbopricesService } from '../../../services/fboprices.service';

export interface TailLookupData {
    tailNumber: string;
    companyId: number;
    fuelVolume: number;
    icao: string;
}

export interface TailLookupResponse {
    pricePerGallon: number;
    fees: number;
    template: string;
    company: string;
    makeModel: string;
}

@Component({
    selector: "tail-lookup-tool",
    templateUrl: "./tail-lookup-tool.component.html",
    styleUrls: ["./tail-lookup-tool.component.scss"],
    providers: [SharedService],
})
export class TailLookupToolComponent {
    public aircraftTailNumber: any;
    public fuelVolume: any;
    public tailLookupData: TailLookupData;
    public tailLookupResponse: TailLookupResponse;
    constructor(private fbopricesService: FbopricesService, private sharedService: SharedService) { }

    LookupItem() {
        this.tailLookupData = {
            companyId: 0,
            icao: '',
            tailNumber: '',
            fuelVolume: 0
        };
        this.tailLookupData.companyId = this.sharedService.currentUser.fboId;
        this.tailLookupData.icao = this.sharedService.currentUser.icao;
        this.tailLookupData.tailNumber = this.aircraftTailNumber;
        this.tailLookupData.fuelVolume = this.fuelVolume;
        
        this.fbopricesService.getFuelPricesForCompany(this.tailLookupData).subscribe((data: TailLookupResponse) => {
            if (data) {
                this.tailLookupResponse = {
                    pricePerGallon: data.pricePerGallon,
                    fees: data.fees,
                    company: data.company,
                    makeModel: data.makeModel,
                    template: data.template
                }
                
            }
        });
        
    }
}
