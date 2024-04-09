import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-customer-contact-history',
  templateUrl: './customer-contact-history.component.html',
  styleUrls: ['./customer-contact-history.component.scss']
})
export class CustomerContactHistoryComponent implements OnInit {

    @Input() oldContact : any;
    @Input() newContact : any;
  constructor() { }

  ngOnInit(): void {
  }

}
