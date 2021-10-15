import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-customer-history',
  templateUrl: './customer-history.component.html',
  styleUrls: ['./customer-history.component.scss']
})
export class CustomerHistoryComponent implements OnInit {
    public customerHistoryDataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = [
        'Time (UTC)',
        'Action',
        'Changes',
        'User',
        'Role',
        'Location'
    ];
    public resultsLength = 0;
    public aircraftSizes: Array<any>;
    public aircraftTypes: Array<any>;
    public isLoadingAircraftTypes = false;
    public pageIndex = 0;


  constructor() { }

  ngOnInit() {
  }

  public applyFilter(filterValue: string) {
    this.customerHistoryDataSource.filter = filterValue
        .trim()
        .toLowerCase();
}

  onPageChanged(e: any) {
    sessionStorage.setItem('pageIndex', e.pageIndex);
}
}
