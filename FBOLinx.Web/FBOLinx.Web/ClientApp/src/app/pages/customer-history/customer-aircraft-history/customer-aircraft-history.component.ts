import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-customer-aircraft-history',
  templateUrl: './customer-aircraft-history.component.html',
  styleUrls: ['./customer-aircraft-history.component.scss']
})
export class CustomerAircraftHistoryComponent implements OnInit {

   @Input() oldCustomerAircrafts:any;
   @Input () newCustomerAircrafts : any;
   @Input () oldAircraft : any;
   @Input () newAircraft : any;
  constructor() { }

  ngOnInit(): void {
  }

}
