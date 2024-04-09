import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-customer-info-by-group-history',
  templateUrl: './customer-info-by-group-history.component.html',
  styleUrls: ['./customer-info-by-group-history.component.scss']
})
export class CustomerInfoByGroupHistoryComponent implements OnInit {

  @Input() newCustomerInfoByGroup : any;
  @Input() oldCustomerInfoByGroup : any;
  constructor() { }

  ngOnInit(): void {
  }

}
