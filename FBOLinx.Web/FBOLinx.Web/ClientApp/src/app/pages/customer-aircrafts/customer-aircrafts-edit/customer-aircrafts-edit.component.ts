import { Component, EventEmitter, Input, Output, OnInit, Inject } from '@angular/core';

//Services
import { AircraftsService } from '../../../services/aircrafts.service';
import { AircraftpricesService } from '../../../services/aircraftprices.service';
import { CustomeraircraftsService } from '../../../services/customeraircrafts.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
    selector: 'app-customer-aircrafts-edit',
    templateUrl: './customer-aircrafts-edit.component.html',
    styleUrls: ['./customer-aircrafts-edit.component.scss']
})
/** customer-aircrafts-edit component*/
export class CustomerAircraftsEditComponent implements OnInit {

    @Output() saveEditClicked = new EventEmitter<any>();
    @Output() cancelEditClicked = new EventEmitter<any>();
    @Input() customerAircraftInfo: any;

    //Public Members
    public aircraftSizes: Array<any>;
    public aircraftTypes: Array<any>;
    public pricingTemplates: Array<any>;

    /** customer-aircrafts-edit ctor */
    constructor(private aircraftsService: AircraftsService,
        private aircraftPricesService: AircraftpricesService,
        private customerAircraftsService: CustomeraircraftsService,
        private pricingTemplateService: PricingtemplatesService, @Inject(MAT_DIALOG_DATA) public data: any,
        public dialogRef: MatDialogRef<CustomerAircraftsEditComponent>) {

    }

    ngOnInit() {
        if (this.data) {
            this.aircraftsService.get(this.data).subscribe((data: any) => {
                this.customerAircraftInfo = data;

                console.log(this.customerAircraftInfo);
            });
        };

        this.aircraftsService.getAll().subscribe((data: any) => this.aircraftTypes = data);
        this.aircraftsService.getAircraftSizes().subscribe((data: any) => this.aircraftSizes = data);
        //this.pricingTemplateService.getByFbo(this.sharedService.currentUser.fboId).subscribe((data: any) => {
        //    this.pricingTemplates = data;
        //    this.pricingTemplates.splice(0, 0, { oid: 0, name: '--Customer Default--' });
        //    if (!this.customerAircraftInfo.aircraftPrices)
        //        this.customerAircraftInfo.aircraftPrices = {};
        //    if (!this.customerAircraftInfo.aircraftPrices.PriceTemplateId)
        //        this.customerAircraftInfo.aircraftPrices.PriceTemplateId = 0;

        //});
        //this.aircraftPricesService.getAircraftPricesForCustomerAircraft(this.customerAircraftInfo.oid, this.sharedService.currentUser.fboId).subscribe((data: any) => {
        //    this.customerAircraftInfo.aircraftPrices = data;
        //    if (!this.customerAircraftInfo.aircraftPrices)
        //        this.customerAircraftInfo.aircraftPrices = {
        //            customerAircraftId: this.customerAircraftInfo.oid,
        //            oid: 0,
        //            priceTemplateId: 0
        //        };
        //});
    }

    public saveEdit() {
        this.customerAircraftsService.update(this.customerAircraftInfo).subscribe((data: any) => {
            //this.saveEditClicked.emit();
            
            this.dialogRef.close(data);
            //if (this.customerAircraftInfo.aircraftPrices.oid > 0) {

            //    this.aircraftPricesService.update(this.customerAircraftInfo.aircraftPrices).subscribe((data: any) =>
            //    {
            //        this.saveEditClicked.emit();
            //    });
            //} else {
            //    this.aircraftPricesService.add(this.customerAircraftInfo.aircraftPrices).subscribe((data: any) => {
            //        this.saveEditClicked.emit();
            //    });
            //}
        });
    }

    public cancelEdit() {
        this.data = null;
        this.dialogRef.close();
    }
}
