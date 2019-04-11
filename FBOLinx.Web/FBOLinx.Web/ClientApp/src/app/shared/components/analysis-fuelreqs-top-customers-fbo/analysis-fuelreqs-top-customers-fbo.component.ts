import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

//Services
import { FuelreqsService } from '../../../services/fuelreqs.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import * as moment from 'moment';

@Component({
    selector: 'app-analysis-fuelreqs-top-customers-fbo',
    templateUrl: './analysis-fuelreqs-top-customers-fbo.component.html',
    styleUrls: ['./analysis-fuelreqs-top-customers-fbo.component.scss']
})
/** analysis-fuelreqs-top-customers-fbo component*/
export class AnalysisFuelreqsTopCustomersFboComponent implements OnInit {

    public topCustomersData: Array<any>;

    /** analysis-fuelreqs-top-customers-fbo ctor */
    constructor(private router: Router,
        private fuelreqsService: FuelreqsService,
        private sharedService: SharedService) {

    }

    ngOnInit() {
        let startDate = moment().add(-6, 'M').format('MM/DD/YYYY');
        let endDate = moment().format('MM/DD/YYYY');
        this.fuelreqsService.topCustomersForFbo(this.sharedService.currentUser.fboId, {startDateTime: startDate, endDateTime: endDate}).subscribe((data: any) => {
            this.topCustomersData = data;
        });
    }

    public viewAllCustomersClicked() {
        this.router.navigate(['/default-layout/customers/']);
    }
}
