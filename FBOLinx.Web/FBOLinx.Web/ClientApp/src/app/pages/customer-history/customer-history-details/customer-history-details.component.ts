import { CustomerHistoryComponent } from '../customer-history/customer-history.component';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-customer-history-details',
  templateUrl: './customer-history-details.component.html',
  styleUrls: ['./customer-history-details.component.scss']
})
export class CustomerHistoryDetailsComponent implements OnInit {

    newCustomerInfoByGroup : any;
    oldCustomerInfoByGroup : any;
    oldContact : any;
    newContact : any;
    oldCustomerAircrafts : any;
    newCustomerAircrafts : any;
    oldAircraft : any;
    newAircraft : any;
    oldPricingTemplate : any;
    newPricingTemplate : any;

  constructor(public dialogRef: MatDialogRef<CustomerHistoryComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
        console.log(this.data)
           this.newCustomerInfoByGroup = this.data.newCustomerInfoByGroup;
           this.oldCustomerInfoByGroup =  this.data.oldCustomerInfoByGroup;
           this.oldCustomerAircrafts = this.data.oldCustomerAircrafts;
           this.newCustomerAircrafts =  this.data.newCustomerAircrafts;
           this.oldPricingTemplate = this.data.oldPricingTemplate;
           this.newPricingTemplate =  this.data.newPricingTemplate;
           this.oldContact = this.data.oldContact;
           this.newContact =  this.data.newContact;
           this.newAircraft = this.data.newAircaft;
           this.oldAircraft = this.data.oldAircaft;



  }



}
