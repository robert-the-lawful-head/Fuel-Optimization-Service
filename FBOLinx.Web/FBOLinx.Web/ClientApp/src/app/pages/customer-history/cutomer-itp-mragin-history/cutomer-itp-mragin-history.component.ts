import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-cutomer-itp-mragin-history',
  templateUrl: './cutomer-itp-mragin-history.component.html',
  styleUrls: ['./cutomer-itp-mragin-history.component.scss']
})
export class CutomerItpMraginHistoryComponent implements OnInit {

  @Input () oldPricingTemplate : any;
  @Input () newPricingTemplate : any;
  constructor() { }

  ngOnInit(): void {
      
  }

}
